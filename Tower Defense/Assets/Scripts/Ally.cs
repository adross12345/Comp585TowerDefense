using UnityEngine;
using System.Collections;

public class Ally : Unit {

	void Awake(){
		this.identity = ALLY_IDENTITY;
		base.Start ();
	}

	protected override void OnTriggerEnter(Collider co) {
	}
}
