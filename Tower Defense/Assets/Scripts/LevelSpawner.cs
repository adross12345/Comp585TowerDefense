using UnityEngine;
using System.Collections;

public class LevelSpawner : MonoBehaviour {
	private TextAsset currentLevelText;
	private string[] currentLevelStrings;
	public string subfolder = "Levels";
	public int level;
	public bool isRunningLevel;

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
	}

	public void StartNextLevel(){
		currentLevelText = Resources.Load (subfolder + "/" + level) as TextAsset;
		if (currentLevelText) {
			string fullText = currentLevelText.text;
			currentLevelStrings = fullText.Split (new string[] { "\n" }, System.StringSplitOptions.RemoveEmptyEntries);
			StartCoroutine (RunTextLevel ());
		} else {
			Debug.Log ("Procedural Generation");
			//TODO add procedural generation method here
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
}
