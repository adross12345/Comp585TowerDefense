using UnityEngine;
using System.Collections;

public class Enemy : Unit {
	public static double ENEMY_IDENTITY = 0.0;
	float damage = 1f;

	public GameObject enemy;
	public GameObject castle;

	new public void Awake(){
		this.identity = ENEMY_IDENTITY;
		//Tests enemy Healthbar
//		InvokeRepeating ("decreaseHealth", 1f, 1f);
		base.Start ();
	}

	public override void OnTriggerEnter(Collider co) {
		if (co.name == "Castle") {
			Destroy (enemy);	
		}
	}

}
