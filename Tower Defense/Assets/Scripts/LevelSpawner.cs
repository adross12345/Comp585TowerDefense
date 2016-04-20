using UnityEngine;
using System.Collections;

public class LevelSpawner : MonoBehaviour {
	private TextAsset currentLevelText;
	private string[] currentLevelStrings;
	public string subfolder = "Levels";
	public int level;
	public bool isRunningLevel;

	public float enemyScalingPower = 1.8f;
	public float enemyScalingScalar = .5f;
	public float allyScalingPower = 0.7f;
	public float allyScalingScalar = 5f;
	public float rushContuationScalingPower = 0.5f;
	public float startingDelay = 3f;
	public float delayScalingFactor = 0.75f;
	public float noiseScalingFactor = 0.8f;
	private float noiseScalingScalar;
	public float rushIncrease = 5f;

	public int numEnemiesSpawned;
	public int numAlliesSpawned;
	public int numEnemiesToSpawn;
	public int numAlliesToSpawn;

	private UnitGenerator uGen;
	private Vector3 spawn;

	// Use this for initialization
	void Awake () {
		isRunningLevel = false;
		level = 1;
		uGen = Camera.main.GetComponent<UnitGenerator> ();
		GameObject go = GameObject.Find ("Spawn");
		if (go) {
			spawn = go.transform.position;
		} else {
			Debug.Log ("Spawn not found");
		}
		//Makes it so the noise is 0.25 at level 50.
		noiseScalingScalar = 0.25f / Mathf.Pow (50f, noiseScalingFactor);
	}

	public void StartNextLevel(){
		currentLevelText = Resources.Load (subfolder + "/" + level) as TextAsset;
		if (currentLevelText) {
			string fullText = currentLevelText.text;
			currentLevelStrings = fullText.Split (new string[] { "\n" }, System.StringSplitOptions.RemoveEmptyEntries);
			StartCoroutine (RunTextLevel ());
		} else {
			Debug.Log ("Procedural Generation");
			StartCoroutine(RunProceduralGeneration ());
		}
	}

	private IEnumerator RunTextLevel(){
		isRunningLevel = true;
		for (int i = 0; i < currentLevelStrings.Length; i++) {
			yield return StartCoroutine (RunLine (i));
		}
		level += 1;
		isRunningLevel = false;
	}

	private IEnumerator RunLine(int lineNumber){
		string line = currentLevelStrings [lineNumber];
		string[] lineParts = line.Split (default(string[]), System.StringSplitOptions.RemoveEmptyEntries);
		if (lineParts.Length < 4) {
			if (lineParts.Length >= 2 && "Wait".Equals (lineParts [0])) {
				float delay = float.Parse (lineParts [1]);
				yield return new WaitForSeconds (delay);
			} else {
				yield return StartCoroutine (ErrorLine (lineNumber));
			}
		} else {
			if ("Spawn".Equals (lineParts [0])) {
				float delay = float.Parse (lineParts [1]);
				float noise = float.Parse (lineParts [2]);
				bool enemy = "0".Equals (lineParts [3]);
				int index = -1;
				if (lineParts.Length >= 5) {
					index = int.Parse (lineParts [4]);
				}
				uGen.MakeUnit (enemy, index, spawn, noise);
				yield return new WaitForSeconds (delay);

				//Spawn Command

			} else if ("Execute".Equals (lineParts [0])) {
				float delay = float.Parse (lineParts [1]);
				int linesAbove = int.Parse (lineParts [2]);
				int numExecutes = int.Parse (lineParts [3]);
				int startLine = lineNumber - linesAbove;
				if (startLine < 0 || linesAbove <= 0 || delay < 0) {
					Debug.Log ("Execute Index incorrect at line " + lineNumber);
					yield return new WaitForEndOfFrame ();
				} else {
					for (int execution = 0; execution < numExecutes; execution++) {
						//Execution delay is first
						yield return new WaitForSeconds (delay);

						int lineExecuting = startLine;
						while (lineExecuting < lineNumber) {
							yield return StartCoroutine (RunLine (lineExecuting));
							lineExecuting++;
						}
					}
				}//Execution command is A-OK to run

				//Execution command

			} else{
				//Nonsense command
				yield return StartCoroutine (ErrorLine (lineNumber));
			}
		} 
	}//RunLine()

	private IEnumerator ErrorLine(int lineNumber){
		Debug.Log ("Incorrect text formatting at line " + lineNumber);
		yield return new WaitForEndOfFrame ();
	}

	private IEnumerator RunProceduralGeneration(){
		isRunningLevel = true;
		numAlliesSpawned = 0;
		numEnemiesSpawned = 0;
		numEnemiesToSpawn = CalcEnemiesToSpawn();
		numAlliesToSpawn = CalcAlliesToSpawn ();
		float delay = CalcDelay ();
		float noise = CalcNoise();
		float rushNum = CalcRushNum ();
		//TODO add full rush levels, etc.
		while (numAlliesSpawned < numAlliesToSpawn && numEnemiesToSpawn > numEnemiesSpawned) {
			float enemySpawnChance = (float) numEnemiesToSpawn / (numAlliesToSpawn + numEnemiesToSpawn);
			Debug.Log (enemySpawnChance);
			bool spawnEnemy = false;
			if (enemySpawnChance > Random.Range (0f, 1f)) {
				spawnEnemy = true;
			}
			yield return SpawnUnit (spawnEnemy, noise, delay, rushNum);
		}
		level += 1;
		isRunningLevel = false;
	}

	private IEnumerator SpawnUnit(bool spawnEnemy, float noise, float delay, float rushNum){
		if ((startingDelay * 4 / rushNum) < Random.Range (0, 1)) {
			yield return SpawnRush (spawnEnemy, noise, delay, rushNum, 0);
			yield return new WaitForSeconds (delay * 5f);
		} else {
			uGen.MakeUnit (spawnEnemy, spawn, noise);
			AddToNumSpawned (spawnEnemy);
			yield return new WaitForSeconds (delay);
		}
	}

	private IEnumerator SpawnRush(bool spawnEnemy, float noise, float delay, float rushNum, int numUnitsSpawned){
		numUnitsSpawned++;
		if(ContinueRush(numUnitsSpawned, rushNum)){
			yield return SpawnRush (spawnEnemy, noise, delay, rushNum, numUnitsSpawned);
		}
		uGen.MakeUnit (spawnEnemy, spawn, noise);
		AddToNumSpawned (spawnEnemy);
		yield return new WaitForSeconds (delay / 5f);
	}

	private bool ContinueRush(int numUnitsSpawned, float rushNum){
		float cutOff = rushNum /(float) numUnitsSpawned;
		float rand = Random.Range (0, 1);
		if (rand > cutOff) {
			return false;
		}
		return true;
	}

	private void AddToNumSpawned(bool isEnemy){
		if (isEnemy) {
			numEnemiesSpawned++;
		} else {
			numAlliesSpawned++;
		}
	}

	private int CalcEnemiesToSpawn(){
		return (int) (enemyScalingScalar*Mathf.Pow((float) level, enemyScalingPower));
	}

	private int CalcAlliesToSpawn(){
		return (int) (allyScalingScalar*Mathf.Pow((float) level, allyScalingPower));
	}

	private float CalcRushNum(){
		return (4 * Mathf.Pow ((float)level, rushContuationScalingPower));
	}

	private float CalcDelay(){
		return (int) 3f/Mathf.Pow((float) level, delayScalingFactor);
	}

	private float CalcNoise(){
		return (int) noiseScalingScalar * Mathf.Pow((float) level, noiseScalingFactor);
	}


}
