using UnityEngine;
using System.Collections;

public class BuildingPlacement : MonoBehaviour {
	
	public float scrollSensitivity;
	
	private PlaceableBuilding placeableBuilding;
	private Transform currentBuilding;
	public bool hasPlaced;
	
	public LayerMask buildingsMask;
	public LayerMask groundMask;
	
	private PlaceableBuilding placeableBuildingOld;
	
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
			Debug.Log ("Not null");
			currentBuilding.position = new Vector3(p.x,p.y,p.z);

			if (Input.GetMouseButtonDown(0)) {
				if (IsLegalPosition(p)) {
					Debug.Log ("Placing Building");
					hasPlaced = true;	
					placeableBuilding.SetSelected (false);
					placeableBuilding.SetPlaced (true);
					currentBuilding = null;
				}
			}
		}
		else {
			if (Input.GetMouseButtonDown(0)) {
				// hasPlaced = true;
				RaycastHit hit = new RaycastHit();
				ray = new Ray(new Vector3(p.x,10,p.z), Vector3.down);
				if (Physics.Raycast(ray, out hit,Mathf.Infinity,buildingsMask)) {
					if (placeableBuildingOld != null) {
						placeableBuildingOld.SetSelected(false);
					}
					hit.collider.gameObject.GetComponent<PlaceableBuilding>().SetSelected(true);
					placeableBuildingOld = hit.collider.gameObject.GetComponent<PlaceableBuilding>();
				}
				else {
					if (placeableBuildingOld != null) {
						placeableBuildingOld.SetSelected(false);
					}
				}
			}
		}
	}


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

	public void SetItem(GameObject b) {
		if (currentBuilding != null) {
			try{
				Destroy (currentBuilding.gameObject);
			}catch(UnityException e){
				Debug.Log ("This throws a null pointer exception for some reason");
			}
		}
		hasPlaced = false;
		Debug.Log ("Set Item");
		currentBuilding = ((GameObject)Instantiate(b)).transform;
//		GameObject go = transform.Find ("BuildingFootprint").gameObject;
//		placeableBuilding = (PlaceableBuilding)go;
		placeableBuilding = currentBuilding.GetComponentsInChildren<PlaceableBuilding>()[0];
		placeableBuilding.SetSelected (true);
	}

	public void PlaceItem(GameObject b) {
		
	}
}
