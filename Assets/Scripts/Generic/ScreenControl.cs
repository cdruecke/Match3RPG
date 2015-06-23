using UnityEngine;
using System.Collections;

public class ScreenControl : MonoBehaviour {
	/*  start: title screen
	 *  select: level select
	 *  make: sandwich making
	 *  deliver: sandwich delivery
	 *  loss: game over screen
	 *  win: game win screen
	 */
	public string state = "start";
	int level = 0;
	public GameObject startUI;
	public GameObject selectUI;
	public GameObject makeUI;
	public GameObject deliverUI;
	public GameObject lossUI;
	public GameObject winUI; 
	public bool updated = false;
	GameObject sandwich;
	GameObject delivery;
	// Use this for initialization
	void Start () {
		sandwich = GameObject.Find("Board");
		//DisableUIs();
	}

	void DisableUIs() {
		startUI.SetActive(false);
		selectUI.SetActive(false);
		makeUI.SetActive(false);
		deliverUI.SetActive(false);
		lossUI.SetActive(false);
		winUI.SetActive(false);
	}

	void DisableGameObjects() {
		sandwich.SetActive(false);
		//delivery.SetActive(false);
	}

	public void ToLevelSelect() {
		updated = false;
		state = "select";
	}

	public void ToMakeSandwich(int i) {
		updated = false;
		state = "make";
		level = i;
	}

	public void ToDeliverSandwich() {
		updated = false;
		state = "deliver";
	}

	public void ToWinGame() {
		updated = false;
		state = "win";
	}

	public void ToLoseGame() {
		updated = false;
		state = "loss";
	}
	
	// Update is called once per frame
	void Update () {
		if(!updated) {
			//DisableUIs();
			DisableGameObjects();
			if(state=="start") {
				// Do things.
				updated = true;
				//startUI.SetActive(true);
			} else if (state=="select") {
				updated = true;
				selectUI.SetActive(true);
			} else if (state=="make") {
				updated = true;
				//makeUI.SetActive(true);
				sandwich.SetActive(true);
				sandwich.GetComponent<Board>().LoadLevel();
			} else if(state=="deliver") {
				updated = true;
				deliverUI.SetActive(true);
				delivery.SetActive(true);
				//delivery.GetComponent<DeliverSandwich>().LoadSandwich();
			} else if(state=="loss") {
				updated = true;
				lossUI.SetActive(true);
			} else if(state=="win") {
				updated = true;
				winUI.SetActive(true);
			} else {
				state = "start";
				Debug.Log("Illegal game state!!");
			}
		}
	}
}
