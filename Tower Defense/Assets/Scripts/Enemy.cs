using UnityEngine;
using System.Collections;


public class Enemy : Unit {

	void Awake(){
		this.identity = ENEMY_IDENTITY;
		//Tests enemy Healthbar
//		InvokeRepeating ("decreaseHealth", 1f, 1f);
		base.Start ();
	}


	public override void OnTriggerEnter(Collider co) {
		if (co.tag == "Castle") {
			StartCoroutine(DestroySelf());
		}
	}


}
