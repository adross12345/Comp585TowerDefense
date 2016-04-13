using UnityEngine;
using System.Collections;

public class BuildingManager : MonoBehaviour {

	public GameObject[] buildings;
	private BuildingPlacement buildingPlacement;
	private bool initOnce = false;

	// Use this for initialization
	void Start () {
		buildingPlacement = GetComponent<BuildingPlacement>();
	}

	// Update is called once per frame
	void Update () {
		if (buildingPlacement.hasPlaced == true && !initOnce) {
			ShowTowerPopup.Init (buildings [0]);
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
        CastleHealth castle = GameObject.Find("Castle").GetComponent<CastleHealth>();
        if (castle.canPurchase(200)) {
            Debug.Log("spawnTower1() triggered");
            buildingPlacement.SetItem(buildings[0]);
            castle.makePurchase(200);
            initOnce = false;
        } else {
            // TODO Produce an alert that says you cannot make that purchase
        }
	}

	public void spawnTower2() {
        CastleHealth castle = GameObject.Find("Castle").GetComponent<CastleHealth>();
        if (castle.canPurchase(300)) {
            Debug.Log("spawnTower2() triggered");
            buildingPlacement.SetItem(buildings[1]);
            castle.makePurchase(300);
            initOnce = false;
        } else {
            // TODO Produce an alert that says you cannot make that purchase
        }
    }

    public void spawnTower3() {
        CastleHealth castle = GameObject.Find("Castle").GetComponent<CastleHealth>();
        if (castle.canPurchase(300))
        {
            Debug.Log("spawnTower3() triggered");
            buildingPlacement.SetItem(buildings[2]);
            castle.makePurchase(500);
            initOnce = false;
        }
        else {
            // TODO Produce an alert that says you cannot make that purchase
        }
    }

    public void spawnTower4() {
        CastleHealth castle = GameObject.Find("Castle").GetComponent<CastleHealth>();
        if (castle.canPurchase(750))
        {
            Debug.Log("spawnTower4() triggered");
            buildingPlacement.SetItem(buildings[3]);
            castle.makePurchase(750);
            initOnce = false;
        }
        else {
            // TODO Produce an alert that says you cannot make that purchase
        }
    }

    public void spawnTower5() {
        CastleHealth castle = GameObject.Find("Castle").GetComponent<CastleHealth>();
        if (castle.canPurchase(1000))
        {
            Debug.Log("spawnTower5() triggered");
            buildingPlacement.SetItem(buildings[4]);
            castle.makePurchase(1000);
            initOnce = false;
        }
        else {
            // TODO Produce an alert that says you cannot make that purchase
        }
    }
}