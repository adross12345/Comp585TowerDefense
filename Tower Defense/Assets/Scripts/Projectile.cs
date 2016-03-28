using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {

	public float mySpeed = 10;
	public float myDamage = 0.5f;
	private GameObject projectile;
	private GameObject target;
	private CannonFire source;

	private float distance;

	void Start(){
	}

	void Update(){
		if (target != null) {
			transform.LookAt (target.transform);
			transform.Translate (Vector3.forward * mySpeed * Time.deltaTime);
//			nav.destination = target.transform.position;
		}
	}

	void OnTriggerEnter(Collider other){	
		if(other.gameObject==target){
			Unit u = other.gameObject.GetComponent<Unit> ();
			if (u != null) {
				u.decreaseHealth (myDamage);
			}
			Destroy (gameObject);
		}
	}

	public void setTarget(GameObject go){
		this.target = go;
	}
		

}
