using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/* 	@ HUD-UI>InfoScreen>Life
	Des: displaying the "Health" of the player
*/

public class LifeController : MonoBehaviour {

	public int Health = 100;
	public Text HealthText; //UI element which was added threw Editor
	int oldHealth;
	GameControll GC;

	void Start () {
		HealthText.text = Health.ToString();
		oldHealth = Health;
		GC = GameObject.Find ("GameMASTER").GetComponent<GameControll>();
	}
	
	void Update () {
		if (oldHealth != Health) {
			HealthText.text = Health.ToString();
			oldHealth = Health;
		}

		if (Health == 0) {
			//print ("Game Over");
			GC.GameOver ();
			//TODO: later here you can call a function of GameController 
			// which then ends the game and shows UI etc..
		}
	}
}
