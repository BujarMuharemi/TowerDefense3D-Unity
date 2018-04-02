using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/* 	@ "Every type of turrent".Prefab
	Des: the AI of the turrents
*/

public class TurrentScript : MonoBehaviour {
	
	/*
		BUG#2: the bullets of the turrent must damage every enemy they hit , not just the ones for which they are aimed for
	*/

	public GameObject EnemyHolder;	//for storing all the spawned enemies so we can loop threw them

	public Transform child;

	float dist;	//the distance to the enemy
	int currentEnemy=0;	//index of the current enemy, on which the turrent should lock on

	public float range = 4f;	//the range in which the turrent shoots the enemy  
	public float fireRate = 1f; //the rate of shoot rounds per second
	public float damage = 10f; //the damage it makes to the enemy
	public int price = 100;	//price to pay for placing a turrent
	public bool inRange;

	public int Level = 1;

	float cooldownTimer=0;	// a simple timer for the cooldown
	GameObject[] Enemies;	//array for saving the enemy gameObjects
	
	GameObject tempChild;

	public bool SlowBullet=false;

	GameObject bullet;

	bool showDialog=false;
	GameObject dialog=null;
	TurrentDialog dialogScipt=null;
	Transform temp;
	GameObject[] dias;

	bool upgraded=false;

	public float upgradePrice=0;
	public float sellPrice=0;

	MoneyController money;
	EnemySpawner ES;

	void Start () {
		EnemyHolder = GameObject.Find ("EnemyHolder");	//getting the enemyHolder gameObject
		InvokeRepeating("UpdateEnemies",0f,0.1f);	//updating the array of enemies , every second-> not in Update()-> for better performance												
		UpdateEnemies();

		upgradePrice = price * 1.5f;
		sellPrice = price * 0.5f;

		money = GameObject.Find ("Money").GetComponent<MoneyController>();

		ES = GameObject.Find ("Path").GetComponent<EnemySpawner> ();
	}
	

	void Update () {		
		cooldownTimer += Time.deltaTime;	//counting the seconds

		//checking if even an enemy has spawned
		if (Enemies.Length !=null)
		{	//child count starts from 1 not 0

			//looping threw every enemie and checking which is in range->true->that enemy becomes the target
			for (int i = 0; i < Enemies.Length-1; i++) {
				UpdateEnemies();
				tempChild = EnemyHolder.transform.GetChild(i).gameObject;

				float tempDist = Vector3.Distance (transform.position,tempChild.transform.position); //getting the distance between enemy and turrent
				
				if (tempDist < range) {	//if enemy is in range
					//print ("Distance: " + dist +" child No."+EnemyHolder.transform.childCount);
					child=tempChild.transform;				
					//Debug.DrawLine(transform.position, tempChild.transform.position, Color.yellow);
					inRange=true;
					break;				
				}

				if(tempDist > range){	//if enemy is out of range
					inRange=false;
				}
			}

			//sometimes this can be true if a Turrent instances trys to acces a enemy which was already destroyed by another turrent
			if (child != null && ES.spawnWave) 
			{
				dist = Vector3.Distance (transform.position,child.transform.position); //getting the distance between enemy and turrent
				
				var rotation = Quaternion.LookRotation(child.transform.position - transform.position);
				transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 16.0f);
				
								
				if (dist < range)
				{	//if enemy is in range

					//get the rotation the turrent has to make to look at the enemy
					//var rotation = Quaternion.LookRotation(child.transform.position - transform.position);
					//transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 6.0f);
					//print("Rotation " + rotation.ToString());
					//transform.LookAt (child.transform.position, Vector3.up);	//looking at the enemy(Locking on him)
					
					if(tempChild != null){
						Debug.DrawLine(transform.position, child.transform.position, Color.yellow);
					}
					
					//shooting rate and fire etc...
					if (cooldownTimer > fireRate)
					{
						Shoot (child.gameObject);
						cooldownTimer = 0;
					}
				}
				

			}
						

		}
		//calls the upgradelevel method from the dialogscipt
		if (dialogScipt != null) {
			dialogScipt.sellPrice = (int)sellPrice;

			if (dialogScipt.upgrade) {				
				UpgradeLevel();
			}
			if (dialogScipt.sell) {
				SellTurrent();
			}


			if (!upgraded) {
				sellPrice = price/2;
			} else if(upgraded) {
				sellPrice = (int)(price + upgradePrice) / 2;
			}

		}

	}

	//method used for shooting the bullet and the handeling of it
	void Shoot(GameObject target){

		//getting the transform of the FirePoint(end of the barrel @ turrent)
		Transform FirePoint = transform.GetChild (1);
		FirePoint = FirePoint.transform.GetChild (0);
		Vector3 temp = FirePoint.transform.position;
		
		//TODO: here we have to change the type of bullet for each given turrent (flag or somethig similar)


		if(SlowBullet){
			bullet = (GameObject)Instantiate (Resources.Load ("Bullet_Slow"));	//spawning bullet
		}else{
			bullet = (GameObject)Instantiate (Resources.Load ("Bullet"));	//spawning bullet		
		}


		bullet.transform.parent = transform;	//setting the bullet as a child of the turrent prefab
		BulletMovement bulletScript = bullet.GetComponent<BulletMovement>();	//script Referenc to bulletMove...

		bullet.transform.position = temp;	//positioning bullet @ barrel end

		bulletScript.Target = target.transform.position; //setting the target of the bullet

	}

	//used to update the array of enemies , so we know which enemy died etc.
	void UpdateEnemies(){
		Enemies = new GameObject[EnemyHolder.transform.childCount];

		//saving all the actual enemies in the array
		for (int i = 0; i < Enemies.Length; i++) {
			Enemies [i] = EnemyHolder.transform.GetChild(i).gameObject;

			child = Enemies[i].transform;	//used so that the child is never null
		}

	}


	void OnMouseDown(){
		//creating an transform on the pos of the turrent so the UI-dialog doesn't rotate with the turrent
		dias = GameObject.FindGameObjectsWithTag("TurrentDialog");
		foreach (GameObject dias in dias) {
			//print (dias.name+"->"+dias.gameObject.transform.childCount);
		}

		showDialog = !showDialog;

		if (showDialog) {			

			temp = transform;
			temp.transform.rotation.Set (0f, 0f, 0f, 0f);

			//dialog gets spawned here
			dialog = (GameObject)Instantiate (Resources.Load ("TurrentDialog"));

			//print (transform.position);

			dialog.transform.position = transform.position;

			//print (dialog.transform.position);

			//dialog.transform.SetParent (GameObject.Find("MASTER-UI").transform);
			Canvas dialogCanvas = dialog.GetComponent<Canvas> ();

			Camera cam = GameObject.Find ("Camera").GetComponent<Camera> ();
			dialogCanvas.worldCamera = cam;

			dialog.transform.position = temp.transform.position;
			Vector3 finalPos = dialog.transform.position;
			finalPos.y += 2.0f;
			finalPos.z += 1.0f;

			dialog.transform.position = finalPos;

			//getting the turrentdialog script
			dialogScipt = dialog.GetComponent<TurrentDialog> ();

			if (!upgraded && dialogScipt!= null) {				
				dialogScipt.setUpgradePrice ((int)upgradePrice, Level);

			}
			if(upgraded && dialogScipt!= null){
				//this is important don't delte or it will trigger #8-UpgradeText Bug
				dialogScipt.setUpgradePrice (50, Level);
			}

		}
		else if (!showDialog) {
			if (dialog != null) {			
				Destroy (dialog);
			}
		}

	}

	void UpgradeLevel(){		
		if (Level == 1) {
			Level++;
		}

		if (Level == 2 && !upgraded) {								
				//money.Money -= (int)upgradePrice;
				
				if (!money.noMoney) {
				
					money.addMoney(-1*(int)upgradePrice);

					if (!money.noMoney) {
						dialogScipt.Upgrade ();
						//Upgrade Stats -> NOTE: should be public so you can edit them threw the editor
						range *= 1.2f;
						damage *= 1.2f;
						fireRate *= 1.2f;

						Vector3 tempScale = transform.localScale;
						tempScale *= 1.5f;
						transform.localScale = tempScale;
						upgraded = true;
					} else {
						Level = 1;
					}
				}

			print (money.noMoney);

		} 

		dialogScipt.upgrade = false;
	}

	void SellTurrent(){		
		money.addMoney ((int)sellPrice);

		Destroy (transform.gameObject);
	}


}
