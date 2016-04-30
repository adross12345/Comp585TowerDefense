using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BuildingManager : MonoBehaviour {

	public GameObject[] buildings;
	private BuildingPlacement buildingPlacement;
	private bool initOnce = false;
	private GameObject currentTower;
	private CastleHealth castle;
	private int lastPurchaseAmount = 0;


	// Use this for initialization
	void Start () {
		buildingPlacement = GetComponent<BuildingPlacement>();
		buildingPlacement.SetManager (this);
		castle = GameObject.Find("Castle").GetComponent<CastleHealth>();
	}

	// Update is called once per frame
	void Update () {
		if (buildingPlacement.hasPlaced == true && !initOnce) {
			GameObject.Find("TrainingWindow").GetComponent<Canvas>().enabled = true;
			GameObject.Find("Main Camera").GetComponent<PopUpUI>().Init(currentTower);
			initOnce = true;
		}
	}

	void OnGUI() {
		/*for (int i = 0; i <buildings.Length; i ++) {
			if (GUI.Button(new Rect(Screen.width/20,Screen.height/15 + Screen.height/12 * i,100,30), buildings[i].name)) {
				buildingPlacement.SetItem(buildings[i]);
			}
		}*/
	}

	public void spawnTower1() {
		if (castle.canPurchase(200)) {
			Debug.Log("spawnTower1() triggered");
			currentTower = buildingPlacement.SetItem(buildings[0]);
			castle.makePurchase(200);
			lastPurchaseAmount = 200;
			initOnce = false;
		} else {
			// TODO Produce an alert that says you cannot make that purchase
		}
	}

	public void spawnTower2() {
		if (castle.canPurchase(300)) {
			Debug.Log("spawnTower2() triggered");
			currentTower = buildingPlacement.SetItem(buildings[1]);
			castle.makePurchase(300);
			lastPurchaseAmount = 300;
			initOnce = false;
		} else {
			// TODO Produce an alert that says you cannot make that purchase
		}
	}

	public void spawnTower3() {
		if (castle.canPurchase(500)) {
			Debug.Log("spawnTower3() triggered");
			currentTower = buildingPlacement.SetItem(buildings[2]);
			castle.makePurchase(500);
			lastPurchaseAmount = 500;
			initOnce = false;
		}
		else {
			// TODO Produce an alert that says you cannot make that purchase
		}
	}

	public void spawnTower4() {
		if (castle.canPurchase(500))
		{
			Debug.Log("spawnTower4() triggered");
			currentTower = buildingPlacement.SetItem(buildings[3]);
			castle.makePurchase(500);
			lastPurchaseAmount = 500;
			initOnce = false;
		}
		else {
			// TODO Produce an alert that says you cannot make that purchase
		}
	}

	public void spawnTower5() {
		if (castle.canPurchase(750))
		{
			Debug.Log("spawnTower5() triggered");
			currentTower = buildingPlacement.SetItem(buildings[4]);
			castle.makePurchase(750);
			lastPurchaseAmount = 750;
			initOnce = false;
		}
		else {
			// TODO Produce an alert that says you cannot make that purchase
		}
	}

	public void Refund(){
		castle.makePurchase (-1 * lastPurchaseAmount);
	}

	public void MakePurchase(int cost){
		castle.makePurchase (cost);
	}

	public void AddHealth(int health){
		castle.curHealth += health;
	}
}