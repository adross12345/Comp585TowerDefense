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
    private Sprite allySprite;
    private Sprite enemySprite;
    private Sprite targetSprite;
    private string zvalue;
    private string bvalue;

    // Use this for initialization
    void Start() {
        //ally.sprite = UnityEditor.AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/Background.psd");
        //enemy.sprite = UnityEditor.AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/Background.psd");
        //target.sprite = UnityEditor.AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/Background.psd");
        ally.sprite = UnityEditor.AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/UIMask.psd");
        enemy.sprite = UnityEditor.AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/UIMask.psd");
        target.sprite = UnityEditor.AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/UIMask.psd");
    }

    // Update is called once per frame
    void Update() {
        if (allySprite != null && enemySprite != null && targetSprite != null && zvalue != null && bvalue != null) {
            ally.sprite = allySprite;
            enemy.sprite = enemySprite;
            target.sprite = targetSprite;
            z.text = "z = " + zvalue;
            b.text = "b = " + bvalue;
        } else {
            //ally.sprite = UnityEditor.AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/Background.psd");
            //enemy.sprite = UnityEditor.AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/Background.psd");
            //target.sprite = UnityEditor.AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/Background.psd");
            ally.sprite = UnityEditor.AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/UIMask.psd");
            enemy.sprite = UnityEditor.AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/UIMask.psd");
            target.sprite = UnityEditor.AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/UIMask.psd");
            z.text = "z = ";
            b.text = "b = ";
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

    public void setZ(string s) {
        zvalue = s;
    }

    public void setB(string s) {
        bvalue = s;
    }

    public void resetAll() {
        allySprite = null;
        enemySprite = null;
        targetSprite = null;
        zvalue = null;
        bvalue = null;
}
}