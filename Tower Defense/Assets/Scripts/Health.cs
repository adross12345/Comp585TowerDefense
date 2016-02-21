using UnityEngine;
using System.Collections;

public class EnemyHealth : MonoBehaviour {

	// Use this for initialization
	void Start () {
		GetComponent<TextMesh>();
		}

	// Update is called once per frame
	void Update () {

		//Face the camera
		transform.forward = Camera.main.transform.forward;
	}

	// Return the current Health by counting the '-'
	/*public int current() {
		return tm.text.Length;
	}

		// Decrease the current Health by removing one '-'
	public void decrease() {
		if (current() > 1)
			tm.text = tm.text.Remove(tm.text.Length - 1);
		else
			Destroy(transform.parent.gameObject);
	}*/
}
