using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/* 	@ every Turrent 
	Des: used for displaying a dialog for updating&selling the turrent
	NOTE: Gets Instantiated from TurrentScript @Line 207
*/

public class TurrentDialog : MonoBehaviour {

	public bool upgrade=false;
	public bool sell=false;

	float time=0;
	public Text upgradeButtonText=null;
	public Text sellButtonText=null;

	public int sellPrice=0;

	public void Start(){
		sellButtonText = transform.GetChild (1).transform.GetChild (0).GetComponent<Text> ();

		if (!upgrade) {
			sellButtonText.text = "Sell " + sellPrice;
		}
		//transform.rotation = Quaternion.AngleAxis (130,Vector3.up);
		//	print (transform.position);

		//z axis must be changed because of a wierd bug with the positions
		Vector3 vec = transform.position;
		vec.z -= 1f;
		transform.position = vec;
	}

	public void Upgrade(){	
		
		upgrade = true;

		if (upgradeButtonText != null) {
			upgradeButtonText.text = "No Upgrades";
			//TODO: change the text to "no money" if thers no money...
		}
		//Destroy (transform.gameObject);
	}

	public void Sell(){
		sell = true;
		Destroy (transform.gameObject);
	}

	public void Update(){
		//print (transform.position);
		time += Time.deltaTime;

		if (time > 3) {
			Destroy (transform.gameObject);
		}

		if (upgrade) {
			//upgradeButtonText.text = "No Upgrades";
		} else {
			sellButtonText.text = "Sell " + sellPrice;
		}

	}

	public void setUpgradePrice(int price,int lvl){
		upgradeButtonText = transform.GetChild (0).transform.GetChild (0).GetComponent<Text> ();
		if (lvl == 1) {
			upgradeButtonText.text = "Upgrade "+price.ToString ();
		}

		if (lvl == 2) {			
			upgradeButtonText.text = "No Upgrades";
		}

	}

}
