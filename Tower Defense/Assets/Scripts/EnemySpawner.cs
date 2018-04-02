using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;


/* 	@ Map>Path 
	Des:	Is responsible for spawning the public enemy Prefab at the start position
		-> The WaveSpawner
*/

public class EnemySpawner : MonoBehaviour {
	
	public GameObject enemyPrefab1; //geting enempy prefab

	public List<GameObject> EnemyTypeList = new List<GameObject> ();

	public static  Transform[] pathNodes; //array for every path node

	Transform startPos; //transform of the start pos

	GameControll GC;

	//----------------------------
	//used in the waveSpawner
	float timer=0;
	int count=0;

	public int WaveNumber=0; //int for changing wave number in UI
	public bool waveSpawned; //maybe usefull for later
	public bool spawnWave;

	int indexWaveNumber=0;

	public Wave[] waves;	//main array for storing all the waves of an level
	//----------------------------

	public GameObject EnemyHolderPrefab;	//used to hold the spawed enemys
	
	//Used for UI //TODO: can be removed if not need
	GameObject enemiesLeftText; 
	Text elText;

	GameObject enemy123;

	void Start () {

		startPos = transform.GetChild (0); //getting the start position for your enemy

		pathNodes = new Transform[transform.childCount]; //defining array length

		for (int i = 0; i < transform.childCount; i++)	//getting all the Transforms of the pathNodes
		{
			pathNodes [i] = transform.GetChild (i);
		}

		enemiesLeftText = GameObject.Find ("EnemyNo"); //getting the UI component
		//elText = enemiesLeftText.GetComponent<Text> ();	//UI elemnts not used any more

		GC = GameObject.Find ("GameMASTER").GetComponent<GameControll>(); //getting the GameController


	}

	void Update () {		

		//print (EnemyHolderPrefab.transform.childCount);

		timer += Time.deltaTime;

		if (timer > 5 && count==0 && spawnWave) {
			StartCoroutine (SpawnWave (indexWaveNumber));
			timer = 0;
			count++;
			indexWaveNumber++;
		}

		if (waveSpawned && indexWaveNumber<waves.Length) {
			if (EnemyHolderPrefab.transform.childCount == 0) {
				//print ("Finnaly once zero");

				StartCoroutine (SpawnWave (indexWaveNumber));
				indexWaveNumber++;
				WaveNumber++;
				waveSpawned = false;

			}
		}

		if (waveSpawned && EnemyHolderPrefab.transform.childCount==0 && indexWaveNumber==waves.Length) {
			GC.LevelCompleted();
		}

		//print ("WaveNumber " + WaveNumber);

	}

	void spawnEnemy(GameObject enemyPrefab){		
		enemy123 = (GameObject)Instantiate(enemyPrefab, new Vector3(startPos.position.x, startPos.position.y, startPos.position.z), Quaternion.identity);
		enemy123.transform.rotation = Quaternion.AngleAxis (136,Vector3.up);

		enemy123.transform.SetParent (EnemyHolderPrefab.transform);
		//enemiesSpawned++;
	}
	/*
	IEnumerator spawnEnemy(GameObject enemyPrefab,float time){
		print (time);

		GameObject enemy123 = (GameObject)Instantiate(enemyPrefab, new Vector3(startPos.position.x, startPos.position.y, startPos.position.z), Quaternion.identity);
		enemy123.transform.SetParent (EnemyHolderPrefab.transform);
		yield return new WaitForSeconds (time);
		//enemiesSpawned++;
	}*/

	IEnumerator SpawnWave(int h){		
		//for (int h = 0; h < waves.Length; h++) {		

			//print ("Wave " + h);

			//WaveNumber = h + 1; //used in s_WaveController to change UI
					
		yield return new WaitForSeconds (2f); //used so it doesn't spawn it directly
			for (int i = 0; i < waves [h].Nodes.Length; i++) {

				//print ("Node" + i);
				for (int j = 0; j < waves[h].Nodes[i].enemyNum; j++) {
					//print (waves[h].Nodes[i].enemyNum);
					//print (waves[h].Nodes[i].enemyPrefab.name);

					if (timer > waves [h].Nodes[i].spawnRate) {	

						//spawnEnemy (waves[h].Nodes[i].enemyPrefab);
						//print (waves [h].Nodes [i].enemyPrefab.name);
						//timer=0;
						//StartCoroutine (spawnEnemy (waves[h].Nodes[i].enemyPrefab , waves [h].Nodes[i].spawnRate));
					}


					//TODO: some we have to wait for a wave to finish so the next one can begin, or just make a fancy countdown
					//when a wave ends ...

					spawnEnemy (waves[h].Nodes[i].enemyPrefab);
					yield return new WaitForSeconds (waves [h].Nodes[i].spawnRate);

					//StartCoroutine (spawnEnemy (waves[h].Nodes[i].enemyPrefab , waves [h].Nodes[i].spawnRate));

				}
				//print ("Node");
				yield return new WaitForSeconds (waves [h].WaveSpawnRate);
			}
			//TODO: when this happesn there could be a count down or similar,
			//also the time can be a bit shorter ,maybe 5secs...

			//print ("Wave End ->5sec till new waves");
			if (EnemyHolderPrefab.transform.childCount != 0) {
				//yield return new WaitForSeconds (20f);	
			} 
			//yield return new WaitForSeconds (10f);	
			//Time.timeScale = 0;

			//TODO: a if statement which checks if there are enemies still left and if
			//there are the yiel should wait
		//}

		print ("after");
		//waves ends here
		waveSpawned=true;
	}



}
