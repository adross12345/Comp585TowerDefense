using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {

	public float mySpeed = 10;
	public float myDamage = 0.5f;
	public float myRange = 999;
	//	protected GameObject projectile;
	protected Unit target;
	protected bool startFire = false;
	//	protected CannonFire source;

	public float splashDamage = 0.25f;

	protected float distance;

	protected virtual void Start(){
		distance = 0;
	}

	protected virtual void Update(){
		if (target != null && startFire) {
			transform.LookAt (target.transform);
			transform.Translate (Vector3.forward * mySpeed * Time.deltaTime);
		}
	}

	protected virtual void OnTriggerEnter(Collider other){	
		Unit u = other.gameObject.GetComponent<Unit> ();
		if(u == target && other is BoxCollider){
			if (u != null) {
				u.takeDamage (myDamage);
			}
			target.removeFromAimedAtMe (this);
			Destroy (gameObject);
		}
	}

	public virtual void setTarget(Unit u){
		this.target = u;
		u.addToAimedAtMe (this);
		startFire = true;
	}

	public virtual void SetDamage(float damage){
		this.myDamage = damage;
	}

	public virtual void SetSplashDamage(float spDam){
		this.splashDamage = spDam;
	}

	public void killYourself(){
		Destroy (this.gameObject);
	}


}
