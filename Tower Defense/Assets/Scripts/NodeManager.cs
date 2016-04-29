using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class NodeManager : MonoBehaviour
{
    public Text z;
    public Text b;
    public Image ally;
    public Image enemy;
    public Image target;
	public Image features;
	public Text enemiesInRange;
	public Text alliesInRange;
    private Sprite allySprite;
    private Sprite enemySprite;
    private Sprite targetSprite;
	private Sprite featureSprite;
    private double zvalue;
    private double bvalue;
	public int numEneInRange;
	public int numAllyInRange;
	public GameObject allyBackground;
	public GameObject enemyBackground;

    // Use this for initialization
    void Start() {
        //ally.sprite = UnityEditor.AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/Background.psd");
        //enemy.sprite = UnityEditor.AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/Background.psd");
        //target.sprite = UnityEditor.AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/Background.psd");
        ally.sprite = /*UnityEditor.AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/UIMask.psd");*/ Resources.Load<Sprite>("Sprites/gray");
        enemy.sprite = /*UnityEditor.AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/UIMask.psd");*/ Resources.Load<Sprite>("Sprites/gray");
        target.sprite = /*UnityEditor.AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/UIMask.psd");*/ Resources.Load<Sprite>("Sprites/gray");
        allyBackground.SetActive (false);
		enemyBackground.SetActive (false);
    }

    // Update is called once per frame
    void Update() {
        if (allySprite != null && enemySprite != null && targetSprite != null && zvalue != 9001 && bvalue != 9001) {
            ally.sprite = allySprite;
            enemy.sprite = enemySprite;
            target.sprite = targetSprite;
			features.sprite = featureSprite;
			z.text = "z = " + zvalue.ToString("0.####");
			b.text = "b = " + bvalue.ToString("0.####");
			enemiesInRange.text = "" + numEneInRange;
			alliesInRange.text = "" + numAllyInRange;
			if (zvalue > 0) {
				allyBackground.SetActive (true);
				enemyBackground.SetActive (false);
			} else if (zvalue < 0) {
				allyBackground.SetActive (false);
				enemyBackground.SetActive (true);
			} else {
				allyBackground.SetActive (false);
				enemyBackground.SetActive (false);
			}
        } else {
            //ally.sprite = UnityEditor.AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/Background.psd");
            //enemy.sprite = UnityEditor.AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/Background.psd");
            //target.sprite = UnityEditor.AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/Background.psd");
            ally.sprite = /*UnityEditor.AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/UIMask.psd");*/ Resources.Load<Sprite>("Sprites/gray");
            enemy.sprite = /*UnityEditor.AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/UIMask.psd");*/ Resources.Load<Sprite>("Sprites/gray");
            target.sprite = /*UnityEditor.AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/UIMask.psd");*/ Resources.Load<Sprite>("Sprites/gray");
            features.sprite = /*UnityEditor.AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/UIMask.psd");*/ Resources.Load<Sprite>("Sprites/gray");
            z.text = "z = ";
            b.text = "b = ";
			allyBackground.SetActive (false);
			enemyBackground.SetActive (false);
        }
    }

    public void setAllySprite(Sprite s) {
        allySprite = s;
    }

    public void setEnemySprite(Sprite s) {
        enemySprite = s;
    }

    public void setTargetSprite(Sprite s) {
        targetSprite = s;
    }

    public void setZ(double d) {
        zvalue = d;
    }

    public void setB(double d) {
        bvalue = d;
    }

	public void setFeatureSprite(Sprite s){
		featureSprite = s;
	}

    public void resetAll() {
        allySprite = null;
        enemySprite = null;
        targetSprite = null;
        zvalue = 9001;
        bvalue = 9001;
}
}