using UnityEngine;
using System.Collections;

public class Enemy : Unit {
	public static double ENEMY_IDENTITY = 0.0;
	public void Start(){
		this.identity = ENEMY_IDENTITY;
		base.Start ();
	}

	public override void OnTriggerEnter(Collider co) {

		//If the value equals castle then the enemy will deal damage
		if (co.name == "Castle") {
			co.GetComponentInChildren<TowerHealth> ().decrease ();
			Destroy (gameObject);	
		}
	}
}
