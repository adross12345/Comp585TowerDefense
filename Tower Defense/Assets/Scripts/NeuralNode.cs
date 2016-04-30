using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class NeuralNode : ScriptableObject {
	public double[] weights;
	protected double[] actualWeights;
	public double b;
	protected double actualB;
	protected double learningRate = 0.01;
	protected int unitWidth = 32;
	protected int unitHeight = 32;
	protected int iters = 100;
	protected List<PhantomUnit> trainingSet;
	protected int[] nonzeroIndices;
	protected List<EnemyAIConfound> enemiesToInform;
	protected List<int> confoundedIndices;
	public bool isAILearned = false;

	public Texture2D targetTex;
	public double lastZ;

	public Unit target;

	public enum NodeType{FULLCOLOR, COLORHIST, GRAYSCALE, CONVOLVED, COMBINATION}

	public static NeuralNode create(NodeType nodeType){
		NeuralNode res = null;
		if (nodeType == NodeType.FULLCOLOR) {
			res = ScriptableObject.CreateInstance<ColorNode> ();
		}else if (nodeType == NodeType.COLORHIST) {
			res = ScriptableObject.CreateInstance<ColorHistNode> ();
		}else if (nodeType == NodeType.GRAYSCALE) {
			res = ScriptableObject.CreateInstance<GrayscaleNode> ();
		}
//		else if (nodeType == NodeType.CONVOLVED) {
//			res = ScriptableObject.CreateInstance<ConvolvedNode> ();
//		}else if (nodeType == NodeType.COMBINATION) {
//			res = ScriptableObject.CreateInstance<CombinationNode> ();
//		}
		return res;
	}

	void Start () {
	}

	// Use this for initialization
	protected void Awake () {
		trainingSet = new List<PhantomUnit> ();
		nonzeroIndices = new int[0];
		confoundedIndices = new List<int> ();
		enemiesToInform = new List<EnemyAIConfound> ();
		weights = new double[unitWidth * unitHeight * 3];
		targetTex = new Texture2D (unitWidth, unitHeight);
		learningRate = 1.0 / iters;
	}

	public abstract void LearnUnits ();

	public abstract double calculateZ (double[] features);

	public abstract double calculateZ (Unit unit);

	public abstract double calculateZ (Color[] pixels);

	public abstract Texture2D getAllyTexture ();

	public abstract Texture2D getEnemyTexture ();

	public abstract Texture2D getFeatureTexture ();

	public abstract NeuralNode Clone ();

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

	public void AddToTrainingSet(Unit unit, double identity){
		MeshRenderer mr = unit.GetComponent<MeshRenderer> ();
		Texture tex = mr.material.mainTexture;
		PhantomUnit pu = ScriptableObject.CreateInstance<PhantomUnit>();
		pu.Initialize (tex, identity);
		trainingSet.Add (pu);
	}

	public void AddToTrainingSet(Unit unit, bool isEnemy){
		double identity = 1.0;
		if (isEnemy) {
			identity = 0.0;
		}
		MeshRenderer mr = unit.GetComponent<MeshRenderer> ();
		Texture tex = mr.material.mainTexture;
		PhantomUnit pu = ScriptableObject.CreateInstance<PhantomUnit>();
		pu.Initialize (tex, identity);
		trainingSet.Add (pu);
	}

	public void AddToTrainingSet(PhantomUnit pu){
		trainingSet.Add (pu);
	}

	public void AddToTrainingSet(Color[] pixels, double identity, int width, int height){
		Texture2D tex = new Texture2D (width, height);
		tex.SetPixels (pixels);
		tex.Apply ();
		PhantomUnit pu = ScriptableObject.CreateInstance<PhantomUnit>();
		pu.Initialize (tex, identity);
		trainingSet.Add (pu);
	}

	public void SetWeights(double[] newWeights, double newB){
		actualWeights = new double[newWeights.Length];
		this.weights = new double[newWeights.Length];
		System.Array.Copy (newWeights, actualWeights, newWeights.Length);
		System.Array.Copy (newWeights, weights, newWeights.Length);
		this.b = newB;
		this.actualB = newB;
	}

	protected void SetNonzeroIndices(){
		List<int> nonzeros = new List<int> ();
		for (int i = 0; i < actualWeights.Length; i++) {
			//			if (actualWeights [i] != 0) {
			//				nonzeros.Add (i);
			//			}
			nonzeros.Add(i);
		}
		nonzeros.Add (-1);
		nonzeroIndices = new int[nonzeros.Count];
		int j = 0;
		foreach (int index in nonzeros) {
			nonzeroIndices [j] = index;
			j++;
		}
	}//setNonzeroIndices()

	public int[] GetNonzeroIndices(){
		return nonzeroIndices;
	}

	public void ResetWeights(List<int> indices){
		foreach (int index in indices) {
			if (index == -1) {
				b = actualB;
			} else {
				weights [index] = actualWeights [index];
			}
			confoundedIndices.Remove (index);
		}
	}

	public void ResetAllWeights(){
		for (int i = 0; i < actualWeights.Length; i++) {
			weights [i] = actualWeights [i];
		}
		b = actualB;
		confoundedIndices.Clear ();
	}

	public void AddToConfoundedIndices(int index){
		confoundedIndices.Add (index);
	}

	public List<int> GetConfoundedIndices(){
		return confoundedIndices;
	}

	public void AddToEnemiesToInform(EnemyAIConfound en){
		enemiesToInform.Add (en);
	}

	public void RemoveFromEnemiesToInform(EnemyAIConfound en){
		enemiesToInform.Remove (en);
	}

	public void InformEnemies(List<int> revertedIndices){
		foreach (EnemyAIConfound en in enemiesToInform) {
			en.RevertIndices (this, revertedIndices);
		}
	}

	public Texture2D GetTargetTex(){
		return targetTex;
	}

	public Unit GetTarget(){
		return target;
	}

	public void SetTarget(Unit u){
		this.target = u;
		this.targetTex = (Texture2D) u.GetComponent<MeshRenderer>().material.mainTexture;
	}

	// Update is called once per frame
	void Update () {

	}
}
