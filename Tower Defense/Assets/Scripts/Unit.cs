﻿using UnityEngine;
using System.Collections;

public abstract class Unit : MonoBehaviour {
	public double identity;
	public int speed;
	public float maxHealth = 1.00f;
	public float curHealth = 1.00f;
	public GameObject healthBar;
	public double[] weights;

	// Use this for initialization
	protected void Start () {

		curHealth = maxHealth;
		//Moves enemy toward castle
		GameObject castle = GameObject.Find ("Castle");
		if (castle) {
			GetComponent<NavMeshAgent>().destination = castle.transform.position;
		}
	}

	//Decreases health when enemy gets hit.
	//Returns true if the enemy died due to it.
	public bool decreaseHealth(float damage)
	{
		bool res = false;
		curHealth -= damage;
		float calcHealth = curHealth / maxHealth;
		setHealthBar (calcHealth);
		if (curHealth <= 0f) {
			Destroy (gameObject);
			res = true;
		}
		return res;
	}

	public void setHealthBar(float healthPercent)
	{
		healthBar.transform.localScale = new Vector3(curHealth, 
			healthBar.transform.localScale.y, healthBar.transform.localScale.z);
	}

	public void addNoise(float noise){
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
					//TODO change amount of noise.
					Color newC = new Color (c.r + Random.Range (-1*noise, noise), c.g + Random.Range (-1*noise, noise), c.b + Random.Range (-1*noise, noise), c.a);
					newTex.SetPixel (x, y, newC);
				}
			}
			newTex.Apply ();
			mr.material.mainTexture = newTex;
		}
	}

	public void setTexture(Texture tex){
		MeshRenderer mr = this.GetComponent<MeshRenderer> ();
		mr.material.mainTexture = tex;
	}

	public abstract void OnTriggerEnter (Collider co);
	
	// Update is called once per frame
	public void Update () {
	
	}
}
