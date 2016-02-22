using UnityEngine;
using System.Collections;

public class Spawn : MonoBehaviour {

	// The Monster that should be spawned
	public Unit monsterPrefab;
	public Unit allyPrefab;
	public static int numSpawned;
	//Remove this. Put it in the tower.
	//It's here now for testing purposess.
	private NeuralNode node;

	// Spawn Delay (seconds)
	public float interval = 3;

	// Use this for initialization
	void Start() {
		node = new NeuralNode ();
		node.Start ();
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
		if (numSpawned < 6) {
			node.AddToTrainingSet (unit);
		}else if (numSpawned == 6) {
			Debug.Log ("Learning Units");
			node.LearnUnits ();
			double z = node.calculateZ (unit);
			Debug.Log ("Z:"+z);
			if (z > 0) {
				unit.setTexture (node.getAllyTexture());
			} else {
				unit.setTexture (node.getEnemyTexture());
			}
		} else if (numSpawned > 6) {
			double z = node.calculateZ (unit);
			Debug.Log ("Z:"+z);
			if (z > 0) {
				unit.setTexture (node.getAllyTexture());
			} else {
				unit.setTexture (node.getEnemyTexture());
			}
		}
		numSpawned++;
	}
}
