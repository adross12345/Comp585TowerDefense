using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlaceableBuilding : MonoBehaviour {

	[HideInInspector]
	public List<Collider> colliders = new List<Collider>();
	private bool isSelected;
	private bool isShowingRange;
	public string bName;

	void OnGUI() {
		if (isSelected) {
//			GUI.Button(new Rect(Screen.width /2, Screen.height / 20, 100, 30), bName);	
			if (!isShowingRange) {
//				Transform t = transform.parent;
//				Debug.Log (t.name);
//				Transform rng = t.Find ("Range");
//				rng.gameObject.GetComponent<MeshRenderer> ().enabled = true;
				transform.parent.Find("Turret").Find ("Range").gameObject.GetComponent<MeshRenderer> ().enabled = true;
				isShowingRange = true;
			}
		} else if(isShowingRange) {
			transform.parent.Find("Turret").Find ("Range").gameObject.GetComponent<MeshRenderer> ().enabled = false;
			isShowingRange = false;
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

	
}
