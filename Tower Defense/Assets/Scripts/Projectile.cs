using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {

	public float mySpeed = 10;
	public float myDamage = 0.5f;
	private GameObject projectile;
	private Unit target;
	private CannonFire source;

	private float distance;

	void Start(){
	}

	void Update(){
		if (target != null) {
			transform.LookAt (target.transform);
			transform.Translate (Vector3.forward * mySpeed * Time.deltaTime);
		}
	}

	void OnTriggerEnter(Collider other){	
		Unit u = other.gameObject.GetComponent<Unit> ();
		if(u == target){
			if (u != null) {
				u.decreaseHealth (myDamage);
			}
			target.removeFromAimedAtMe (this);
			Destroy (gameObject);
		}
	}

	public void setTarget(Unit u){
		this.target = u;
		u.addToAimedAtMe (this);
	}

	public void killYourself(){
		Destroy (this.gameObject);
	}
		

}
