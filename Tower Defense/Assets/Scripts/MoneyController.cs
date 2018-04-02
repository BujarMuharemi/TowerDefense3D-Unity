using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/* 	@ HUD-UI>InfoScreen>Money
	Des: displaying money to the ui elements
		-controlling money for buying/selling turrents
*/

public class MoneyController : MonoBehaviour {

	/*TODO: get the prices for the turrent from their prefabs or get the via Editor
	 * 	monney checker , to check money player has and buy price
	*/

	public int Money=100;

	Text moneyText;	//used for changin the UI text 

	public bool turrentWasPlaced;
	public bool noMoney;
	bool bulletKilledEnemy;

	bool allowPlacement;

	TurrentControl TC;


	void Start () {
		GameObject a = transform.GetChild (0).gameObject;	//getting the child GameObject
		moneyText = a.GetComponent<Text> ();	//getting the Text component
		moneyText.text= Money.ToString()+" $";	//setting it to Money 
		TC = GameObject.Find("TurrentController").GetComponent<TurrentControl>();			
	}

	void Update () {

		TurrentScript TSC = TC.choosenPrefab.GetComponent<TurrentScript> ();
		
		//int price = TC.choosenPrefab.GetComponent<TurrentScript> ().price;	//getting the price of the choosen turrent
		int price = TSC.price;
		if (TSC.Level == 2) {
			price = (int)TSC.upgradePrice;
		}

		if (Money <= 0) {	//checking money for the noMoney Flag
			noMoney = true;

		} else if(Money>0){	//checking if you have enough money for the choosen turrent

			if (Money >= price) {
				noMoney = false;
			} else {
				noMoney = true;
			}

		}


		if (turrentWasPlaced) {	//controll for placing turrents
			Money -= price;
			turrentWasPlaced = false;
		}

		//TODO: can be deleted
		if (bulletKilledEnemy) {// -||- for getting money for enemy kills
			Money += 25;	//FIXME: price has somhow to increase with higher Wave numbers
			bulletKilledEnemy = false;
		}



		moneyText.text = Money.ToString()+" $";	//updating
	}

	public void addMoney(int amount){
		if (amount > 0) {
			Money += amount;
		} else if (amount < 0) { //used when we want to add a negative value->for upgrades
			if (amount * -1 <= Money) {
				Money += amount;
			} else {				
				noMoney = true;
			}
		} 	

	}

}
