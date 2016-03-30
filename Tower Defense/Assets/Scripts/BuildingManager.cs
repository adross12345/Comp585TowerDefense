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

    public void spawnTower() {
        Debug.Log("spawnTower() triggered");
        buildingPlacement.SetItem(buildings[0]);
    }
}
