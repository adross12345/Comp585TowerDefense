using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BuildingPlacement : MonoBehaviour {
	public float scrollSensitivity;

	private PlaceableBuilding placeableBuilding;
	private Transform currentBuilding;
	public bool hasPlaced;

	public LayerMask buildingsMask;
	public LayerMask groundMask;
	public LayerMask unitMask;

	public PlaceableBuilding placeableBuildingOld;

	private BuildingManager manager;

	// Update is called once per frame
	void Update () {
		Camera mainCam = GetComponent<Camera> ();
		Vector3 m = Input.mousePosition;
		//		m = new Vector3(m.x,m.y,transform.position.y);
		//		Vector3 p = mainCam.ScreenToWorldPoint(m);

		Ray ray = mainCam.ScreenPointToRay (m);
		Plane xz = new Plane(Vector3.up, new Vector3(0,0.5f,0));
		float distance;
		xz.Raycast (ray, out distance);
		Vector3 p = ray.GetPoint (distance);


		//		Vector3 p = new Vector3 (0, 0, 0);
		//		Ray ray = mainCam.ScreenPointToRay (m);
		//		if (Physics.Raycast (ray, out hit, Mathf.Infinity, groundMask)) {
		//			m = new Vector3(m.x,m.y,hit.distance);
		//			p = mainCam.ScreenToWorldPoint(m);
		//		}


		if (currentBuilding != null && !hasPlaced) {
			currentBuilding.position = new Vector3(p.x,p.y,p.z);

			if (Input.GetMouseButtonDown (0)) {
				if (IsLegalPosition (p)) {
					Debug.Log ("Placing Building");
					hasPlaced = true;	
					//					placeableBuilding.SetSelected (false);
					placeableBuilding.SetPlaced (true);
					placeableBuildingOld = placeableBuilding;
					currentBuilding = null;
				}
			} else if (Input.GetMouseButtonDown (1)) {
				Destroy (currentBuilding.gameObject);
				if (manager != null) {
					manager.Refund ();
				}
				currentBuilding = null;
			}
		}
		else {
			if (Input.GetMouseButtonDown(0)) {
				// hasPlaced = true;
				RaycastHit hit = new RaycastHit();
				ray = new Ray(new Vector3(p.x,10,p.z), Vector3.down);
				if (Physics.Raycast(ray, out hit,Mathf.Infinity,buildingsMask)) {
					if (placeableBuildingOld != null) {
						placeableBuildingOld.transform.parent.FindChild ("Turret").GetComponent<CannonFireAOE> ().SetPlayerTarget (null);
						placeableBuildingOld.SetSelected(false);
					}
					hit.collider.gameObject.GetComponent<PlaceableBuilding>().SetSelected(true);
					placeableBuildingOld = hit.collider.gameObject.GetComponent<PlaceableBuilding>();
				}
				else {
					if (placeableBuildingOld != null) {
						RaycastHit unitHit = new RaycastHit();
						ray = new Ray(new Vector3(p.x,10,p.z), Vector3.down);
						if (Physics.Raycast (ray, out unitHit, Mathf.Infinity, unitMask)) {
							if(unitHit.collider is BoxCollider){
								Unit u = unitHit.collider.gameObject.GetComponent<Unit> ();
								placeableBuildingOld.transform.parent.FindChild ("Turret").GetComponent<CannonFireAOE> ().SetPlayerTarget (u);
							}
						}else {
							placeableBuildingOld.transform.parent.FindChild ("Turret").GetComponent<CannonFireAOE> ().SetPlayerTarget (null);
							placeableBuildingOld.SetSelected(false);
//							placeableBuildingOld = null;
						}
					}
				}
			}
		}
	}//Update()


	bool IsLegalPosition(Vector3 position) {
		bool res = true;
		RaycastHit hit = new RaycastHit();
		Ray ray = new Ray(new Vector3(position.x,10,position.z), Vector3.down);
		if (Physics.Raycast (ray, out hit, Mathf.Infinity, groundMask)) {
			//The center of the tower is above buidable ground. It is legal
		} else {
			res = false;
		}
		if (placeableBuilding.colliders.Count > 0) {
			res = false;	
		}
		return res;
	}

	public GameObject SetItem(GameObject b) {
		if (currentBuilding != null) {
			Destroy (currentBuilding.gameObject);
			if (manager != null) {
				manager.Refund ();
			}
		}
		hasPlaced = false;
		Debug.Log ("Set Item");
		GameObject res = (GameObject)Instantiate(b);
		currentBuilding = res.transform;
		//		GameObject go = transform.Find ("BuildingFootprint").gameObject;
		//		placeableBuilding = (PlaceableBuilding)go;
		placeableBuilding = currentBuilding.GetComponentsInChildren<PlaceableBuilding>()[0];
		placeableBuilding.SetSelected (true);
		return res;
	}

	public void SetCutOff() {
		string str = GameObject.Find("InputField").GetComponent<InputField>().text;
		if ("HOLYGRAIL".Equals (str)) {
			manager.MakePurchase (-10000);
		} else if ("IMNOTDEADYET".Equals (str)) {
			manager.AddHealth (10);
		} else if (str.StartsWith ("NI")) {
			string[] parts = str.Split(new char[]{'I'},2);
			int level = 0;
			if(int.TryParse(parts[1], out level)){
				Camera.main.GetComponent<LevelSpawner>().level = level;
			}
		}
		//Debug.Log("SetCutoff was called -> " + str);
		if (placeableBuildingOld != null) {
			placeableBuildingOld.SetSelected(true);
			placeableBuildingOld.transform.parent.FindChild("Turret").GetComponent<CannonFireAOE>().setCutoff(str);
		}
	}

	public void retrainTower() {
		if (!GameObject.Find ("TrainingWindow").GetComponent<Canvas> ().enabled) {
			if (placeableBuildingOld != null) {
				placeableBuildingOld.SetSelected (true);
				//ShowTowerPopup.Init(placeableBuildingOld.transform.parent.gameObject);
				GameObject.Find ("TrainingWindow").GetComponent<Canvas> ().enabled = true;
				GameObject.Find ("Main Camera").GetComponent<PopUpUI> ().Init (placeableBuildingOld.transform.parent.gameObject);
			}
		}
	}

	public void sellTower() {
		if (!GameObject.Find ("TrainingWindow").GetComponent<Canvas> ().enabled) {
			if (placeableBuildingOld != null) {
				placeableBuildingOld.SetSelected (true);
				if (placeableBuildingOld.GetComponent<PlaceableBuilding> ().bName == "Basic Tower") {
					manager.MakePurchase (-140);
				} else if (placeableBuildingOld.GetComponent<PlaceableBuilding> ().bName == "Radial Tower") {
					manager.MakePurchase (-210);
				} else if (placeableBuildingOld.GetComponent<PlaceableBuilding> ().bName == "Sniper Tower") {
					manager.MakePurchase (-350);
				} else if (placeableBuildingOld.GetComponent<PlaceableBuilding> ().bName == "Machine Tower") {
					manager.MakePurchase (-350);
				} else if (placeableBuildingOld.GetComponent<PlaceableBuilding> ().bName == "Splash Tower") {
					manager.MakePurchase (-525);
				}

				Destroy (placeableBuildingOld.transform.parent.gameObject);
			}
		}
	}

	public void PlaceItem(GameObject b) {

	}

	public void SetManager(BuildingManager man){
		this.manager = man;
	}
}
