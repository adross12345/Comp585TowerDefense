using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {

	//Speed
	public int speed = 10;

	public Transform Enemy;

	void FixedUpdate(){

		if (Enemy) {
			Vector3 dir = Enemy.position - transform.position;
			GetComponent<Rigidbody> ().velocity = dir.normalized * speed;
		}
		else {
			Destroy (gameObject);
		}
	}

	void OnTriggerEnter(Collider co){
	
		Health health = co.GetComponentsInChildren(Health);

		if (health) {
			EnemyHealth.decrease();
			Destroy (gameObject);

		}
	
	}
}
