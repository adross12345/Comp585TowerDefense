using UnityEngine;
using System.Collections;

public class UnitAnimated : Unit {
	public Texture2D[] textures;
	public float timePerTexture;
	protected float timeOfLastSwap;
	public int indexTexture;

	// Use this for initialization
	protected override void Awake () {
		base.Awake ();
		indexTexture = 0;
		timeOfLastSwap = Time.time;
	}

	public override void addNoise(float noise){
		this.noise = noise;
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
		for (int i = 0; i < textures.Length; i++) {
			Texture2D arrayTex = textures [i];
			Color[] pixels = arrayTex.GetPixels ();
			//This code works for adding noise to the textures.
			Texture2D newTex = new Texture2D (arrayTex.width, arrayTex.height);
			for (int x = 0; x < newTex.width; x++) {
				for (int y = 0; y < newTex.height; y++) {
					Color c = pixels [y * newTex.width + x];
					Color newC = new Color (c.r + Random.Range (-1*noise, noise), c.g + Random.Range (-1*noise, noise), c.b + Random.Range (-1*noise, noise), c.a);
					newTex.SetPixel (x, y, newC);
				}
			}
			newTex.Apply ();
			textures [i] = newTex;
		}
	}

	void Update(){
		if (Time.time >= timeOfLastSwap + timePerTexture) {
			timeOfLastSwap = Time.time;
			indexTexture++;
			if (indexTexture >= textures.Length) {
				indexTexture = 0;
			}
			if (textures.Length > 0) {
				this.GetComponent<MeshRenderer> ().material.mainTexture = textures [indexTexture];
			}
		}
	}

	public void SetTimePerTexture(float tpt){
		timePerTexture = tpt;
	}


}
