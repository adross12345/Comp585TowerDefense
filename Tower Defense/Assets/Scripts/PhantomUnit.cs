using UnityEngine;
using System.Collections;

public class PhantomUnit : ScriptableObject {
	public Texture tex;
	public double identity;

	// Use this for initialization
	public void Initialize (Texture tex, double identity) {
		this.tex = tex;
		this.identity = identity;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
