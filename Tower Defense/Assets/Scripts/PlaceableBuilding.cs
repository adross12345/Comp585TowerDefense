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
	private NodeManager nodeMan;

	void Start(){
		nodeMan = GameObject.Find ("UIManager").GetComponent<NodeManager> ();
	}

	void OnGUI() {
		if (isSelected) {
			//Debug.Log ("Selected");
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
			CannonFireAOE cnfire = transform.parent.Find("Turret").GetComponent<CannonFireAOE>();
			NeuralNode n = cnfire.GetNode();
			Texture2D a = n.getAllyTexture();
			Texture2D e = n.getEnemyTexture();
			Texture2D c = n.GetTargetTex();
			Texture2D f = n.getFeatureTexture ();
			Sprite sa = Sprite.Create(a, new Rect(0, 0, a.width, a.height), new Vector2(0.5f, 0.5f));
			Sprite se = Sprite.Create(e, new Rect(0, 0, e.width, e.height), new Vector2(0.5f, 0.5f));
			Sprite sc = Sprite.Create(c, new Rect(0, 0, c.width, c.height), new Vector2(0.5f, 0.5f));
			Sprite sf = Sprite.Create(f, new Rect(0, 0, f.width, f.height), new Vector2(0.5f, 0.5f));
			nodeMan.setAllySprite(sa);
			nodeMan.setEnemySprite(se);
			nodeMan.setTargetSprite(sc);
			nodeMan.setFeatureSprite (sf);
			nodeMan.setB(n.b);
			nodeMan.setZ(n.lastZ);
			nodeMan.numEneInRange = cnfire.numEnemiesInRange;
			nodeMan.numAllyInRange = cnfire.numAlliesInRange;

            // Place tower picture and name

            GameObject.Find("UIManager").GetComponent<InfoManager>().setTowerName(bName);
            Sprite icon = null;
            string sellPrice = "";
            if (bName == "Basic Tower") {
                //Debug.Log("Basic Tower Selected");
                icon = Resources.Load<Sprite>("Sprites/BasicTower");
                sellPrice = "$140";
            } else if (bName == "Radial Tower") {
                //Debug.Log("Radial Tower Selected");
                icon = Resources.Load<Sprite>("Sprites/RadialTower");
                sellPrice = "$210";
            } else if (bName == "Sniper Tower") {
                //Debug.Log("Sniper Tower Selected");
                icon = Resources.Load<Sprite>("Sprites/SniperTower");
                sellPrice = "$350";
            } else if (bName == "Machine Tower") {
                //Debug.Log("Machine Tower Selected");
                icon = Resources.Load<Sprite>("Sprites/MachineTower");
                sellPrice = "$350";
            } else if (bName == "Splash Tower") {
                //Debug.Log("Splash Tower Selected");
                icon = Resources.Load<Sprite>("Sprites/SplashTower");
                sellPrice = "$525";
            } else {
                icon = Resources.Load<Sprite>("Sprites/black");
                sellPrice = "";
            }
            //Debug.Log(sellPrice);
            GameObject.Find("UIManager").GetComponent<InfoManager>().setTowerImage(icon);
            GameObject.Find("UIManager").GetComponent<InfoManager>().setTowerPrice(sellPrice);
        } else if(isShowingRange) {
			transform.parent.Find("Turret").Find ("Range").gameObject.GetComponent<MeshRenderer> ().enabled = false;
			isShowingRange = false;

            // Remove nodes from UI
            // I'm not so sure we want to do that. We may as well let the player casually look at the masks and z.
            // nodeMan.resetAll();

            GameObject.Find("UIManager").GetComponent<InfoManager>().resetAll();
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
