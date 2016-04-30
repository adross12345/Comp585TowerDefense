using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

public class PopUpUI : MonoBehaviour {

	public readonly int TOTAL_ENEMY_UNITS = 14;
	public readonly int TOTAL_ALLY_UNITS = 10;
	public static int[] ENEMY_INDICES = { 0, 1, 10, 11, 5, 2, 4, 13, 3, 12, 8, 7, 6, 9 };
	public static int[] ALLY_INDICES = { 0, 1, 2, 3, 4, 6, 7, 9, 8, 5};


    public Canvas window;
    public Text trainingCost;
	private static CastleHealth castle;
	string noDataWarning = "Your tower has no training data";
	string priceWarning = "You do not have enough money!";
	int price = 0;

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
		castle = GameObject.Find("Castle").GetComponent<CastleHealth>();
        //GetComponent<Canvas> ().enabled = false;
        GameObject.Find("TrainingWindow").GetComponent<Canvas>().enabled = false;
		GameObject.Find ("TrainButton").GetComponent<Button> ()
			.onClick.AddListener (delegate {
				if(castle.canPurchase(price)) {
					train();
					GetComponent<Canvas> ().enabled = false;
				} else if(!castle.canPurchase(price)) {
					Debug.Log("Not enough money");
				} else if(getTotalUnits() == 0) {
					Debug.Log("No units to train!");
				}
			});
	}

    public void Init(GameObject tower) {
        // ShowTowerPopup window = ScriptableObject.CreateInstance<ShowTowerPopup>();
        // window.position = new Rect(Screen.width / 2 + 100, Screen.height / 2 - 50, 300, 300);
		// GetComponent<Canvas> ().enabled = true;

        castle = GameObject.Find("Castle").GetComponent<CastleHealth>();
        uGen = Camera.main.GetComponent<UnitGenerator>();
        cFire = tower.transform.FindChild("Turret").GetComponent<CannonFire>();
		
        // pause game
        previousTimeScale = Time.timeScale;
        Time.timeScale = 0;
    }

    // Update is called once per frame
    void Update () {
		calculateCost ();
		// setNoiseText (getNoise ());
		setPrice (price);
	}

	public void calculateCost() {
		InstantiatePrimitive ();
		if (getTotalUnits() <= 4)
		{
			price = (int)(10 * (1 - getNoise()) + nodeCost);
		}
		else {
			// $5 for every training unit greater than total of 10
			price = (int)(10 * (1 - getNoise()) + 5 * (getTotalUnits() - 4) 
				+ nodeCost);
		}
	}

	public int getTotalUnits() {
		int total = 0;
		for (int i = 1; i <= TOTAL_ALLY_UNITS; i++) {
			total += getQuantityOfAlly (i);
		}

		for (int i = 1; i <= TOTAL_ENEMY_UNITS; i++) {
			if (isEnemyTarget (i)) {
				total += getQuantityOfEnemy (i);
			}
		}

		return total;
	}

	public int getQuantityOfEnemy(int i) {
		int quantity;
		if (Int32.TryParse (GameObject.Find ("Enemy" + i + "InputField").GetComponent<InputField> ().text, out quantity)) {
			return quantity;
		}

		return 0;
	}

	public int getQuantityOfAlly(int i) {
		int quantity;
		if(Int32.TryParse(GameObject.Find ("Ally" + i + "InputField").GetComponent<InputField> ().text, out quantity)) {
			return quantity;
		}

		return 0;
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

	public int getPrice() {
		return price;
	}

	public void setPrice(int cost) {
		if (castle.canPurchase(cost)) {
			GameObject.Find ("Cost").GetComponent<Text> ().text = 
				"$" + cost;
			GameObject.Find ("Cost").GetComponent<Text> ().color = Color.white;
		} else {
			Debug.Log ("New price: " + price);
			GameObject.Find ("Cost").GetComponent<Text> ().text = 
				"$" + cost;
			GameObject.Find ("Cost").GetComponent<Text> ().color = Color.red;
		}
	}

	public void setNoiseText(float n) {
		GameObject.Find ("NoiseInputFieldText").GetComponent<Text> ().text =
			n.ToString ();
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

    public void sliderChange() {
        //GameObject.Find("NoiseInputField").GetComponent<InputField>().text = Convert.ToString(GameObject.Find("NoiseSlider").GetComponent<Slider>().value);
        GameObject.Find("NoiseInputField").GetComponent<InputField>().text =  Convert.ToString(Mathf.Round(GameObject.Find("NoiseSlider").GetComponent<Slider>().value * 100f) / 100f);
    }

    public void inputChange() {
        float quantity;
        if (float.TryParse(GameObject.Find("NoiseInputField").GetComponent<InputField>().text, out quantity)) {
            GameObject.Find("NoiseSlider").GetComponent<Slider>().value = quantity;
        }
    }

	public void train() {
		if (castle.canPurchase (getPrice())) {
			// Start training!
			NeuralNode node = NeuralNode.create(neuralNodeType);
			cFire.node = node;
			float noise = getNoise ();

			for (int i = 1; i <= TOTAL_ENEMY_UNITS; i++) {
				for(int j = 0; j < getQuantityOfEnemy(i); j++) {
					Unit unit = uGen.MakeUnit(true, ENEMY_INDICES[i-1], farOff, noise, true);
					if (unit is UnitAnimated) {
						UnitAnimated ua = (UnitAnimated)unit;
						Texture2D[] texes = ua.textures;
						int texIdx = j % texes.Length;
						ua.setTexture(texes[texIdx]);
					}

					node.AddToTrainingSet(unit, isEnemyTarget(i));
					unit.DestroyMe();
				}
			}

			for (int i = 1; i <= TOTAL_ALLY_UNITS; i++) { 
				for(int j = 0; j < getQuantityOfAlly(i); j++) {
					Unit unit = uGen.MakeUnit(false, ALLY_INDICES[i-1], farOff, noise, true);
					if (unit is UnitAnimated) {
						UnitAnimated ua = (UnitAnimated)unit;
						Texture2D[] texes = ua.textures;
						int texIdx = j % texes.Length;
						ua.setTexture(texes[texIdx]);
					}

					node.AddToTrainingSet(unit, isAllyTarget(i));
					unit.DestroyMe();
				}
			}

			node.LearnUnits();
			castle.makePurchase(getPrice());
			Time.timeScale = previousTimeScale;
		
			// Close window
			GetComponent<Canvas> ().enabled = false;
			Camera.main.GetComponent<BuildingPlacement> ().placeableBuildingOld.SetSelected (true);

		}
	}

}
