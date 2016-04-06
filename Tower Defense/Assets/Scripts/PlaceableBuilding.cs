using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlaceableBuilding : MonoBehaviour {
    public Sprite allySprite;
    public Sprite enemySprite;

	[HideInInspector]
	public List<Collider> colliders = new List<Collider>();
	private bool isSelected;
	private bool isPlaced;
	private bool isShowingRange;
	public string bName;

	void OnGUI() {
		if (isSelected) {
			Debug.Log ("Selected");
//			GUI.Button(new Rect(Screen.width /2, Screen.height / 20, 100, 30), bName);	
			if (!isShowingRange) {
//				Transform t = transform.parent;
//				Debug.Log (t.name);
//				Transform rng = t.Find ("Range");
//				rng.gameObject.GetComponent<MeshRenderer> ().enabled = true;
				transform.parent.Find("Turret").Find ("Range").gameObject.GetComponent<MeshRenderer> ().enabled = true;
				isShowingRange = true;

                // Add nodes to UI
                //allySprite = Sprite.Create(transform.parent.Find("Turret").GetComponent<CannonFire>().node.getAllyTexture(), new Rect(0, 0, 32, 32), new Vector2(0.5f, 0.5f));
                //enemySprite = Sprite.Create(transform.parent.Find("Turret").GetComponent<CannonFire>().node.getEnemyTexture(), new Rect(0, 0, 32, 32), new Vector2(0.5f, 0.5f));
            }
        } else if(isShowingRange) {
			transform.parent.Find("Turret").Find ("Range").gameObject.GetComponent<MeshRenderer> ().enabled = false;
			isShowingRange = false;

            // Remove nodes from UI
		}

	}
	
	void OnTriggerEnter(Collider c) {
		if (c.tag == "BuildingPlacement") {
			colliders.Add(c);	
		}
	}
	
	void OnTriggerExit(Collider c) {
		if (c.tag == "BuildingPlacement") {
			colliders.Remove(c);	
		}
	}
	
	public void SetSelected(bool s) {
		isSelected = s;	
	}

	public void SetPlaced(bool p) {
		isPlaced = p;
	}

	
}
