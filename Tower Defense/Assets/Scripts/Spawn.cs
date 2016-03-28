using UnityEngine;
using System.Collections;

public class Spawn : MonoBehaviour {

	// The Monster that should be spawned
	public Unit monsterPrefab;
	public Unit allyPrefab;
	public static int numSpawned;



	// Spawn Delay (seconds)
	public float interval = 3;

	// Use this for initialization
	void Start() {
		numSpawned = 0;
		InvokeRepeating("SpawnNext", interval, interval);

	}

	void SpawnNext() {
		Unit unit = null;
		if (numSpawned % 2 == 0) {
			unit = Instantiate (monsterPrefab, transform.position, Quaternion.identity) as Unit;
		} else {
			unit = Instantiate (allyPrefab, transform.position, Quaternion.identity) as Unit;
		}
		unit.addNoise (0.1F);
//		if (numSpawned <= 5) {
//			node.AddToTrainingSet (unit);
//			if (numSpawned == 5) {
//				node.LearnUnits ();
//			}
//		} else {
//			double z = node.calculateZ (unit);
//			unit.weights = node.weights;
//			Debug.Log ("Z:"+z);
//			if (z > 0) {
//				unit.setTexture (node.getAllyTexture());
//			} else {
//				unit.setTexture (node.getEnemyTexture());
//			}
//		}
		numSpawned++;
	}
}
