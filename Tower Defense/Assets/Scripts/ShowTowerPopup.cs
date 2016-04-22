using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;


public class ShowTowerPopup : EditorWindow {

	private static UnitGenerator uGen;
	private Vector3 farOff = new Vector3 (-500, -500, -500);
	string myName = "";
	string noDataWarning = "Your tower has no training data";
	string priceWarning = "You do not have enough money!";
	int price = 0;
	int nodeCost = 0;
	bool isTarget;
	private static CastleHealth castle;
	List<bool> leaisTargetUnits = new List<bool>();
	float noiseScale = 1.0f;
	public string[] options = new string[] {"Grayscale", "Full Color", "Color Histogram"};
	public int nodeType = 0;
	NeuralNode.NodeType neuralNodeType;

	bool isTarget1;
	bool isTarget2;
	bool isTarget3;
	bool isTarget4;
	bool isTarget5;
	bool isTarget6;
	bool isTarget7;
	bool isTarget8;
	bool isTarget9;
	bool isTarget10;

	int quantity1;
	int quantity2;
	int quantity3;
	int quantity4;
	int quantity5;
	int quantity6;
	int quantity7;
	int quantity8;
	int quantity9;
	int quantity10;

	public static GameObject tower = null;
	private static CannonFire cFire;

	[MenuItem("Example/ShowPopup Example")]
	public static void Init(GameObject tower) {
		ShowTowerPopup window = ScriptableObject.CreateInstance<ShowTowerPopup> ();
		window.position = new Rect (Screen.width / 2 + 100, Screen.height / 2 - 50, 300, 300);

		castle = GameObject.Find ("Castle").GetComponent<CastleHealth> ();
		uGen = Camera.main.GetComponent<UnitGenerator>();
		cFire = tower.transform.FindChild("Turret").GetComponent<CannonFire>();
		window.ShowPopup ();

		// pause game
		Time.timeScale = 0;

	}

	void InstantiatePrimitive() {
		// define node types
		switch(nodeType) {
			case 0:  
				// grayscale
				neuralNodeType = NeuralNode.NodeType.GRAYSCALE;
				nodeCost = 0;
				break;
			case 1: 
				// full color
				neuralNodeType = NeuralNode.NodeType.FULLCOLOR;
				nodeCost = 10;
				break;
			case 2:
				// color histogram
				neuralNodeType = NeuralNode.NodeType.COLORHIST;
				nodeCost = 25;
				break;
			default:
				// grayscale
				neuralNodeType = NeuralNode.NodeType.GRAYSCALE;
				nodeCost = 0;
				break;
		}
	}


	void OnGUI() {

		// Still need to add pictures
		// Noise slider
		noiseScale = EditorGUILayout.Slider ("Noise", noiseScale, 1, 0);

		// Node options
		nodeType = EditorGUILayout.Popup(nodeType, options);
		InstantiatePrimitive ();

		// Unit target and quantity fields
		EditorGUILayout.BeginHorizontal();
		isTarget1 = EditorGUILayout.Toggle ("Unit 1 (Ally1)", isTarget1);
		quantity1 = EditorGUILayout.IntField (quantity1);
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.BeginHorizontal();
		isTarget2 = EditorGUILayout.Toggle ("Unit 2 (Ally2)", isTarget2);
		quantity2 = EditorGUILayout.IntField (quantity2);
		EditorGUILayout.EndHorizontal ();

		EditorGUILayout.BeginHorizontal();
		isTarget3 = EditorGUILayout.Toggle ("Unit 3 (Enemy1)", isTarget3);
		quantity3 = EditorGUILayout.IntField (quantity3);
		EditorGUILayout.EndHorizontal ();

		EditorGUILayout.BeginHorizontal();
		isTarget4 = EditorGUILayout.Toggle ("Unit 4 (Enemy2)", isTarget4);
		quantity4 = EditorGUILayout.IntField (quantity4);
		EditorGUILayout.EndHorizontal ();

		EditorGUILayout.BeginHorizontal();
		isTarget5 = EditorGUILayout.Toggle ("Unit 5 (EnemyCamo)", isTarget5);
		quantity5 = EditorGUILayout.IntField (quantity5);
		EditorGUILayout.EndHorizontal ();

		EditorGUILayout.BeginHorizontal();
		isTarget6 = EditorGUILayout.Toggle ("Unit 6 (EnemyHydra)", isTarget6);
		quantity6 = EditorGUILayout.IntField (quantity6);
		EditorGUILayout.EndHorizontal ();

		EditorGUILayout.BeginHorizontal();
		isTarget7 = EditorGUILayout.Toggle ("Unit 7 (EnemySnake)", isTarget7);
		quantity7 = EditorGUILayout.IntField (quantity7);
		EditorGUILayout.EndHorizontal ();

		EditorGUILayout.BeginHorizontal();
		isTarget8 = EditorGUILayout.Toggle ("Unit 8 (EnemyAnimated)", isTarget8);
		quantity8 = EditorGUILayout.IntField (quantity8);
		EditorGUILayout.EndHorizontal ();

		EditorGUILayout.BeginHorizontal();
		isTarget9 = EditorGUILayout.Toggle ("Unit 9 (EnemyAISwapper)", isTarget9);
		quantity9 = EditorGUILayout.IntField (quantity9);
		EditorGUILayout.EndHorizontal ();

		EditorGUILayout.BeginHorizontal();
		isTarget10 = EditorGUILayout.Toggle ("Unit 10 (EnemyAIZeroer)", isTarget10);
		quantity10 = EditorGUILayout.IntField (quantity10);
		EditorGUILayout.EndHorizontal ();

		// Training cost field and control
		GUIStyle red = new GUIStyle(EditorStyles.label);
		GUIStyle black = new GUIStyle(EditorStyles.label);
		red.normal.textColor = Color.red;
		black.normal.textColor = Color.black;
		EditorGUILayout.LabelField ("Cost of Training:", "$" + price.ToString(), 
			castle.canPurchase(price) ? black : red);
		int sumQuant = quantity1 + quantity2 + quantity3 + quantity4 + quantity5
		               + quantity6 + quantity7 + quantity8 + quantity9 + quantity10;

		if (sumQuant <= 10) {
			price = (int)(10 * (1 - noiseScale) + nodeCost);
		} else {
			// $5 for every training unit greater than total of 10
			price = (int)(10 * (1 - noiseScale) + 5 * (sumQuant - 10) + nodeCost);
		}

		/*
		EditorGUILayout.LabelField("Time since start: ",
			((int)(200 - EditorApplication.timeSinceStartup)).ToString());
		Repaint ();
		*/

		// Train button
		GUILayout.Space(30);
		if (GUILayout.Button ("Train!")) {

			if (sumQuant > 0 && castle.canPurchase(price)) {

				// Start training!
				NeuralNode node = NeuralNode.create (neuralNodeType);
				cFire.node = node;
				
				for (int i = 0; i < quantity1; i++) {
					Unit unit = uGen.MakeUnit (false, 0, farOff, noiseScale, true);
					node.AddToTrainingSet (unit, isTarget1);
					unit.DestroyMe ();
				}

				for (int i = 0; i < quantity2; i++) {
					Unit unit = uGen.MakeUnit (false, 1, farOff, noiseScale, true);
					node.AddToTrainingSet (unit, isTarget2);
					unit.DestroyMe ();
				}

				for (int i = 0; i < quantity3; i++) {
					Unit unit = uGen.MakeUnit (true, 0, farOff, noiseScale, true);
					node.AddToTrainingSet (unit, isTarget3);
					unit.DestroyMe ();
				}

				for (int i = 0; i < quantity4; i++) {
					Unit unit = uGen.MakeUnit (true, 1, farOff, noiseScale, true);
					node.AddToTrainingSet (unit, isTarget4);
					unit.DestroyMe ();
				}

				for (int i = 0; i < quantity5; i++) {
					Unit unit = uGen.MakeUnit (true, 2, farOff, noiseScale, true);
					node.AddToTrainingSet (unit, isTarget5);
					unit.DestroyMe ();
				}

				for (int i = 0; i < quantity6; i++) {
					Unit unit = uGen.MakeUnit (true, 4, farOff, noiseScale, true);
					node.AddToTrainingSet (unit, isTarget6);
					unit.DestroyMe ();
				}

				for (int i = 0; i < quantity7; i++) {
					Unit unit = uGen.MakeUnit (true, 6, farOff, noiseScale, true);
					node.AddToTrainingSet (unit, isTarget6);
					unit.DestroyMe ();
				}

				for (int i = 0; i < quantity8; i++) {
					Unit unit = uGen.MakeUnit (true, 5, farOff, noiseScale, true);
					node.AddToTrainingSet (unit, isTarget8);
					unit.DestroyMe ();
				}

				for (int i = 0; i < quantity9; i++) {
					Unit unit = uGen.MakeUnit (true, 9, farOff, noiseScale, true);
					node.AddToTrainingSet (unit, isTarget9);
					unit.DestroyMe ();
				}

				for (int i = 0; i < quantity10; i++) {
					Unit unit = uGen.MakeUnit (true, 3, farOff, noiseScale, true);
					node.AddToTrainingSet (unit, isTarget10);
					unit.DestroyMe ();
				}

				node.LearnUnits ();
				castle.makePurchase (price);
				this.Close ();
				Time.timeScale = 1;
			} else if (!castle.canPurchase(price)) {
				Debug.Log (price);
				priceWarning = EditorGUILayout.TextField (priceWarning);
				ShowNotification(new GUIContent(priceWarning));
			} else if (sumQuant == 0) {
				// alert that no training is going to be done
				Debug.Log("Training set is empty!");
				noDataWarning = EditorGUILayout.TextField (noDataWarning);
				ShowNotification(new GUIContent(noDataWarning));
			}
				
		}

	}

}
