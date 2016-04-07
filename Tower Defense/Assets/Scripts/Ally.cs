using UnityEngine;
using System.Collections;

public class Ally : Unit {

	void Awake(){
		this.identity = ALLY_IDENTITY;
		base.Start ();
	}

	public override void OnTriggerEnter(Collider co) {

		//If the value equals castle then the enemy will deal damage
		if (co.name == "Castle") {
			StartCoroutine(DestroySelf());
		}
	}
}
