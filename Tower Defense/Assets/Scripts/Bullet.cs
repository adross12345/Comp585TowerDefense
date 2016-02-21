using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {

	//Speed
	public int speed = 10;

	public Transform target;

	void FixedUpdate(){

		if (target) {
			Vector3 dir = target.position - transform.position;
			GetComponent<Rigidbody> ().velocity = dir.normalized * speed;
		}
		else {
			Destroy (gameObject);
		}
	}

	void OnTriggerEnter(Collider co){
	
//		Health health = co.GetComponentsInChildren(Health);
//
//		if (health) {
//			health.decrease();
//			Destroy (gameObject);
//
//		}
	
	}
}
