using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {


	// Use this for initialization
	void Start () {

		//Moves enemy toward castle
		GameObject castle = GameObject.Find ("Castle");

		if (castle) {
			GetComponent<NavMeshAgent> ().destination = castle.transform.position;
		}
	}

	void OnTriggerEnter(Collider co) {

		//If the value equals castle then the enemy will deal damage
		if (co.name == "Castle") {
			co.GetComponentInChildren<TowerHealth> ().decrease ();
			Destroy (gameObject);	
		}
	}
}
