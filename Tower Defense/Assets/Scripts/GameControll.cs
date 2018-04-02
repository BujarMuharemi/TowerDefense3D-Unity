using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/* 	@ GameMaster
	Des: used to controll the whole game-level loading,pausing etc..
*/

public class GameControll : MonoBehaviour {

	//vars for fastforward and playpause buttons
	bool faster=false;
	public bool pause=false;
	float lastScale=1;

	int currentLevel=0;

	GameObject Events;
	GameObject GameOverPanel;
	GameObject WinLevelPanel;
	GameObject StartScreen;
	GameObject LevelSelect;
	GameObject Fade;

	GameObject WaveButton;
	Text WaveButtonText;
	int lastWaveNum=0;

	Text SpeedButtonText;

	 Camera camera;
	 Color color1;
	 Color color2;

	EnemySpawner ES;

	bool afterReload;
	bool gameOver=false;

	int count=0;


	void Start () {
		SpeedButtonText = GameObject.Find ("FastForwardButton").transform.GetChild(0).GetComponent<Text>();
		SpeedButtonText.text="Speed x"+lastScale.ToString();

		Events = GameObject.Find ("Events");

		GameOverPanel = Events.transform.GetChild (0).gameObject;
		WinLevelPanel =   Events.transform.GetChild (1).gameObject;
		StartScreen =  Events.transform.GetChild (2).gameObject;
		LevelSelect =  Events.transform.GetChild (3).gameObject;

		Fade = GameObject.Find ("MASTER-UI").transform.GetChild(2).gameObject;

		ES = GameObject.Find ("Path").GetComponent<EnemySpawner>();


		WaveButton = GameObject.Find ("PlayPauseButton");
		WaveButtonText = WaveButton.transform.GetChild(0).GetComponent<Text> ();
		WaveButtonText.text = "Begin Wave " + ES.WaveNumber;
		lastWaveNum = ES.WaveNumber;

		if (Data.reloading) {
			if (Data.dGameOver && !Data.a) {
				StartGame ();
				print ("started");

			} else if(Data.a && Data.dGameOver){
				print ("a glourius eles");
				Data.a = false;	//reseting the static var so it doesn't break the TryAgain button
				Data.dGameOver=false;
				StartScreen.SetActive (true);
			}
			Data.reloading = false;
		}

		//Time.timeScale = 0; //->must be commented so that the fadeing works after reloading
	}
	
	//-----------------------------------------------------------------------
	void Update () {
		//print(lastWaveNum+"--"+ES.WaveNumber);
		//print(lastScale+"---"+Time.timeScale);
		//print (pause);

		print (Time.timeScale);

		if (lastWaveNum < ES.WaveNumber) {
			pause=true; //important so the PlayPause button doens't get buggy
			Time.timeScale = 0;


			lastWaveNum = ES.WaveNumber;
			WaveButtonText.text = "Begin Wave " + ES.WaveNumber;
		} 

		//used so #25(Bug) doesn't happen->bullet not despawning when timeScale=0
		if (Time.timeScale == 0) {
			if (GameObject.Find ("Bullet(Clone)")) {
				print ("Found stoped bullet");
				Destroy (GameObject.Find("Bullet(Clone)"));
			}
		}

		//----DEV-keys
		if (Input.GetKeyDown(KeyCode.Escape)) {
			Application.Quit();
		}

		if (Input.GetKeyDown(KeyCode.Space)) {
			Time.timeScale *= 2;
		}

		if (Input.GetKeyDown(KeyCode.M)) {
			Time.timeScale /= 2;
		}
		//print (Time.timeScale+" "+lastScale);
		//FadeBackground ();
		//print("a:"+Data.a+"--gameover "+Data.dGameOver);

	}
	//-----------------------------------------------------------------------


	//gets called when the FF-Button is clicked
	public void FastForward(){
		int maxSpeed = 16;
		if (Time.timeScale != 0) {

			if (Time.timeScale < maxSpeed) {
				Time.timeScale *= 2;
			} else if (Time.timeScale == maxSpeed) {
				Time.timeScale = 1;
			}
			lastScale = Time.timeScale;
		}
		SpeedButtonText.text="Speed x"+lastScale.ToString(); 
		//changing the UI-text of the speed button->just when clicking it
	}

	//gets called when PP-Button is clicked
	public void PlayPause(){
		pause = !pause;
		if (pause) {
			Time.timeScale = 0;
		} else {			
			Time.timeScale = lastScale;
		}

		WaveButtonText.text = "►  Play/Pause";

		//Start of game
		if (count == 0) {
			ES.spawnWave = true;
			count++;
			Time.timeScale = lastScale;
			// ►  Play/Pause
			WaveButtonText.text = "►  Play/Pause";
		}
	}

	public void GameOver(){
		GameOverPanel.SetActive (true);
		gameOver = true;
		Data.dGameOver = true;
		Time.timeScale = 0;
	}

	public void LevelCompleted(){
		WinLevelPanel.SetActive (true);
		Time.timeScale = 0;
	}

	public void QuitGame(){
		//Application.Quit();
		//used to prevent killing the unity process while being in editor mode
		if (!Application.isEditor){
			System.Diagnostics.Process.GetCurrentProcess().Kill();	
		}

	}

	public void StartGame(){
		StartScreen.SetActive (false);
		LevelSelect.SetActive (false);
		StartCoroutine (FadeScreen());
	}

	public void QuitToMenu(){
		Data.a = true;
		StartToLevelSelect ();
	}
	public void toStartScreen(){
		Application.LoadLevel ("Level1");
	}

	public void StartToLevelSelect(){		
		StartScreen.SetActive (false);
		StartCoroutine (FadeScreen());
		LevelSelect.SetActive (true);

		if (Data.dGameOver) {
			TryAgain ();
			//GameOverPanel.SetActive (false);
			//Fade.SetActive (false);
			//LevelSelect.SetActive (true);
		}
	}

	public void chooseLevelToLoad(int level){
		currentLevel = level;

		if (level != 1) {
			Application.LoadLevel ("Level" + level.ToString ());
			StartGame ();
		} else {
			StartGame ();
		}

	}

	IEnumerator FadeScreen(){
		Fade.SetActive (true);
		Image img = Fade.GetComponent<Image> (); 
		float timer = 1f;

		Time.timeScale = 1;

		while (timer > 0f) {
			timer -= Time.deltaTime;
			img.color = new Color (0f,0f,0f,timer);
			yield return 0;
		}
		//Time.timeScale = 0;
		Fade.SetActive (false);

	}

	//can be deleted
	void FadeBackground(){
		float t = Mathf.InverseLerp(0,1f,Time.time);
		//camera.backgroundColor = Color.Lerp (color1,color2,t);
	}

	public void TryAgain(){		
		Data.reloading = true;
		//Data.dGameOver=false;
		Application.LoadLevel (Application.loadedLevel);

	}

}
