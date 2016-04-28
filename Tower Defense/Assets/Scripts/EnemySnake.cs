using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemySnake : Unit {
	public float timeBetweenSpawns = 0.75f;
	public float spawnChance = 3f;
	protected int numBodiesSpawned;
	protected float timeOfLastSpawn;
	protected Vector3 spawn;
	protected UnitGenerator uGen;
	protected bool keepSpawning;
	private List<UnitAnimated> bodyAndTail;

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
		bodyAndTail = new List<UnitAnimated> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (keepSpawning && nav.enabled && Time.time >= timeOfLastSpawn + timeBetweenSpawns) {
			float chanceForExtraBody = spawnChance / numBodiesSpawned;
			float randNum = Random.Range (0f, 1f);
			if (chanceForExtraBody > randNum) {
				timeOfLastSpawn = Time.time;
				UnitAnimated body = (UnitAnimated) uGen.MakeUnit (true, 7, spawn, this.noise);
				bodyAndTail.Add (body);
				body.SetTimePerTexture (9999f);
				body.SetSpeedAndAccel (this.speed, this.acceleration);
				body.SetArmor (900f);
				numBodiesSpawned++;
			} else {
				UnitAnimated tail = (UnitAnimated) uGen.MakeUnit (true, 8, spawn, this.noise);
				bodyAndTail.Add (tail);
				tail.SetTimePerTexture (9999f);
				tail.SetSpeedAndAccel (this.speed, this.acceleration);
				tail.SetArmor (1400f);
				keepSpawning = false;
			}
		}
	}//Update()

	protected override IEnumerator DestroySelf(){
		foreach (Projectile p in aimedAtMe) {
			p.killYourself ();
		}
		NavMeshAgent nav = gameObject.GetComponent<NavMeshAgent> ();
		nav.enabled = false;
		transform.position = new Vector3 (-500, -500, -500);
		int i = 0;
		foreach (UnitAnimated bodyPart in bodyAndTail) {
			bodyPart.SetTimePerTexture (0.2f);
			if (i % 2 == 1) {
				bodyPart.indexTexture += 2;
			}
			bodyPart.SetSpeedAndAccel (bodyPart.speed / 4, bodyPart.acceleration);
			bodyPart.SetArmor (0f);
			i++;
		}
		yield return new WaitForSeconds(0.1f);
		Destroy (gameObject);
	}
}
