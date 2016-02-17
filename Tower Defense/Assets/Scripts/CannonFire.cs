using UnityEngine;
using System.Collections;

public class CannonFire : MonoBehaviour {


	public GameObject projectilePrefab;
	public float rotationSpeed = 5;


	void Update() {
		transform.Rotate(Vector3.up * Time.deltaTime * rotationSpeed, Space.World);
	}

	void OnTriggerEnter(Collider co) {
		// Was it a Monster? Then Shoot it
		if (co.GetComponent<Enemy>()) {
			GameObject g = (GameObject)Instantiate(projectilePrefab, transform.position, Quaternion.identity);
			g.GetComponent<Bullet>().target = co.transform;
		}
	}
}
