using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/* 	@ HUD-UI>InfoScreen>WaveSpawner
	Des: displaying the wave number on the ui text element
*/

public class WaveController : MonoBehaviour {

	public Text Wave;	//adding the UI-Text 
	EnemySpawner ES;	//getting the EnemySpawner Script

	void Start () {
		ES = GameObject.Find ("Path").GetComponent<EnemySpawner>();
		Wave.text = ES.WaveNumber.ToString ();
	}	

	void Update () {	
		// FIXME: a better optimized way like @LifeConroller.cs
		Wave.text = ES.WaveNumber.ToString ();

	}
}
