//Not usable. Logistics were bad.

//using UnityEngine;
//using System.Collections;
//using System.Collections.Generic;
//
//public class CombinationNode : NeuralNode {
//	private List<NeuralNode> subNodes;
//
//	public override void LearnUnits(){
//		//I should probably make this a float array. 
//		//I'm not sure what kind of memory restrictions we're looking at.
//		//This list of arrays is going to be reclaimed after the method finishes, as is the trainingSet
//		List<double[]> features = new List<double[]> ();
//		int featureLength = subNodes.Count;
//		unitWidth = -1;
//		unitHeight = -1;
//		//Gets the relevant data out of the training set
//		foreach(PhantomUnit u in trainingSet){
//			if (unitWidth < 0 || unitHeight < 0) {
//				Texture2D tex2D = (Texture2D)u.tex;
//				unitWidth = tex2D.width;
//				unitHeight = tex2D.height;
//			}
//			foreach (NeuralNode node in subNodes) {
//				node.AddToTrainingSet (u);
//			}
//		}//for
//
//		foreach (NeuralNode node in subNodes) {
//			node.LearnUnits();
//		}
//
//		foreach (PhantomUnit u in trainingSet) {
//			Texture tex = u.tex;
//			if (tex is Texture2D) {
//				Texture2D tex2D = (Texture2D)tex;
//				double[] fs = new double[featureLength + 1];
//				double[] nodeFs = GetNodeFeatures (tex2D);
//				for (int i = 0; i < featureLength; i++) {
//					fs [i] = nodeFs [i];
//				}
//				fs[featureLength] = u.identity;
//				features.Add(fs);
//			} else {
//				Debug.Log ("I'm not sure what Texture this is or what to do with it.");
//			}
//		}
//
//
//		weights = new double[featureLength];
//		b = 0;
//
//		for (int it = 0; it < iters; it++) {
//			int misses = 0;
//			//Shuffles the feature list 
//			NeuralNode.Shuffle (features);
//			foreach(double[] fs in features){
//				int trueIdentity = (int) Mathf.Round((float)fs [featureLength]);
//				int f = (int) Unit.ALLY_IDENTITY;
//				if (calculateZ (fs) < 0) {
//					f = (int) Unit.ENEMY_IDENTITY;
//				}
//				misses += Mathf.Abs (f - trueIdentity);
//				for (int i = 0; i < featureLength; i++) {
//					weights [i] = weights [i] + learningRate * fs[i] * (trueIdentity - f);
//				}
//				b = b + learningRate * (trueIdentity - f);
//			}//for each of the feature arrays
//			if (misses == 0)
//				break;
//		}//for iterations
//		actualWeights = new double[weights.Length];
//		System.Array.Copy (weights, actualWeights, weights.Length);
//		actualB = b;
//		SetNonzeroIndices ();
//		//Notice that I am clearing out the training set here.
//		//Keeping the references is not necessary and will just take up space.
//		//Being able to get rid of them is sort of the point of making a model.
//		foreach(PhantomUnit u in trainingSet){
//			Destroy (u);
//		}
//		trainingSet.Clear();
//	}
//
//	public double[] GetNodeFeatures(Texture2D tex){
//		double[] res = new double[subNodes.Count];
//		for (int i = 0; i < subNodes.Count; i++) {
//			double featureVal = 0;
//			for (int x = 0; x + unitWidth < tex.width; x++) {
//				for(int y = 0; y + unitHeight < tex.height; y++){
//					double z = subNodes [i].calculateZ (tex.GetPixels (x, y, unitWidth, unitHeight));
//					if (z < 0) {
//						z = 0;
//					}
//					featureVal += z;
//				}
//			}
//			res [i] = featureVal;
//		}
//		return res;
//	}//GetNodeFeatures
//
//	public double[] GetNodeFeatures(Color[] pixels, int width, int height){
//		double[] res = new double[subNodes.Count];
//		for (int i = 0; i < subNodes.Count; i++) {
//			double featureVal = 0;
//			for (int x = 0; x + unitWidth < width; x++) {
//				for(int y = 0; y + unitHeight < height; y++){
//					double z = subNodes [i].calculateZ (pixels);
//					if (z < 0) {
//						z = 0;
//					}
//					featureVal += z;
//				}
//			}
//			res [i] = featureVal;
//		}
//		return res;
//	}//GetNodeFeatures
//
//	public override double calculateZ(double[] features){
//		double z = 0;
//		for (int i = 0; i < weights.Length; i++) {
//			z += weights [i] * features [i];
//		}
//		z += b;
//		return z;
//	}
//
//	public override double calculateZ(Unit unit){
//		double res = 0;
//		MeshRenderer mr = unit.GetComponent<MeshRenderer> ();
//		Texture tex = mr.material.mainTexture;
//		if (tex is Texture2D) {
//			Texture2D tex2D = (Texture2D)tex;
//			double[] unitFeatures = GetNodeFeatures(tex2D);
//			res = calculateZ (unitFeatures);
//		}
//		return res;
//	}
//
//	public override double calculateZ(Color[] pixels){
//		double res = 0;
//		double[] unitFeatures = GetNodeFeatures(pixels,unitWidth, unitHeight);
//		res = calculateZ (unitFeatures);
//		return res;
//	}
//
//	public override Texture2D getAllyTexture(){
//		Texture2D newTex = new Texture2D (unitWidth, unitHeight);
//		int udDivs = (int) Mathf.Ceil(Mathf.Sqrt (subNodes.Count));
//		int lrDivs = (int) Mathf.Ceil((float) subNodes.Count/udDivs);
//		int nodeNum = 0;
//		int lrSpace = unitWidth/lrDivs;
//		int udSpace = unitHeight/udDivs;
//		for (int xDiv = 0; xDiv < udDivs; xDiv++) {
//			for (int yDiv = 0; yDiv < lrDivs; yDiv++) {
//				if (nodeNum >= subNodes.Count) {
//					break;
//				}
//				Texture2D subTex = subNodes[nodeNum].getAllyTexture();
//				double lrRatio = (double)subTex.width/lrSpace;
//				double udRatio = (double)subTex.height/udSpace;
//				double lrX = 0;
//				double udY = 0;
//				for (int x = 0; x < lrSpace; x++) {
//					for (int y = 0; y < udSpace; y++) {
//						Color c = subTex.GetPixel((int)lrX,(int)udY);
//						newTex.SetPixel (xDiv*lrSpace+x, yDiv*udSpace+y, c);
//						udY += udRatio;
//					}
//					lrX += lrRatio;
//				}
//				nodeNum++;
//			}
//		}
//		newTex.Apply ();
//		return newTex;
//	}
//
//	public override Texture2D getEnemyTexture(){
//		Texture2D newTex = new Texture2D (unitWidth, unitHeight);
//		int udDivs = (int) Mathf.Ceil(Mathf.Sqrt (subNodes.Count));
//		int lrDivs = (int) Mathf.Ceil((float) subNodes.Count/udDivs);
//		int nodeNum = 0;
//		int lrSpace = unitWidth/lrDivs;
//		int udSpace = unitHeight/udDivs;
//		for (int xDiv = 0; xDiv < udDivs; xDiv++) {
//			for (int yDiv = 0; yDiv < lrDivs; yDiv++) {
//				if (nodeNum >= subNodes.Count) {
//					break;
//				}
//				Texture2D subTex = subNodes[nodeNum].getEnemyTexture();
//				double lrRatio = (double)subTex.width/lrSpace;
//				double udRatio = (double)subTex.height/udSpace;
//				double lrX = 0;
//				double udY = 0;
//				for (int x = 0; x < lrSpace; x++) {
//					for (int y = 0; y < udSpace; y++) {
//						Color c = subTex.GetPixel((int)lrX,(int)udY);
//						newTex.SetPixel (xDiv*lrSpace+x, yDiv*udSpace+y, c);
//						udY += udRatio;
//					}
//					lrX += lrRatio;
//				}
//				nodeNum++;
//			}
//		}
//		newTex.Apply ();
//		return newTex;
//	}
//
//	public void AddSubNode(NeuralNode node){
//		if (subNodes == null) {
//			subNodes = new List<NeuralNode> ();
//		}
//		subNodes.Add (node);
//	}
//
//	public override NeuralNode Clone(){
//		throw new UnityException ("Method not implemented");
//		return null;
//	}
//}
