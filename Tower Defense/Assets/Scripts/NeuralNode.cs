using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NeuralNode : MonoBehaviour {
	double[] weights;
	double b;
	double learningRate;
	int unitWidth;
	int unitHeight;
	int iters;
	List<MonoBehaviour> trainingSet;
	// Use this for initialization
	void Start () {
		trainingSet = new List<MonoBehaviour> ();
		unitWidth = -1;
		unitHeight = -1;
	}
		
	public void LearnUnits(){
		//I should probably make this a float array. 
		//I'm not sure what kind of memory restrictions we're looking at.
		//This list of arrays is going to be reclaimed after the method finishes, as is the trainingSet
		List<double[]> features = new List<double[]> ();
		int featureLength = -1;
		//Gets the relevant data out of the training set
		foreach(MonoBehaviour subject in trainingSet){
			if (subject is Unit) {
				Unit u = (Unit)subject;
				MeshRenderer mr = u.GetComponent<MeshRenderer> ();
				Texture tex = mr.material.mainTexture;
				if (tex is Texture2D) {
					Texture2D tex2D = (Texture2D)tex;
					Color[] pixels = tex2D.GetPixels ();
					if (featureLength == -1) {
						featureLength = pixels.Length * 3;
						unitWidth = tex2D.width;
						unitHeight = tex2D.height;
					}
					//I don't actually check that the width and height are correct, just that width*height is correct. I'm going to assume it'll work out.
					if (pixels.Length * 3 == featureLength) {
						//This is really IMPORTANT.
						//The last spot in unit features is reserved for the unit identity (1 for Enemy, 0 for Ally)
						//Don't iterate over unitFeatures or its full length.
						double[] unitFeatures = new double[featureLength+1];
						//Assigns the pixel values for each of the colors in the pattern {r,g,b,r,g,b,...)
						for (int i = 0; i < pixels.Length; i++) {
							Color c = pixels [i];
							unitFeatures [3 * i] = c.r;
							unitFeatures [3 * i + 1] = c.g;
							unitFeatures [3 * i + 2] = c.b;
						}
						unitFeatures [featureLength] = u.identity;
						features.Add (unitFeatures);
					} else {
						Debug.Log ("Unit has incorrect dimensions to be analyzed: " + subject.ToString());
					}
				} else if (tex is RenderTexture) {
					RenderTexture texRend = (RenderTexture)tex;
					Debug.Log ("This is a RenderTexture");
					//I don't have the code to get the pixels for this yet
				} else {
					Debug.Log ("I'm not sure what Texture this is or what to do with it.");
				}
			} else {
				//The node should not be learning non-units. If this occurs it is a bug
				throw new System.ArgumentException("Node is trying to learn a non-unit","subject");
			}
		}//for


		weights = new double[featureLength];
		b = 0;

		for (int it = 0; it < iters; it++) {
			int misses = 0;
			//Shuffles the feature list 
			NeuralNode.Shuffle (features);
			foreach(double[] fs in features){
				int trueIdentity = (int) Mathf.Round((float)fs [featureLength]);
				int f = (int) Ally.ALLY_IDENTITY;
				if (calculateZ (fs) < 0) {
					f = (int) Enemy.ENEMY_IDENTITY;
				}
				misses += Mathf.Abs (f - trueIdentity);
				for (int i = 0; i < featureLength; i++) {
					weights [i] = weights [i] + learningRate * fs[i] * (trueIdentity - f);
				}
				b = b + learningRate * (trueIdentity - f);
			}//for each of the feature arrays
			if (misses == 0)
				break;
		}//for iterations
		//Notice that I am clearing out the training set here.
		//Keeping the references is not necessary and will just take up space.
		//Being able to get rid of them is sort of the point of making a model.
		trainingSet.Clear();
	}

	public double calculateZ(double[] features){
		double z = 0;
		for (int i = 0; i < weights.Length; i++) {
			z += weights [i] * features [i];
		}
		z += b;
		return z;
	}

	public Texture2D getAllyTexture(){
		Texture2D newTex = new Texture2D (unitWidth, unitHeight);
		for (int x = 0; x < newTex.width; x++) {
			for (int y = 0; y < newTex.height; y++) {
				float r = convertToColorVal(weights [(y * newTex.width + x) * 3]);
				float g = convertToColorVal(weights [(y * newTex.width + x) * 3 + 1]);
				float b = convertToColorVal(weights [(y * newTex.width + x) * 3 + 2]);
				Color c = new Color(r,g,b);
				newTex.SetPixel (x, y, c);
			}
		}
		newTex.Apply ();
		return newTex;
	}

	public Texture2D getEnemyTexture(){
		Texture2D newTex = new Texture2D (unitWidth, unitHeight);
		for (int x = 0; x < newTex.width; x++) {
			for (int y = 0; y < newTex.height; y++) {
				float r = convertToColorVal(-1 * weights [(y * newTex.width + x) * 3]);
				float g = convertToColorVal(-1 * weights [(y * newTex.width + x) * 3 + 1]);
				float b = convertToColorVal(-1 * weights [(y * newTex.width + x) * 3 + 2]);
				Color c = new Color(r,g,b);
				newTex.SetPixel (x, y, c);
			}
		}
		newTex.Apply ();
		return newTex;
	}

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

	public void AddToTrainingSet(MonoBehaviour unit){
		trainingSet.Add (unit);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
