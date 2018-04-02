using UnityEngine;
using System.Collections;
using UnityEngine.UI;
/* 	@ Enemy.Prefab
	Des: -moving the enemy from one node to another
		 -handling money on the ui side
*/

public class EnemyMovement : MonoBehaviour {

	int counter =2;
	public  float speed =1f;	//for use in the WaveSpawner 
	public float health = 30f;
	public int reward = 25;
	float healthFinal;	//used for calculating the size of the healthbar

	float timeR = 0;
	bool slower = false;
	bool slowedDown =false;
	float slowDownSpeed = 0.5f;
	float originalSpeed;
	float slowDownTime = 5.0f;

	LifeController LC;
	MoneyController Money;
	Image HealthBar;

	void Start(){
		healthFinal = health;
		Money = GameObject.Find ("Money").GetComponent<MoneyController>();	//getting the MoneyController SCript
		LC= GameObject.Find("Life").GetComponent<LifeController>();	//same for life
		HealthBar = transform.GetChild(0).GetComponent<Image>();

		originalSpeed = speed;
	}

	void Update () {	

		//geting the vector of the targe(from the pathNodes)
		Vector3 target = EnemySpawner.pathNodes [counter].transform.position;
		
		//setting the y heigher so the enemy moves over the nodes
		target.y = EnemySpawner.pathNodes [0].transform.position.y;	

		//for slowing down the enemy but just for a certain amout of time and the back to the original speed
		if (slower) {
			timeR += Time.deltaTime;

			if (timeR > slowDownTime) {
				speed = originalSpeed;
				slower = false;
				slowedDown = false;
				timeR = 0f;

			} else if(!slowedDown) {	//here is the enemy in SlowMode TODO:add particle Effet or similar to show that this enemy 
				speed -= slowDownSpeed;	// is slower
				slowedDown = true;
			}

		}

		//the setp for the .MoveTowards method
		float step = speed * Time.deltaTime;

		//moving enemy towars node
		Vector3 direction = Vector3.MoveTowards (transform.position, target, step);

		transform.position = direction;

		//checking if the enemy has reached its targed True-> set up next node as target
		if (transform.position == target && counter< EnemySpawner.pathNodes.Length-1) {
			counter++;
		}
		//Destroying enemy if it has reached the last node
		if (counter == EnemySpawner.pathNodes.Length-1) {
			//LC.Health -= (int)health/5;	//here we substract the health of the player
			LC.Health--;
			Destroy (gameObject);
		} 

		//killing itself if health is 0 ...
		if (health <= 0) {
			//Money.bulletKilledEnemy = true;	//setting the flag for the script
			Money.addMoney(reward); //improved way of adding money 
			Die();		
		}


	}

	void OnCollisionEnter(Collision other){
		print(other.gameObject.name);
	}

	public void LoseHealth(float ammount){		
		health -= ammount;
		HealthBar.fillAmount = health / healthFinal;
	}

	public void Die(){
		//here it dies and an particle effect spawns at this place
		Destroy (gameObject);
	}

	public void GetSlower(){
		slower = true;
	}
		
}
