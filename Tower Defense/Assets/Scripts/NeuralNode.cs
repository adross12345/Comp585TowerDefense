using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class NeuralNode : ScriptableObject {
	public double[] weights;
	protected double b;
	protected double learningRate = 0.1;
	protected int unitWidth = -1;
	protected int unitHeight = -1;
	protected int iters = 100;
	protected List<PhantomUnit> trainingSet;

	public void Start () {
	}

	// Use this for initialization
	public void Awake () {
		trainingSet = new List<PhantomUnit> ();
	}
		
	public abstract void LearnUnits ();

	public abstract double calculateZ (double[] features);

	public abstract double calculateZ (Unit unit);

	public abstract Texture2D getAllyTexture ();

	public abstract Texture2D getEnemyTexture ();

	public static float convertToColorVal(double val){
		float res = (float)val;
		if (res < 0) {
			res = 0;
		} else if (res > 1) {
			res = 1;
		}
		return res;
	}

	public static void Shuffle<T>(List<T> list)  
	{  
		int n = list.Count;  
		while (n > 1) {  
			n--;  
			int k = (int) Mathf.Round(Random.Range(0.0F, (float) n));  
			T value = list[k];  
			list[k] = list[n];  
			list[n] = value;  
		}  
	}

	public void AddToTrainingSet(Unit unit){
		MeshRenderer mr = unit.GetComponent<MeshRenderer> ();
		Texture tex = mr.material.mainTexture;
		PhantomUnit pu = ScriptableObject.CreateInstance<PhantomUnit>();
		pu.Initialize (tex, unit.identity);
		trainingSet.Add (pu);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
