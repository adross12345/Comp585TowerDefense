using UnityEngine;
using System.Collections;

public class EnemySnake : Enemy {
	public float timeBetweenSpawns = 0.75f;
	public float spawnChance = 3f;
	protected int numBodiesSpawned;
	protected float timeOfLastSpawn;
	protected Vector3 spawn;
	protected UnitGenerator uGen;
	protected bool keepSpawning;

	// Use this for initialization
	protected override void Awake () {
		base.Awake ();
		timeOfLastSpawn = Time.time;
		numBodiesSpawned = 1;
		keepSpawning = true;
		uGen = Camera.main.GetComponent<UnitGenerator> ();
		GameObject go = GameObject.Find ("Spawn");
		if (go) {
			spawn = go.transform.position;
		} else {
			Debug.Log ("Spawn not found");
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (keepSpawning && Time.time >= timeOfLastSpawn + timeBetweenSpawns) {
			float chanceForExtraBody = spawnChance / numBodiesSpawned;
			float randNum = Random.Range (0f, 1f);
			if (chanceForExtraBody > randNum) {
				timeOfLastSpawn = Time.time;
				uGen.MakeUnit (true, 7, spawn, this.noise);
				numBodiesSpawned++;
			} else {
				uGen.MakeUnit (true, 8, spawn, this.noise);
				keepSpawning = false;
			}
		}
	}
}
