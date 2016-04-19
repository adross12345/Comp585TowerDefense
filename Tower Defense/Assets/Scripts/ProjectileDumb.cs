using UnityEngine;
using System.Collections;

public class ProjectileDumb : Projectile {

	// Use this for initialization
	protected override void Start () {
		base.Start ();
	}

	// Update is called once per frame
	protected override void Update () {
		if (startFire) {
			if (distance >= myRange) {
				Destroy (this.gameObject);
			}
			float deltaDist = mySpeed * Time.deltaTime;
			transform.Translate (Vector3.forward * deltaDist);
			distance += deltaDist;
		}
	}

	protected override void OnTriggerEnter(Collider other){	
		Unit u = other.gameObject.GetComponent<Unit> ();
		if (u != null && other is BoxCollider) {
			u.takeDamage (myDamage);
			Destroy (this.gameObject);
		}
	}

	public override void setTarget(Unit u){
		this.target = u;
		startFire = true;
	}
}
