using UnityEngine;
using System.Collections;

public class Health : MonoBehaviour {

	//The tower that should be built
	public GameObject towerPrefab;

	//To build stuff
	void OnMouseUpAsButton () {
		GameObject g = (GameObject)Instantiate (towerPrefab);
		g.transform.position = transform.position + Vector3.up;
	}
}
