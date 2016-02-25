using UnityEngine;
using System.Collections;

public class CastleHealth : MonoBehaviour {

	public float maxHealth = 100f;
	public float curHealth = 0f;
	public GameObject healthBar;



	// Use this for initialization
	void Start () 
	{
		curHealth = maxHealth;

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	//Decreases castle health by one
	void decreaseHealth()
	{
		curHealth -= 1;
	}

	public void setHealthBar(float castleHealth)
	{
		healthBar.transform.localScale = new Vector3(myHealth, , ,);
	}
}
