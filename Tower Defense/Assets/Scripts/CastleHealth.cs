using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CastleHealth : MonoBehaviour {

	public float maxHealth = 1.00f;
	public float curHealth = 0f;
	public static float damage;
	public GameObject healthBar;


    [SerializeField]
    private Text healthText = null;



	// Use this for initialization
	void Start () 
	{
		curHealth = maxHealth;
		//Tests the Tower Health Bar
		//InvokeRepeating ("decreaseHealth", 1f, 1f);
	}

	public static void changeDamage(float value)
	{
		damage = value;
	}

	//Decreases castle health when enemy reaches castle.
	void decreaseHealth(float damage)
	{
		curHealth -= damage / 100.0f;
		Debug.Log ("curHealth" + curHealth);

		float calcHealth = curHealth / maxHealth;
		Debug.Log ("calcHealth" + calcHealth);

		setHealthBar (calcHealth);

		if (curHealth <= 0f) {
			Destroy (gameObject);
		}
	}

	public void OnTriggerEnter(Collider co) {
		if (co.tag == "Enemy") {
			Debug.Log ("Castle Hit");
			float damage = 10.0f;
			changeDamage (damage);
			decreaseHealth(damage);	
		}

	}

	public void setHealthBar(float castleHealth)
	{
		healthBar.transform.localScale = new Vector3(curHealth, 
			healthBar.transform.localScale.y, healthBar.transform.localScale.z);
	}
}