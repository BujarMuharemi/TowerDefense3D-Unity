using UnityEngine;
using System.Collections;
using System;

/* 	@ Bullet.Prefab
	Des: moving & destroying of bullets....	
	
*/

public class BulletMovement : MonoBehaviour {

	public bool slowDown=false;
	public Vector3 Target;	//getting the Vector of the target
	TurrentScript TS;
	public float speed = 12f; 
	public bool LGM; //used to determin if the type of bullet should be an "LaserGuidedMissled" or not
	float killTimer=0;	//used to count the time to kill the bullet
	
	int cnt = 0;
	Vector3 tempTarget;
	
	void Start(){
	 	TS = transform.parent.GetComponent<TurrentScript> ();	//getting reference to turrent script , to acces hit var
	}

	void Update () {
		killTimer += Time.deltaTime;	//increasing timer

		float step = speed * Time.deltaTime;
		tempTarget=Target;	
		
		//TODO: here we need to get the bullet to fly even after it hits the target -> get direction of flight....
		
		if(LGM){
			Vector3 direction = Vector3.MoveTowards (transform.position, tempTarget, step);	//moving bullet towards enemy
			Vector3 targetDir = direction - transform.position;
			transform.position = direction;
			//Debug.DrawLine(transform.position, targetDir, Color.blue);
		}
		
		if(!LGM){
			cnt++;
			if(cnt==1){
				tempTarget = Target;
			}
			Vector3 direction = Vector3.MoveTowards (transform.position, tempTarget, step);	//moving bullet towards enemy
			transform.position = direction;
			
		}
		
		//print(killTimer);

		float distBE = Vector3.Distance (transform.position,Target);	//distance Bullet-Enemy
		
		//bool a = TS.EnemyHolder.transform.childCount!=0;

		if (distBE<0.3f && TS.EnemyHolder.transform.childCount!=0) {	//checking for collision of bullet and enemy

			//This try&catch is used because somtimes a enemy gets destroy by another bullet
			//and then if some other bullets try that to thy can't because it already delted
			try {
				EnemyMovement EM = TS.child.GetComponent<EnemyMovement>();	//getting the script of the enemy
				if(slowDown){
					//EM.speed-=0.25f;
					EM.GetSlower();
				}else{
					//EM.health-=TS.damage;	//changing the health of the enemy if hit
					EM.LoseHealth(TS.damage);
				}

			}
			catch{
				print ("tried to kill child at birth ? ....spawnKill");
			}
			
			Destroy (gameObject); //killing bullet
		}
		//NOTE:not used
		//if the bullet has exsisted for more than one second , it gets destroyed
		if (killTimer > 10f) {
			//print ("I didn't hit anything !");
			//Destroy (gameObject);
		}

	}
	
	void OnCollisionEnter(Collision other){
		print(other.gameObject.name);
		print("asdf");
	}
	
	


}
