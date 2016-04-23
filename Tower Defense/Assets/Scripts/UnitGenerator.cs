using UnityEngine;
using System.Collections;

public class UnitGenerator : MonoBehaviour {
	public Unit[] enemies;
	public Unit[] allies;
	public Vector3 forward = new Vector3(1,0,0);
	private Vector3 up = new Vector3(0,1,0);

	// Use this for initialization
	void Start () {

	}

	public Unit MakeUnit(bool enemy, int index, Vector3 position){
		Unit unit = null;
		if (enemy) {
			if (index < enemies.Length) {
				unit = Instantiate (enemies [index], position, Quaternion.LookRotation(forward, up)) as Unit;
//				Debug.Log ("Made enemy type " + index);
			}
		} else {
			if (index < allies.Length) {
				unit = Instantiate (allies [index], position, Quaternion.LookRotation(forward, up)) as Unit;
//				Debug.Log ("Made ally type " + index);
			}
		}
		return unit;
	}

	public Unit MakeUnit(bool enemy, int index, Vector3 position, float noise){
		if (index == -1) {
			return MakeUnit (enemy, position, noise);
		}
		Unit unit = MakeUnit (enemy, index, position);
		unit.addNoise (noise);
		return unit;
	}

	public Unit MakeUnit(bool enemy, int index, Vector3 position, float noise, bool disableNavMesh){
		Unit unit = MakeUnit (enemy, index, new Vector3(0,0,0), noise);
		if (disableNavMesh) {
			unit.gameObject.GetComponent<NavMeshAgent> ().enabled = false;
		}
		unit.transform.position = position;
		return unit;
	}

	public Unit MakeUnit(bool enemy, Vector3 position, float noise, bool disableNavMesh){
		Unit unit = MakeUnit (enemy, new Vector3(0,0,0), noise);
		if (disableNavMesh) {
			unit.gameObject.GetComponent<NavMeshAgent> ().enabled = false;
		}
		unit.transform.position = position;
		return unit;
	}

	public Unit MakeUnit(bool enemy, Vector3 position){
		Unit unit = null;
		if (enemy) {
			int index = Random.Range (0, enemies.Length);
			unit = MakeUnit (true, index, position);
		} else {
			int index = Random.Range (0, allies.Length);
			unit = MakeUnit (false, index, position);
		}
		return unit;
	}

	public Unit MakeUnit(bool enemy, Vector3 position, float noise){
		Unit unit = MakeUnit (enemy, position);
		unit.addNoise (noise);
		return unit;
	}
}