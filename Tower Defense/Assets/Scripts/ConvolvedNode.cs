//Not usable. Too slow.

//using UnityEngine;
//using System.Collections;
//using System.Collections.Generic;
//
//public class ConvolvedNode : NeuralNode {
//	private NeuralNode[] subNodes;
//	private int divisions=4;
//	private NodeType nodeType = NodeType.COLORHIST;
//	private int totalWidth;
//	private int totalHeight;
//
//	public override void LearnUnits(){
//		int divSq = divisions * divisions;
//		subNodes = new NeuralNode[divSq];
//		for (int i = 0; i < divSq; i++) {
//			subNodes [i] = NeuralNode.create (nodeType);
//		}
//		//I should probably make this a float array. 
//		//I'm not sure what kind of memory restrictions we're looking at.
//		//This list of arrays is going to be reclaimed after the method finishes, as is the trainingSet
//		List<double[]> features = new List<double[]> ();
//		int featureLength = -1;
//		//Gets the relevant data out of the training set
//		foreach(PhantomUnit u in trainingSet){
//			Texture tex = u.tex;
//			if (tex is Texture2D) {
//				Texture2D tex2D = (Texture2D)tex;
//				if (featureLength == -1) {
//					featureLength = divSq*2;
//					unitWidth = (int) Mathf.Ceil((float) tex2D.width/divisions);
//					unitHeight = (int) Mathf.Ceil((float) tex2D.height/divisions);
//					totalWidth = tex2D.width;
//					totalHeight = tex2D.height;
//				}
//				int nodeNum = 0;
//				for (int x = 0; x < divisions; x++) {
//					for (int y = 0; y < divisions; y++) {
//						if (((x + 1) * unitWidth < tex2D.width) && ((y + 1) * unitWidth < tex2D.width)) {
//							subNodes [nodeNum].AddToTrainingSet (tex2D.GetPixels (x * unitWidth, y * unitHeight, unitWidth, unitHeight, 0), u.identity, unitWidth, unitHeight);
//						} else {
//							int xOffset = x * unitWidth;
//							int yOffset = y * unitWidth;
//							if ((x + 1) * unitWidth > tex2D.width) {
//								xOffset = tex2D.width - unitWidth;
//							}
//							if ((y + 1) * unitHeight > tex2D.height) {
//								yOffset = tex2D.height - unitHeight;
//							}
//							subNodes [nodeNum].AddToTrainingSet (tex2D.GetPixels (xOffset, yOffset, unitWidth, unitHeight, 0), u.identity, unitWidth, unitHeight);
//						}
//						nodeNum++;
//					}
//				}
//			} else if (tex is RenderTexture) {
//				RenderTexture texRend = (RenderTexture)tex;
//				Debug.Log ("This is a RenderTexture");
//				//I don't have the code to get the pixels for this yet
//			} else {
//				Debug.Log ("I'm not sure what Texture this is or what to do with it.");
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
//		double[] res = new double[subNodes.Length*2];
//		for (int i = 0; i < subNodes.Length; i++) {
//			double featureValAlly = 0;
//			double featureValEnemy = 0;
//			for (int x = 0; x + unitWidth < tex.width; x++) {
//				for(int y = 0; y + unitHeight < tex.height; y++){
//					double z = subNodes [i].calculateZ (tex.GetPixels (x, y, unitWidth, unitHeight));
//					if (z < 0) {
//						featureValEnemy += z;
//					} else {
//						featureValAlly += z;
//					}
//				}
//			}
//			res [2*i] = featureValAlly;
//			res [2 * i + 1] = featureValEnemy;
//		}
//		return res;
//	}//GetNodeFeatures
//
//	public double[] GetNodeFeatures(Color[] pixels, int width, int height){
//		double[] res = new double[subNodes.Length];
//		for (int i = 0; i < subNodes.Length; i++) {
//			double featureValAlly = 0;
//			double featureValEnemy = 0;
//			for (int x = 0; x + unitWidth < width; x++) {
//				for (int y = 0; y + unitHeight < height; y++) {
//					double z = subNodes [i].calculateZ (pixels);
//					if (z < 0) {
//						featureValEnemy += z;
//					} else {
//						featureValAlly += z;
//					}
//				}
//			}
//			res [2 * i] = featureValAlly;
//			res [2 * i + 1] = featureValEnemy;
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
//			//This is really IMPORTANT.
//			//The last spot in unit features is reserved for the unit identity (0 for Enemy, 1 for Ally)
//			//Don't iterate over unitFeatures or its full length.
//			double[] unitFeatures = GetNodeFeatures(tex2D);
//			res = calculateZ (unitFeatures);
//		}
//		return res;
//	}
//
//	public override double calculateZ(Color[] pixels){
//		double res = 0;
//		double[] unitFeatures = GetNodeFeatures(pixels,totalWidth, totalHeight);
//		res = calculateZ (unitFeatures);
//		return res;
//	}
//
//	public override Texture2D getAllyTexture(){
//		Texture2D newTex = new Texture2D (totalWidth, totalHeight);
//		int nodeNum = 0;
//		for (int xDiv = 0; xDiv < divisions; xDiv++) {
//			for (int yDiv = 0; yDiv < divisions; yDiv++) {
//				Texture2D subTex = subNodes[nodeNum].getAllyTexture();
//				int xOffset = 0;
//				int yOffset = 0;
//				for (int x = unitWidth-1; x-xOffset >= 0; x--) {
//					for (int y = unitHeight-1; y-yOffset >= 0; y--) {
//						if (xDiv * unitWidth + x + xOffset >= totalWidth) {
//							xOffset = xDiv * unitWidth + x - totalWidth;
//						}
//						if (yDiv * unitHeight + x + yOffset >= totalHeight) {
//							yOffset = yDiv * unitHeight + y - totalHeight;
//						}
//						Color c = subTex.GetPixel(x,y);
//						newTex.SetPixel (xDiv*unitWidth+x+xOffset, yDiv*unitHeight+y+yOffset, c);
//					}
//				}
//				nodeNum++;
//			}
//		}
//		newTex.Apply ();
//		return newTex;
//	}
//
//	public override Texture2D getEnemyTexture(){
//		Texture2D newTex = new Texture2D (totalWidth, totalHeight);
//		int nodeNum = 0;
//		for (int xDiv = 0; xDiv < divisions; xDiv++) {
//			for (int yDiv = 0; yDiv < divisions; yDiv++) {
//				Texture2D subTex = subNodes[nodeNum].getEnemyTexture();
//				int xOffset = 0;
//				int yOffset = 0;
//				for (int x = unitWidth-1; x-xOffset >= 0; x--) {
//					for (int y = unitHeight-1; y-yOffset >= 0; y--) {
//						if (xDiv * unitWidth + x + xOffset >= totalWidth) {
//							xOffset = xDiv * unitWidth + x - totalWidth;
//						}
//						if (yDiv * unitHeight + x + yOffset >= totalHeight) {
//							yOffset = yDiv * unitHeight + y - totalHeight;
//						}
//						Color c = subTex.GetPixel(x,y);
//						newTex.SetPixel (xDiv*unitWidth+x+xOffset, yDiv*unitHeight+y+yOffset, c);
//					}
//				}
//				nodeNum++;
//			}
//		}
//		newTex.Apply ();
//		return newTex;
//	}
//
//	public override NeuralNode Clone(){
//		throw new UnityException ("Method not implemented");
//		return null;
//	}
//}
