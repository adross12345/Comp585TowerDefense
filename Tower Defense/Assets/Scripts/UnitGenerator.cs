using UnityEngine;
using System.Collections;

public class UnitGenerator : MonoBehaviour {
	public Unit[] enemies;
	public Unit[] allies;

	// Use this for initialization
	void Start () {

	}

	public Unit MakeUnit(bool enemy, int index, Vector3 position){
		Unit unit = null;
		if (enemy) {
			if (index < enemies.Length) {
				unit = Instantiate (enemies [index], position, Quaternion.identity) as Unit;
			}
		} else {
			if (index < allies.Length) {
				unit = Instantiate (allies [index], position, Quaternion.identity) as Unit;
			}
		}
		return unit;
	}

	public Unit MakeUnit(bool enemy, int index, Vector3 position, float noise){
		Unit unit = MakeUnit (enemy, index, position);
		unit.addNoise (noise);
		return unit;
	}

	public Unit MakeUnit(bool enemy, int index, Vector3 position, float noise, bool disableNavMesh){
		Unit unit = MakeUnit (enemy, index, new Vector3(0,0,0), noise);
		if (disableNavMesh) {
			unit.gameObject.GetComponent<NavMeshAgent> ().enabled = false;
			unit.transform.position = position;
		}
		return unit;
	}

	public Unit MakeUnit(bool enemy, Vector3 position){
		Unit unit = null;
		if (enemy) {
			int index = Random.Range (0, enemies.Length);
			unit = Instantiate (enemies [index], position, Quaternion.identity) as Unit;
		} else {
			int index = Random.Range (0, allies.Length);
			unit = Instantiate (allies [index], position, Quaternion.identity) as Unit;
		}
		return unit;
	}

	public Unit MakeUnit(bool enemy, Vector3 position, float noise){
		Unit unit = MakeUnit (enemy, position);
		unit.addNoise (noise);
		return unit;
	}
}