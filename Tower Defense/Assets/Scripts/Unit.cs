using UnityEngine;
using System.Collections;

public abstract class Unit : MonoBehaviour {
	public double identity;
	// Use this for initialization
	protected void Start () {
		//Moves enemy toward castle
		GameObject castle = GameObject.Find ("Castle");

		if (castle) {
			GetComponent<NavMeshAgent> ().destination = castle.transform.position;
		}
		//TODO change
//		NeuralNode node = new NeuralNode ();
//		node.LearnUnits (this);
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
