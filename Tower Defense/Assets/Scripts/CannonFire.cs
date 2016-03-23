using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CannonFire : MonoBehaviour {


	public List<GameObject> enemiesInRange;



	void Start ()
	{
		enemiesInRange = new List<GameObject>();
	}
		
	//Picks up the target that comes into the turrents range
	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "Enemy") 
		{
			Debug.Log ("Added to list");
			enemiesInRange.Add(other.gameObject);
		}
	}

	//Removes target from list when it leaves range
	void OnTriggerExit(Collider other)
	{
		if (other.gameObject.tag == ("Enemy")) {
			Debug.Log ("Remove from list");
			enemiesInRange.Remove(other.gameObject);
		}
	}
		
}