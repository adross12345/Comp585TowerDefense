using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class InfoManager : MonoBehaviour {
    public Text towerName;
    public Text towerSellPrice;
    public Text enemies;
    public Text allies;
    public Image towerImage;
    private string tname;
    private string price;
    private Sprite timage;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	    if (tname != null) {
            towerName.text = tname;
        } else {
            towerName.text = "TowerName";
        }

        if (price != null) {
            towerSellPrice.text = price;
        } else {
            towerSellPrice.text = "";
        }

        if (timage != null) {
            towerImage.sprite = timage;
        } else {
            towerImage.sprite = Resources.Load<Sprite>("Sprites/black");
        }
	}

    public void setTowerName(string s) {
        this.tname = s;
    }

    public void setTowerImage(Sprite s) {
        this.timage = s;
    }

    public void setTowerPrice(string s) {
        this.price = s;
    }

    public void resetAll() {
        tname = null;
        price = null;
        timage = null;
    }
}
