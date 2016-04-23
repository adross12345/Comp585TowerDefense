using UnityEngine;
using System.Collections;

public class Spawn : MonoBehaviour {

	// The Monster that should be spawned
	public Unit monsterPrefab;
	public Unit allyPrefab;
	public static int numSpawned;
	//TODO take this out.
	private NeuralNode node;
	private UnitGenerator uGen;
	private LevelSpawner lvlSpawn;



	// Spawn Delay (seconds)
	public float interval = 3;

	// Use this for initialization
	void Start() {
		numSpawned = 0;
		uGen = Camera.main.GetComponent<UnitGenerator> ();

//		lvlSpawn = Camera.main.GetComponent<LevelSpawner> ();
//		lvlSpawn.StartNextLevel ();
//		InvokeRepeating("SpawnNext", 0, interval);

//				SpawnNext ();

		//		node = NeuralNode.create (NeuralNode.NodeType.GRAYSCALE);
		//		node = ScriptableObject.CreateInstance<CombinationNode> ();
		//		NeuralNode gray = NeuralNode.create (NeuralNode.NodeType.GRAYSCALE);
		//		NeuralNode color = NeuralNode.create (NeuralNode.NodeType.COLORHIST);
		//		node.AddSubNode (gray);
		//		node.AddSubNode (color);
	}

	void SpawnNext() {
		Unit unit = null;
		if (numSpawned % 2 == 0) {
			//			if (numSpawned < 8) {
			unit = uGen.MakeUnit (true, transform.position, 0.1f);
			//			}
			//			unit = Instantiate (monsterPrefab, transform.position, Quaternion.identity) as Unit;
		} else {
			//			unit = uGen.MakeUnit (false, 0, new Vector3(-500,-500,-500), 0.1f, true);

			unit = uGen.MakeUnit (false, transform.position, 0.1f);

			//			unit = Instantiate (allyPrefab, transform.position, Quaternion.identity) as Unit;
		}
//				unit.addNoise (0.1F);

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
