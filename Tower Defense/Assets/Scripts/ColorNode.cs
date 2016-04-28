using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ColorNode : NeuralNode {
	int itersUsed = 1;

	public override void LearnUnits(){
		//I should probably make this a float array. 
		//I'm not sure what kind of memory restrictions we're looking at.
		//This list of arrays is going to be reclaimed after the method finishes, as is the trainingSet
		List<double[]> features = new List<double[]> ();
		int featureLength = -1;
		//Gets the relevant data out of the training set
		foreach(PhantomUnit u in trainingSet){
			Texture tex = u.tex;
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
					//The last spot in unit features is reserved for the unit identity (0 for Enemy, 1 for Ally)
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
					Debug.Log ("Unit has incorrect dimensions to be analyzed: " + u.ToString());
				}
			} else if (tex is RenderTexture) {
				RenderTexture texRend = (RenderTexture)tex;
				Debug.Log ("This is a RenderTexture");
				//I don't have the code to get the pixels for this yet
			} else {
				Debug.Log ("I'm not sure what Texture this is or what to do with it.");
			}
		}//for

		weights = new double[featureLength];
		b = 0;

		for (int it = 0; it < iters; it++) {
			itersUsed = it+1;
			int misses = 0;
			//Shuffles the feature list 
			NeuralNode.Shuffle (features);
			foreach(double[] fs in features){
				int trueIdentity = (int) Mathf.Round((float)fs [featureLength]);
				int f = (int) Unit.ALLY_IDENTITY;
				if (calculateZ (fs) < 0) {
					f = (int) Unit.ENEMY_IDENTITY;
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
		actualWeights = new double[weights.Length];
		System.Array.Copy (weights, actualWeights, weights.Length);
		actualB = b;
		SetNonzeroIndices ();
		//Notice that I am clearing out the training set here.
		//Keeping the references is not necessary and will just take up space.
		//Being able to get rid of them is sort of the point of making a model.
		foreach(PhantomUnit u in trainingSet){
			Destroy (u);
		}
		trainingSet.Clear();
		isAILearned = true;

	}

	public override double calculateZ(double[] features){
		double z = 0;
		for (int i = 0; i < weights.Length; i++) {
			z += weights [i] * features [i];
		}
		z += b;
		return z;
	}

	public override double calculateZ(Unit unit){
		double res = 0;
		MeshRenderer mr = unit.GetComponent<MeshRenderer> ();
		Texture tex = mr.material.mainTexture;
		if (tex is Texture2D) {
			Texture2D tex2D = (Texture2D)tex;
			Color[] pixels = tex2D.GetPixels ();
			res = calculateZ (pixels);
		}
		return res;
	}

	public override double calculateZ(Color[] pixels){
		double res = 0;
		if (pixels.Length * 3 == weights.Length) {
			//This is really IMPORTANT.
			//The last spot in unit features is reserved for the unit identity (0 for Enemy, 1 for Ally)
			//Don't iterate over unitFeatures or its full length.
			double[] unitFeatures = new double[weights.Length];
			//Assigns the pixel values for each of the colors in the pattern {r,g,b,r,g,b,...)
			for (int i = 0; i < pixels.Length; i++) {
				Color c = pixels [i];
				unitFeatures [3 * i] = c.r;
				unitFeatures [3 * i + 1] = c.g;
				unitFeatures [3 * i + 2] = c.b;
			}
			res = calculateZ (unitFeatures);
		} else {
			Debug.Log ("Unit has incorrect dimensions to be analyzed");
		}
		return res;
	}

	public override Texture2D getFeatureTexture(){
		Texture2D newTex = new Texture2D (unitWidth, unitHeight);
		if (targetTex == null) {
			return newTex;
		}
		return targetTex;
	}

	public override Texture2D getAllyTexture(){
		Texture2D newTex = new Texture2D (unitWidth, unitHeight);
		float multiplyWeight = 1.0f / ((float) learningRate * itersUsed);
		for (int x = 0; x < newTex.width; x++) {
			for (int y = 0; y < newTex.height; y++) {
				float r = convertToColorVal(multiplyWeight * weights [(y * newTex.width + x) * 3]);
				float g = convertToColorVal(multiplyWeight * weights [(y * newTex.width + x) * 3 + 1]);
				float b = convertToColorVal(multiplyWeight * weights [(y * newTex.width + x) * 3 + 2]);
				//				float r = convertToColorVal(1 * weights [(y * newTex.width + x) * 3]);
				//				float g = convertToColorVal(1 * weights [(y * newTex.width + x) * 3 + 1]);
				//				float b = convertToColorVal(1 * weights [(y * newTex.width + x) * 3 + 2]);
				Color c = new Color(r,g,b);
				newTex.SetPixel (x, y, c);
			}
		}
		newTex.Apply ();
		return newTex;
	}

	public override Texture2D getEnemyTexture(){
		Texture2D newTex = new Texture2D (unitWidth, unitHeight);
		float multiplyWeight = 1.0f / ((float) learningRate * itersUsed);
		for (int x = 0; x < newTex.width; x++) {
			for (int y = 0; y < newTex.height; y++) {
				//				float r = convertToColorVal(-1 * weights [(y * newTex.width + x) * 3]);
				//				float g = convertToColorVal(-1 * weights [(y * newTex.width + x) * 3 + 1]);
				//				float b = convertToColorVal(-1 * weights [(y * newTex.width + x) * 3 + 2]);
				float r = convertToColorVal(-multiplyWeight * weights [(y * newTex.width + x) * 3]);
				float g = convertToColorVal(-multiplyWeight * weights [(y * newTex.width + x) * 3 + 1]);
				float b = convertToColorVal(-multiplyWeight * weights [(y * newTex.width + x) * 3 + 2]);
				Color c = new Color(r,g,b);
				newTex.SetPixel (x, y, c);
			}
		}
		newTex.Apply ();
		return newTex;
	}

	public override NeuralNode Clone(){
		ColorNode res = (ColorNode) NeuralNode.create (NodeType.FULLCOLOR);
		res.SetWeights (actualWeights, this.b);
		res.SetNonzeroIndices ();
		return res;
	}
}
