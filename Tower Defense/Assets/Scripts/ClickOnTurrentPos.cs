using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

/* 	@ Map>TurrentPositions>"Every element on which we can place an turrent"
	Des: checks if we can spawn an Turrent on the given object and if we have the money for it
*/

public class ClickOnTurrentPos : MonoBehaviour {	

	bool turrentSet=false;	//used as flag so we can only spawn one turrent at a tile
	GameObject temp;
	TurrentControl TC;
	MoneyController Money;	//getting the MoneyScript
	Color standard;

	TurrentDialog TD;

	bool overUI=false;

	void Start(){
		//Getting the TurrentController GameObject , to access the TurrentControl script 
		temp = GameObject.Find ("TurrentController");
		TC = temp.GetComponent<TurrentControl>();

		Money = GameObject.Find ("Money").GetComponent<MoneyController>();	//getting the MoneyController Scipt
		standard = gameObject.GetComponent<Renderer> ().material.color;

	}

	void Update(){		
		GameObject TDC = GameObject.Find ("TurrentDialog(Clone)");
		if (TDC != null) {
			TD = TDC.GetComponent<TurrentDialog>();
			if (TD.sell) {
				turrentSet = false;
			}
		}

		//checking if the mous is hovering over the TurrentDialog-UI element
		if (EventSystem.current.IsPointerOverGameObject ()) {
			overUI = true;
		} else {
			overUI = false;
		}
	

	}


	void OnMouseDown(){
		//sends the gameObject that was clicked to the TurrentControl-Script

		if (!turrentSet && !Money.noMoney && !overUI) {	//making sure we spawn just one object & checking if we have mony to spawn one
			TC.getClickedTurrent (gameObject);
			turrentSet = true;
			Money.turrentWasPlaced = true;	//setting flag for the script
			//print("You spawned an "+TC.choosenPrefab);
			//GameObject PS = Instantiate(Resources.Load("PS"),typeof(GameObject)) as GameObject;

		}

	}

	void OnMouseOver(){
		if (!overUI) {
			gameObject.GetComponent<Renderer> ().material.color = Color.gray;
		}

	}

	void OnMouseExit(){
		gameObject.GetComponent<Renderer> ().material.color = standard;
	}

}
