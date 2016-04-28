using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ColorHistNode : NeuralNode {
	protected int numBins;
	public double totalPositiveWeights;
	public double totalNegativeWeights;

	new public void Awake(){
		numBins = 8;
		learningRate = .5;
		base.Awake ();
		weights = new double[numBins * numBins * numBins];
	}

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
					featureLength = numBins * numBins * numBins;
					unitWidth = tex2D.width;
					unitHeight = tex2D.height;
				}
				if (tex2D.width == unitWidth && tex2D.height == unitHeight) {
					//This is really IMPORTANT.
					//The last spot in unit features is reserved for the unit identity (0 for Enemy, 1 for Ally)
					//Don't iterate over unitFeatures or its full length.
					double[] unitFeatures = new double[featureLength+1];
					int divisor = 256 / numBins;
					double increment = 1.0 / (unitWidth * unitHeight);
					increment = 1.0;

					//Assigns the pixel values for each of the colors in the pattern increasing green, then increasing blue, then increasing red
					for (int i = 0; i < pixels.Length; i++) {
						Color c = pixels [i];
						//						Debug.Log ((((int)(c.r * 255)) / divisor * numBins * numBins));
						//						Debug.Log ((((int)(c.r * 255)) / divisor * numBins * numBins) + (((int)(c.b * 255))/divisor * numBins) + (((int)(c.g * 255)) / divisor));
						unitFeatures [(((int)(c.r * 255)) / divisor * numBins * numBins) + (((int)(c.b * 255))/divisor * numBins) + (((int)(c.g * 255)) / divisor)] += increment;
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
		totalPositiveWeights = 0.0;
		totalNegativeWeights = 0.0;
		foreach (double d in weights) {
			if (d < 0) {
				totalNegativeWeights += -d;
			} else if (d > 0) {
				totalPositiveWeights += d;
			}
		}
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
		if (pixels.Length == unitHeight*unitWidth) {
			double[] unitFeatures = new double[numBins * numBins * numBins];
			int divisor = 256 / numBins;
			double increment = 1.0 / (unitWidth * unitHeight);

			//Assigns the pixel values for each of the colors in the pattern increasing green, then increasing blue, then increasing red
			for (int i = 0; i < pixels.Length; i++) {
				Color c = pixels [i];
				unitFeatures [(((int)(c.r * 255)) / divisor * numBins * numBins) + (((int)(c.b * 255))/divisor * numBins) + (((int)(c.g * 255)) / divisor)] += increment;
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
		Color[] pixels = targetTex.GetPixels ();
		double[] unitFeatures = new double[numBins * numBins * numBins];
		int divisor = 256 / numBins;
		double increment = 1.0;

		//Assigns the pixel values for each of the colors in the pattern increasing green, then increasing blue, then increasing red
		for (int i = 0; i < pixels.Length; i++) {
			Color c = pixels [i];
			unitFeatures [(((int)(c.r * 255)) / divisor * numBins * numBins) + (((int)(c.b * 255))/divisor * numBins) + (((int)(c.g * 255)) / divisor)] += increment;
		}
		int weightIndex = 0;
		double weightValue = unitFeatures[0];
		if (weightValue < 0) {
			weightValue = 0.0;
		}

		double singlePixelValue = 1.0;
		for (int x = 0; x < newTex.width; x++) {
			for (int y = 0; y < newTex.height; y++) {
				while (weightValue < singlePixelValue) {
					weightIndex++;
					if (weightIndex >= weights.Length) {
						weightIndex = weights.Length-1;
						weightValue += 1;
					}
					if (unitFeatures [weightIndex] > 0) {
						weightValue += unitFeatures [weightIndex];
					}
				}
				weightValue -= singlePixelValue;
				Color c = findHistogramColor(weightIndex);
				newTex.SetPixel (x, y, c);
			}
		}
		newTex.Apply ();
		return newTex;
	}

	public override Texture2D getAllyTexture(){
		double singlePixelValue = totalPositiveWeights / (unitWidth * unitHeight);
		Texture2D newTex = new Texture2D (unitWidth, unitHeight);
		int weightIndex = 0;
		double weightValue = weights[0];
		if (weightValue < 0) {
			weightValue = 0.0;
		}
		for (int x = 0; x < newTex.width; x++) {
			for (int y = 0; y < newTex.height; y++) {
				while (weightValue < singlePixelValue) {
					weightIndex++;
					if (weightIndex >= weights.Length) {
						weightIndex = weights.Length-1;
						weightValue += 1;
					}
					if (weights [weightIndex] > 0) {
						weightValue += weights [weightIndex];
					}
				}
				weightValue -= singlePixelValue;
				Color c = findHistogramColor(weightIndex);
				newTex.SetPixel (x, y, c);
			}
		}
		newTex.Apply ();
		return newTex;
	}

	public override Texture2D getEnemyTexture(){
		double singlePixelValue = totalNegativeWeights / (unitWidth * unitHeight);
		Texture2D newTex = new Texture2D (unitWidth, unitHeight);
		int weightIndex = 0;
		double weightValue = -1*weights[0];
		if (weightValue < 0) {
			weightValue = 0.0;
		}
		for (int x = 0; x < newTex.width; x++) {
			for (int y = 0; y < newTex.height; y++) {
				while (weightValue < singlePixelValue) {
					weightIndex++;
					if (weightIndex >= weights.Length) {
						weightIndex = weights.Length-1;
						weightValue += 1;
					}
					if (weights [weightIndex] < 0) {
						weightValue -= weights [weightIndex];
					}
				}
				weightValue -= singlePixelValue;
				Color c = findHistogramColor(weightIndex);
				newTex.SetPixel (x, y, c);
			}
		}
		newTex.Apply ();
		return newTex;
	}

	protected Color findHistogramColor(int weightIndex){
		float green =(float) ((double)(weightIndex % numBins)+.5)/numBins;
		float blue =(float) ((double)((weightIndex / numBins)%numBins)+.5)/numBins;
		float red =(float) ((double)((weightIndex / (numBins*numBins))%numBins)+.5)/numBins;
		return new Color (red, green, blue);
	}

	public override NeuralNode Clone(){
		ColorHistNode res = (ColorHistNode) NeuralNode.create (NodeType.COLORHIST);
		res.SetWeights (actualWeights, this.b);
		res.SetNonzeroIndices ();
		res.totalNegativeWeights = this.totalNegativeWeights;
		res.totalPositiveWeights = this.totalPositiveWeights;
		return res;
	}
}
