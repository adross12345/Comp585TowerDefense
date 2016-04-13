using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MoneyManager : MonoBehaviour {

    public Text amountMoney;
    private CastleHealth money;

	// Use this for initialization
	void Start () {
        money = GameObject.Find("Castle").GetComponent<CastleHealth>();
        amountMoney.text = "$" + money.curMoney;
    }
	
	// Update is called once per frame
	void Update () {
        money = GameObject.Find("Castle").GetComponent<CastleHealth>();
        amountMoney.text = "$" + money.curMoney;
    }
}
