using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class Unit : MonoBehaviour {
	public static double ALLY_IDENTITY = 1.0;
	public static double ENEMY_IDENTITY = 0.0;

	public double identity;
	public float speed=3;
	public float acceleration=8;
	public float maxHealth = 1.00f;
	public float curHealth = 1.00f;
	//Need to find a way to link healthbar to each instance of enemy/ally
	public GameObject healthBar;
	public float damage=10f;
	public float money=13;
	public float armor = 0f;
	protected float noise;

	protected NavMeshAgent nav;

	protected Vector3 destination;

	protected List<Projectile> aimedAtMe;

	// Use this for initialization
	protected virtual void Awake () {

		curHealth = maxHealth;
		aimedAtMe = new List<Projectile> ();
		//Moves enemy toward castle
		GameObject castle = GameObject.Find ("Castle");
		if (castle) {
			nav = GetComponent<NavMeshAgent> ();
			if (nav.enabled) {
				nav.destination = castle.transform.position;
				this.destination = castle.transform.position;
				nav.speed = speed;
				nav.acceleration = acceleration;
			}
		}
		setHealthBar (curHealth / maxHealth);
	}

	//Decreases health when enemy gets hit.
	//Returns true if the enemy died due to it.
	public bool takeDamage(float damage)
	{
		bool res = false;
		curHealth -= (damage*100)/(armor+100);
		float calcHealth = curHealth / maxHealth;
		setHealthBar (calcHealth);
		if (curHealth <= 0f) {
			DestroyMe ();
			res = true;
		}
		return res;
	}

	public void setHealthBar(float healthPercent)
	{
		Transform bar = this.healthBar.transform;
		bar.GetChild(2).localScale = new Vector3(healthPercent, 
			bar.localScale.y, bar.localScale.z);
	}

	public virtual void addNoise(float noise){
		MeshRenderer mr = this.GetComponent<MeshRenderer> ();
		Texture tex = mr.material.mainTexture;
		if (tex is Texture2D) {
			Texture2D tex2D = (Texture2D)tex;
			Color[] pixels = tex2D.GetPixels ();
			//This code works for adding noise to the textures.
			Texture2D newTex = new Texture2D (tex2D.width, tex2D.height);
			for (int x = 0; x < newTex.width; x++) {
				for (int y = 0; y < newTex.height; y++) {
					Color c = pixels [y * newTex.width + x];
					Color newC = new Color (c.r + Random.Range (-1*noise, noise), c.g + Random.Range (-1*noise, noise), c.b + Random.Range (-1*noise, noise), c.a);
					newTex.SetPixel (x, y, newC);
				}
			}
			newTex.Apply ();
			mr.material.mainTexture = newTex;
		}
		this.noise = noise;
	}

	public float getMoney(){
		return this.money;
	}

	public float getDamage(){
		return this.damage;
	}

	public void setTexture(Texture tex){
		MeshRenderer mr = this.GetComponent<MeshRenderer> ();
		mr.material.mainTexture = tex;
	}

	public void addToAimedAtMe(Projectile p){
		aimedAtMe.Add (p);
	}

	public void removeFromAimedAtMe(Projectile p){
		aimedAtMe.Remove (p);
	}

	public void DestroyMe(){
		StartCoroutine(DestroySelf());
	}

	protected virtual IEnumerator DestroySelf(){
		foreach (Projectile p in aimedAtMe) {
			p.killYourself ();
		}
		NavMeshAgent nav = gameObject.GetComponent<NavMeshAgent> ();
		nav.enabled = false;
		transform.position = new Vector3 (-500, -500, -500);
		yield return new WaitForSeconds(0.1f);
		Destroy (gameObject);
	}

	public void SetSpeedAndAccel(float speed, float accel){
		this.speed = speed;
		this.acceleration = accel;
		NavMeshAgent nav = GetComponent<NavMeshAgent> ();
		if (nav.enabled) {
			nav.speed = speed;
			nav.acceleration = accel;
		}
	}

	public void SetArmor(float newArmor){
		this.armor = newArmor;
	}

	public virtual bool EnterTower(){
		return false;
		//Fill this in in submethods.
	}

	//	void OnDestroy(){
	//		foreach (Projectile p in aimedAtMe) {
	//			p.killYourself ();
	//		}
	//		transform.position = new Vector3 (0, -500, 0);
	//	}

	public virtual void SetSplitsRemaining(int splitsRemaining){
		//For most units this will do nothing.
	}
		
}
