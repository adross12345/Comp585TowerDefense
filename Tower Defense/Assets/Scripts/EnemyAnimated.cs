using UnityEngine;
using System.Collections;

public class EnemyAnimated : Enemy {
	public Texture2D[] textures;
	public float timePerTexture;
	protected float nextTextureSwap;
	protected int indexTexture;

	// Use this for initialization
	protected override void Awake () {
		base.Awake ();
		indexTexture = 0;
	}

	protected override void Update(){
		if (Time.time >= nextTextureSwap) {
			nextTextureSwap = Time.time + timePerTexture;
			indexTexture++;
			if (indexTexture >= textures.Length) {
				indexTexture = 0;
			}
			if (textures.Length > 0) {
				this.GetComponent<MeshRenderer> ().material.mainTexture = textures [indexTexture];
			}
		}
	}


}
