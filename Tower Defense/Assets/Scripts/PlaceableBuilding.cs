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
            }
			// Add nodes to UI
			NeuralNode n = transform.parent.Find("Turret").GetComponent<CannonFire>().GetNode();
			Texture2D a = n.getAllyTexture();
			Texture2D e = n.getEnemyTexture();
			Texture2D c = n.GetTargetTex();
			Sprite sa = Sprite.Create(a, new Rect(0, 0, a.width, a.height), new Vector2(0.5f, 0.5f));
			Sprite se = Sprite.Create(e, new Rect(0, 0, e.width, e.height), new Vector2(0.5f, 0.5f));
			Sprite sc = Sprite.Create(c, new Rect(0, 0, c.width, c.height), new Vector2(0.5f, 0.5f));
			GameObject.Find("UIManager").GetComponent<NodeManager>().setAllySprite(sa);
			GameObject.Find("UIManager").GetComponent<NodeManager>().setEnemySprite(se);
			GameObject.Find("UIManager").GetComponent<NodeManager>().setTargetSprite(sc);
			GameObject.Find("UIManager").GetComponent<NodeManager>().setB("" + n.b);
			GameObject.Find("UIManager").GetComponent<NodeManager>().setZ("");
        } else if(isShowingRange) {
			transform.parent.Find("Turret").Find ("Range").gameObject.GetComponent<MeshRenderer> ().enabled = false;
			isShowingRange = false;

            // Remove nodes from UI
            GameObject.Find("UIManager").GetComponent<NodeManager>().resetAll();
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
