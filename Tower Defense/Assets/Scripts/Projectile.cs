using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {

	private float mySpeed = 10;
	private float myRange = 10;
	private GameObject projectile;

	private float distance;

	void Start(){
		transform.Translate(Vector3.forward * Time.deltaTime * mySpeed);
		distance += Time.deltaTime * mySpeed;
		if (distance >= myRange) {
			Destroy (gameObject);
		}
	}
		

}
