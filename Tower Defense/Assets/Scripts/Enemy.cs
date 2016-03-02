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
			float damage = 10f;
			CastleHealth.changeDamage (damage);
			Destroy (enemy);	
		}

	}

	//Decreases castle health when enemy reaches castle.
	void decreaseHealth()
	{
		curHealth -= .10f;
		float calcHealth = curHealth / maxHealth;
		setHealthBar (calcHealth);
		if (curHealth <= 0f) {
			Destroy (gameObject);
		}
	}

	public void setHealthBar(float enemyHealth)
	{
		healthBar.transform.localScale = new Vector3(curHealth, 
			healthBar.transform.localScale.y, healthBar.transform.localScale.z);
	}

}
