using UnityEngine;
using System.Collections;

public class EnemyAnimated : Enemy {
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

	protected override void Update(){
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
