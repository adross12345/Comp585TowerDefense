using UnityEngine;
using System.Collections;

public class SplashRadius : MonoBehaviour {
	private float myDamage = 0f;
	private float timeOfInstantiation;
	public float timeToExist = 0.05f;

	// Use this for initialization
	void Awake () {
		Debug.Log ("Instantiated: " + timeToExist);
		timeOfInstantiation = Time.time;
	}

	protected void OnTriggerEnter(Collider other){	
		Unit u = other.gameObject.GetComponent<Unit> ();
		if(u!=null && other is BoxCollider){
			u.takeDamage (myDamage);
		}
	}

	public void SetMyDamage(float damage){
		myDamage = damage;
	}

	public void SetSplashRadius(float radius){
		SphereCollider sc = GetComponent<SphereCollider> ();
		Transform rangeIndicator = transform.Find ("Range");
		if (rangeIndicator) {
			rangeIndicator.localScale = new Vector3 (radius * 2, 0, radius * 2);
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (Time.time >= timeOfInstantiation + timeToExist) {
			Destroy (gameObject);
		}
	}
}
