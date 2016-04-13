using UnityEngine;
using System.Collections;


public class Enemy : Unit {

	protected virtual void Awake(){
		this.identity = ENEMY_IDENTITY;
		//Tests enemy Healthbar
		//		InvokeRepeating ("decreaseHealth", 1f, 1f);
		base.Start ();
	}


	protected override void OnTriggerEnter(Collider co) {
	}


}
