using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;


public class ShowTowerPopup : EditorWindow {

	private static UnitGenerator uGen;
	private Vector3 farOff = new Vector3 (-500, -500, -500);
	string myName = "";
	string warning = "Your tower has no training data";
	bool isLearned;
	List<bool> learnedUnits = new List<bool>();
	float noiseScale = 0.0f;
	public string[] options = new string[] {"Grayscale", "Full Color", "Color Histogram"};
	public int nodeType = 0;
	NeuralNode.NodeType neuralNodeType;

	bool isLearned1;
	bool isLearned2;
	bool isLearned3;
	bool isLearned4;
	bool isLearned5;
	bool isLearned6;
	bool isLearned7;
	bool isLearned8;
	bool isLearned9;
	bool isLearned10;

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

	bool isTrainingEmpty = true;

	public static string towerName = "";
	public Transform trainable;
	public Texture2D towerLabel;
	public static GameObject tower = null;
	private static CannonFire cFire;

	[MenuItem("Example/ShowPopup Example")]
	public static void Init(GameObject tower) {
		ShowTowerPopup window = ScriptableObject.CreateInstance<ShowTowerPopup> ();
		window.position = new Rect (Screen.width / 2 + 100, Screen.height / 2 - 50, 300, 300);

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
				break;
			case 1: 
				// full color
				neuralNodeType = NeuralNode.NodeType.FULLCOLOR;
				break;
			case 2:
				// color histogram
				neuralNodeType = NeuralNode.NodeType.COLORHIST;
				break;
			default:
				// grayscale
				neuralNodeType = NeuralNode.NodeType.GRAYSCALE;
				break;
		}
	}


	void OnGUI() {

		// Set up interface
		// Still need to add pictures
		noiseScale = EditorGUILayout.Slider ("Noise", noiseScale, 0, 1);
		nodeType = EditorGUILayout.Popup(nodeType, options);

		EditorGUILayout.BeginHorizontal();
		isLearned1 = EditorGUILayout.Toggle ("Unit 1 (Ally1)", isLearned1);
		EditorGUI.BeginDisabledGroup (!isLearned1);
		quantity1 = EditorGUILayout.IntField (quantity1);
		EditorGUI.EndDisabledGroup();
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.BeginHorizontal();
		isLearned2 = EditorGUILayout.Toggle ("Unit 2 (Ally2)", isLearned2);
		EditorGUI.BeginDisabledGroup (!isLearned2);
		quantity2 = EditorGUILayout.IntField (quantity2);
		EditorGUI.EndDisabledGroup();
		EditorGUILayout.EndHorizontal ();

		EditorGUILayout.BeginHorizontal();
		isLearned3 = EditorGUILayout.Toggle ("Unit 3 (Enemy1)", isLearned3);
		EditorGUI.BeginDisabledGroup (!isLearned3);
		quantity3 = EditorGUILayout.IntField (quantity3);
		EditorGUI.EndDisabledGroup();
		EditorGUILayout.EndHorizontal ();

		EditorGUILayout.BeginHorizontal();
		isLearned4 = EditorGUILayout.Toggle ("Unit 4 (Enemy2)", isLearned4);
		EditorGUI.BeginDisabledGroup (!isLearned4);
		quantity4 = EditorGUILayout.IntField (quantity4);
		EditorGUI.EndDisabledGroup();
		EditorGUILayout.EndHorizontal ();

		EditorGUILayout.BeginHorizontal();
		isLearned5 = EditorGUILayout.Toggle ("Unit 5 (EnemyCamo)", isLearned5);
		EditorGUI.BeginDisabledGroup (!isLearned5);
		quantity5 = EditorGUILayout.IntField (quantity5);
		EditorGUI.EndDisabledGroup();
		EditorGUILayout.EndHorizontal ();

		EditorGUILayout.BeginHorizontal();
		isLearned6 = EditorGUILayout.Toggle ("Unit 6 (EnemyHydra)", isLearned6);
		EditorGUI.BeginDisabledGroup (!isLearned6);
		quantity6 = EditorGUILayout.IntField (quantity6);
		EditorGUI.EndDisabledGroup();
		EditorGUILayout.EndHorizontal ();

		EditorGUILayout.BeginHorizontal();
		isLearned7 = EditorGUILayout.Toggle ("Unit 7 (EnemySnake)", isLearned7);
		EditorGUI.BeginDisabledGroup (!isLearned7);
		quantity7 = EditorGUILayout.IntField (quantity7);
		EditorGUI.EndDisabledGroup();
		EditorGUILayout.EndHorizontal ();

		EditorGUILayout.BeginHorizontal();
		isLearned8 = EditorGUILayout.Toggle ("Unit 8 (EnemyAnimated)", isLearned8);
		EditorGUI.BeginDisabledGroup (!isLearned8);
		quantity8 = EditorGUILayout.IntField (quantity8);
		EditorGUI.EndDisabledGroup();
		EditorGUILayout.EndHorizontal ();

		EditorGUILayout.BeginHorizontal();
		isLearned9 = EditorGUILayout.Toggle ("Unit 9 (EnemyAISwapper)", isLearned9);
		EditorGUI.BeginDisabledGroup (!isLearned9);
		quantity9 = EditorGUILayout.IntField (quantity9);
		EditorGUI.EndDisabledGroup();
		EditorGUILayout.EndHorizontal ();

		EditorGUILayout.BeginHorizontal();
		isLearned10 = EditorGUILayout.Toggle ("Unit 10 (EnemyAIZeroer)", isLearned10);
		EditorGUI.BeginDisabledGroup (!isLearned10);
		quantity10 = EditorGUILayout.IntField (quantity10);
		EditorGUI.EndDisabledGroup();
		EditorGUILayout.EndHorizontal ();

		// Start training!
		InstantiatePrimitive ();
		NeuralNode node = NeuralNode.create (neuralNodeType);
		cFire.node = node;
		GUILayout.Space(45);
		if (GUILayout.Button ("Train!")) {

			for (int i = 0; i < quantity1; i++) {
				Unit unit = uGen.MakeUnit (false, 0, farOff, noiseScale, true);
				node.AddToTrainingSet (unit, isLearned1);
				unit.DestroyMe ();
				isTrainingEmpty = false;
			}

			for (int i = 0; i < quantity2; i++) {
				Unit unit = uGen.MakeUnit (false, 1, farOff, noiseScale, true);
				node.AddToTrainingSet (unit, isLearned2);
				unit.DestroyMe ();
				isTrainingEmpty = false;
			}

			for (int i = 0; i < quantity3; i++) {
				Unit unit = uGen.MakeUnit (true, 0, farOff, noiseScale, true);
				node.AddToTrainingSet (unit, isLearned3);
				unit.DestroyMe ();
				isTrainingEmpty = false;
			}

			for (int i = 0; i < quantity4; i++) {
				Unit unit = uGen.MakeUnit (true, 1, farOff, noiseScale, true);
				node.AddToTrainingSet (unit, isLearned4);
				unit.DestroyMe ();
				isTrainingEmpty = false;
			}

			for (int i = 0; i < quantity5; i++) {
				Unit unit = uGen.MakeUnit (true, 2, farOff, noiseScale, true);
				node.AddToTrainingSet (unit, isLearned5);
				unit.DestroyMe ();
				isTrainingEmpty = false;
			}

			for (int i = 0; i < quantity6; i++) {
				Unit unit = uGen.MakeUnit (true, 4, farOff, noiseScale, true);
				node.AddToTrainingSet (unit, isLearned6);
				unit.DestroyMe ();
				isTrainingEmpty = false;
			}

			for (int i = 0; i < quantity7; i++) {
				Unit unit = uGen.MakeUnit (true, 6, farOff, noiseScale, true);
				node.AddToTrainingSet (unit, isLearned6);
				unit.DestroyMe ();
				isTrainingEmpty = false;
			}

			for (int i = 0; i < quantity8; i++) {
				Unit unit = uGen.MakeUnit (true, 5, farOff, noiseScale, true);
				node.AddToTrainingSet (unit, isLearned8);
				unit.DestroyMe ();
				isTrainingEmpty = false;
			}

			for (int i = 0; i < quantity9; i++) {
				Unit unit = uGen.MakeUnit (true, 9, farOff, noiseScale, true);
				node.AddToTrainingSet (unit, isLearned9);
				unit.DestroyMe ();
				isTrainingEmpty = false;
			}

			for (int i = 0; i < quantity10; i++) {
				Unit unit = uGen.MakeUnit (true, 3, farOff, noiseScale, true);
				node.AddToTrainingSet (unit, isLearned10);
				unit.DestroyMe ();
				isTrainingEmpty = false;
			}

			if (!isTrainingEmpty) {
				node.LearnUnits ();
				this.Close ();
				Time.timeScale = 1;
			} else {
				// alert that no training is going to be done
				// default
				Debug.Log("Training set is empty!");
				warning = EditorGUILayout.TextField (warning);
				ShowNotification(new GUIContent(warning));
			}
				
		}

	}

}
