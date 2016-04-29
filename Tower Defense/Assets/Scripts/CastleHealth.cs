using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class CastleHealth : MonoBehaviour {

	public float maxHealth = 1.00f;
	public float curHealth = 0f;
	public GameObject healthBar;
    public int curMoney;
    public int startMoney = 510;

	[SerializeField]
	private Text healthText = null;



	// Use this for initialization
	void Start () 
	{
		curHealth = maxHealth;
        //Tests the Tower Health Bar
        //InvokeRepeating ("decreaseHealth", 1f, 1f);

        curMoney = startMoney;
	}



	//Decreases castle health when enemy reaches castle.
	void decreaseHealth(float damage)
	{
		curHealth -= damage / 100;
		float calcHealth = curHealth / maxHealth;
		setHealthBar (calcHealth);
		if (curHealth <= 0f) {
            Destroy (gameObject);
            //TODO End the game
            SceneManager.LoadScene("GameOver");
        }
	}

	public void OnTriggerEnter(Collider co) {
		if (co is BoxCollider) {
			Unit unit = co.gameObject.GetComponent<Unit> ();
			if (co.tag == "Enemy" && unit != null) {
				float damage = unit.getDamage ();
				Debug.Log (damage);
				decreaseHealth (damage);	
			} else if (co.tag == "Ally" && unit != null) {
                Debug.Log("Ally got to tower");
				float money = unit.getMoney ();
                //TODO Add money to bank here
                curMoney += (int)money; // 100 is placeholder, need to determine amount of money for each kind of ally
			}
			if (unit != null) {
				unit.DestroyMe ();
			}
		}
	}

	public void setHealthBar(float castleHealth)
	{
		healthBar.transform.localScale = new Vector3(curHealth, 
			healthBar.transform.localScale.y, healthBar.transform.localScale.z);
	}

    public bool canPurchase(int cost) {
        if (cost > curMoney) {
            return false;
        }

        return true;
    }

    public void makePurchase(int cost) {
        curMoney -= cost;
    }
}