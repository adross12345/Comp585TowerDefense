using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {

	public float mySpeed = 10;
	public float myRange = 10;

	private float distance;

	void Update(){
		transform.Translate(Vector3.forward * Time.deltaTime * mySpeed);
		distance += Time.deltaTime * mySpeed;
		if (distance >= myRange) {
			Destroy (gameObject);
		}
	}

}
