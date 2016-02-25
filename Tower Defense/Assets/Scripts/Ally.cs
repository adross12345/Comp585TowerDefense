using UnityEngine;
using System.Collections;

public class Ally : Unit {
	public static double ALLY_IDENTITY = 1.0;
	public void Start(){
		this.identity = ALLY_IDENTITY;
		base.Start ();
	}

	public override void OnTriggerEnter(Collider co) {

		//If the value equals castle then the enemy will deal damage
		if (co.name == "Castle") {
			//co.GetComponentInChildren<CastleHealth> ().decrease ();
			Destroy (gameObject);	
		}
	}
}
