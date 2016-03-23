using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour {

    public Text amountHealth;
    private string testName = "###";
    private CastleHealth health;

	// Use this for initialization
	void Start () {
        health = GameObject.Find("Castle").GetComponent<CastleHealth>();
        amountHealth.text =  (int)(health.curHealth*100) + "/100";
	}
	
	// Update is called once per frame
	void Update () {
        health = GameObject.Find("Castle").GetComponent<CastleHealth>();
		amountHealth.text = (int)Mathf.Round((health.curHealth * 100)) + "/100";
    }
}
