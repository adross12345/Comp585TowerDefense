using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

public class PopUpUI : MonoBehaviour {

	public readonly int TOTAL_ENEMY_UNITS = 14;
	public readonly int TOTAL_ALLY_UNITS = 10;

    public Canvas window;
    public Text trainingCost;
	private static CastleHealth castle;
	string noDataWarning = "Your tower has no training data";
	string priceWarning = "You do not have enough money!";

    private static UnitGenerator uGen;
    private Vector3 farOff = new Vector3(-500, -500, -500);
	int nodeCost = 0;
	/*
    string myName = "";
    int price = 0;
    int nodeCost = 0;
    private static CastleHealth castle;
    List<bool> leaisTargetUnits = new List<bool>();
    float noiseScale = 1.0f;
    public string[] options = new string[] { "Grayscale", "Full Color", "Color Histogram" };
	*/

	NeuralNode.NodeType neuralNodeType;
    private static float previousTimeScale = 1f;

    public static GameObject tower = null;
    private static CannonFire cFire;

    // Use this for initialization
    void Start () {
		GetComponent<Canvas> ().enabled = false;
	}

    public void Init(GameObject tower) {
        // ShowTowerPopup window = ScriptableObject.CreateInstance<ShowTowerPopup>();
        // window.position = new Rect(Screen.width / 2 + 100, Screen.height / 2 - 50, 300, 300);
		// GetComponent<Canvas> ().enabled = true;

        castle = GameObject.Find("Castle").GetComponent<CastleHealth>();
        uGen = Camera.main.GetComponent<UnitGenerator>();
        cFire = tower.transform.FindChild("Turret").GetComponent<CannonFire>();
        // window.ShowPopup();

        // pause game
        previousTimeScale = Time.timeScale;
        Time.timeScale = 0;
    }

    // Update is called once per frame
    void Update () {
		Debug.Log (getNoise ());
		Debug.Log (getNodeType ());
		Debug.Log (getPrice ());

	}

	public int getQuantityOfEnemy(int i) {
		return int.Parse(GameObject.Find ("Enemy" + i + "InputField").GetComponent<InputField> ().text);
	}

	public int getQuantityOfAlly(int i) {
		return int.Parse(GameObject.Find ("Ally" + i + "InputField").GetComponent<InputField> ().text);
	}

	public bool isEnemyTarget(int i) {
		return GameObject.Find ("Enemy" + i + "Toggle").GetComponent<Toggle> ().isOn;
	}

	public bool isAllyTarget(int i) {
		return GameObject.Find ("Ally" + i + "Toggle").GetComponent<Toggle> ().isOn;
	}

	public float getNoise() {
		return GameObject.Find ("NoiseSlider").GetComponent<Slider> ().value;
	}

	public int getNodeType() {
		return GameObject.Find ("NodeDropdown").GetComponent<Dropdown> ().value;
	}

	public String getPrice() {
		return GameObject.Find ("Cost").GetComponent<Text> ().text;
	}
		
    void InstantiatePrimitive() {
        // define node types
		switch (getNodeType()) {
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


	/*
    public void getNoiseInput() {
        float value;
        string input = GameObject.Find("NoiseInputFieldText").GetComponent<Text>().text;
        if (float.TryParse(input, out value)) {
            noiseScale = Convert.ToSingle(input);
            Debug.Log("Noise is now " + noiseScale);
        }
        else {
            Debug.Log("Number in input field is not a number");
        }
    }
	*/

    /*void OnGUI() {

        // Still need to add pictures
        // Noise slider
        noiseScale = EditorGUILayout.Slider("Noise", noiseScale, 1, 0);

        // Node options
        nodeType = EditorGUILayout.Popup(nodeType, options);
        InstantiatePrimitive();

        // Unit target and quantity fields
        EditorGUILayout.BeginHorizontal();
        isTarget1 = EditorGUILayout.Toggle("Unit 1 (Ally1)", isTarget1);
        quantity1 = EditorGUILayout.IntField(quantity1);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        isTarget2 = EditorGUILayout.Toggle("Unit 2 (Ally2)", isTarget2);
        quantity2 = EditorGUILayout.IntField(quantity2);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        isTarget3 = EditorGUILayout.Toggle("Unit 3 (Enemy1)", isTarget3);
        quantity3 = EditorGUILayout.IntField(quantity3);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        isTarget4 = EditorGUILayout.Toggle("Unit 4 (Enemy2)", isTarget4);
        quantity4 = EditorGUILayout.IntField(quantity4);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        isTarget5 = EditorGUILayout.Toggle("Unit 5 (EnemyCamo)", isTarget5);
        quantity5 = EditorGUILayout.IntField(quantity5);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        isTarget6 = EditorGUILayout.Toggle("Unit 6 (EnemyHydra)", isTarget6);
        quantity6 = EditorGUILayout.IntField(quantity6);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        isTarget7 = EditorGUILayout.Toggle("Unit 7 (EnemySnake)", isTarget7);
        quantity7 = EditorGUILayout.IntField(quantity7);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        isTarget8 = EditorGUILayout.Toggle("Unit 8 (EnemyAnimated)", isTarget8);
        quantity8 = EditorGUILayout.IntField(quantity8);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        isTarget9 = EditorGUILayout.Toggle("Unit 9 (EnemyAISwapper)", isTarget9);
        quantity9 = EditorGUILayout.IntField(quantity9);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        isTarget10 = EditorGUILayout.Toggle("Unit 10 (EnemyAIZeroer)", isTarget10);
        quantity10 = EditorGUILayout.IntField(quantity10);
        EditorGUILayout.EndHorizontal();

        // Training cost field and control
        GUIStyle red = new GUIStyle(EditorStyles.label);
        GUIStyle black = new GUIStyle(EditorStyles.label);
        red.normal.textColor = Color.red;
        black.normal.textColor = Color.black;
        EditorGUILayout.LabelField("Cost of Training:", "$" + price.ToString(),
            castle.canPurchase(price) ? black : red);
        int sumQuant = quantity1 + quantity2 + quantity3 + quantity4 + quantity5
                       + quantity6 + quantity7 + quantity8 + quantity9 + quantity10;

        if (sumQuant <= 10)
        {
            price = (int)(10 * (1 - noiseScale) + nodeCost);
        }
        else {
            // $5 for every training unit greater than total of 10
            price = (int)(10 * (1 - noiseScale) + 5 * (sumQuant - 10) + nodeCost);
        }

        
		EditorGUILayout.LabelField("Time since start: ",
			((int)(200 - EditorApplication.timeSinceStartup)).ToString());
		Repaint ();
		

        // Train button
        GUILayout.Space(30);
        if (GUILayout.Button("Train!"))
        {

            

        }

    }*/


	public void train() {
		// castle.canPurchase (getPrice())
		if (castle.canPurchase (1000)) {
			// Start training!
			NeuralNode node = NeuralNode.create(neuralNodeType);
			cFire.node = node;

			for (int i = 1; i <= TOTAL_ENEMY_UNITS; i++) {
				for(int j = 0; j < getQuantityOfEnemy(i); j++) {
					Unit unit = uGen.MakeUnit(true, 0, farOff, getNoise(), true);
					if (unit is UnitAnimated) {
						UnitAnimated ua = (UnitAnimated)unit;
						Texture2D[] texes = ua.textures;
						int texIdx = i % texes.Length;
						ua.setTexture(texes[texIdx]);
					}

					node.AddToTrainingSet(unit, isEnemyTarget(i));
					unit.DestroyMe();
				}
			}

			for (int i = 1; i <= TOTAL_ALLY_UNITS; i++) { 
				for(int j = 0; j < getQuantityOfAlly(i); j++) {
					Unit unit = uGen.MakeUnit(false, 0, farOff, getNoise(), true);
					if (unit is UnitAnimated) {
						UnitAnimated ua = (UnitAnimated)unit;
						Texture2D[] texes = ua.textures;
						int texIdx = i % texes.Length;
						ua.setTexture(texes[texIdx]);
					}

					node.AddToTrainingSet(unit, isAllyTarget(i));
					unit.DestroyMe();
				}
			}

			node.LearnUnits();
			castle.makePurchase(1000);
			Time.timeScale = previousTimeScale;
		
			// Close window
			GetComponent<Canvas> ().enabled = false;

		}
	}

}
