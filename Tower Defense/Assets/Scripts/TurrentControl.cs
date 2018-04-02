using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/* 	@ Map>TurrentControl
	Des: Is responsible for "spawning" the public turrent Prefabs at the their positions.
		-it just spawns the enemys and has nothing to do with their shooting etc
		-it's connected to the UI/Money etc. Script / Manager
*/

public class TurrentControl : MonoBehaviour {	

	//from here we can send the price to the MonneyController

	public List<GameObject> TurrentList = new List<GameObject> ();

	public GameObject choosenPrefab;	//which type of turrent was choosen

	Scene scene;

	void Start () {
		choosenPrefab = TurrentList [0];
		scene = SceneManager.GetActiveScene();
	}	

	void Update () {
	
	}

	//method which gets the gameObject of the clicked Turrent
	public void getClickedTurrent(GameObject turrent){
		
		/*Vector3 temp = turrent.transform.position;
		temp.y += 0.5f;
		turrent.transform.position = temp;
		*/

		//instatiating the turrent , the offset is needed
		//because of a odd reason in the editor the y axis isn't the same in every level
		float pos=1.7f;
		if (scene.name == "Level2" || scene.name=="Level3") {
			pos = 0.9f;
		}

		Instantiate(choosenPrefab, new Vector3(turrent.transform.position.x, pos ,turrent.transform.position.z), Quaternion.identity);
	}

	//choosing the TurrentType
	public void setTurrentType(int Type){
		switch (Type){
		case 0:
			choosenPrefab = TurrentList [0];
			break;
		case 1:
			choosenPrefab = TurrentList[1];
			break;
		case 2:
			choosenPrefab = TurrentList[2];
			break;
		case 3:
			choosenPrefab = TurrentList[3];
			break;
		}

	}

	//#13@HnP ->TODO: implement it in a seperate script which is on the button GameObjects itseld
	// the code can be copied from here 
	void changButton(int num){
		string buttonName = "Button" + (num+1).ToString ();
		print (buttonName);
		Button button = GameObject.Find (buttonName).GetComponent<Button>();
		ColorBlock cb = button.colors;
		Color32 nor = cb.normalColor;
		nor = new Color32 (255,0,255,255);
		cb.normalColor = nor;

		button.onClick.AddListener (DoOnClick);

		button.colors = cb;
	}

	void DoOnClick(){
		print ("you clicked me");
	}
}
