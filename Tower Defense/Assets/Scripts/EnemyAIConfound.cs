using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyAIConfound : Unit {
	protected Dictionary<NeuralNode, List<int>> towersToConfound;
	public float range;
	public float confoundInterval;
	public float intendedConfoundingTime = 2f;
	protected float nextConfoundingTime;
	protected Transform rangeIndicator;
	public float rangeSpeed = 50;

	public float timePerStop = 0f;
	public float timeBetweenStop = 0f;
	protected float timeOfLastStopOrGo;
	protected bool stopped;

	// Use this for initialization
	protected override void Awake () {
		base.Awake ();
		towersToConfound = new Dictionary<NeuralNode, List<int>> ();
		nextConfoundingTime = -10f;
		this.gameObject.GetComponent<SphereCollider> ().radius = range;
		rangeIndicator = transform.Find ("Range");
		timeOfLastStopOrGo = Time.time;
		stopped = false;
	}

	// Update is called once per frame
	void Update () {
		if (timePerStop > 0) {
			if (!stopped && Time.time > timeOfLastStopOrGo + timeBetweenStop) {
				stopped = true;
				timeOfLastStopOrGo = Time.time;
				nav.SetDestination (transform.position);
			}else if(stopped && Time.time > timeOfLastStopOrGo + timePerStop) {
				stopped = false;
				timeOfLastStopOrGo = Time.time;
				nav.SetDestination (destination);
			}
		}
		if (Time.time >= nextConfoundingTime) {
			foreach (KeyValuePair<NeuralNode, List<int>> kvp in towersToConfound) {
				NeuralNode node = kvp.Key;
				int[] nonzeroIndices = node.GetNonzeroIndices ();
				if (nonzeroIndices != null && nonzeroIndices.Length > 0) {
					float numCallsToConfound = intendedConfoundingTime / Time.deltaTime;
					int numConfoundings = (int) (nonzeroIndices.Length / numCallsToConfound);
					for (int i = 0; i < numConfoundings; i++) {
						List<int> preconfoundedIndices = node.GetConfoundedIndices ();
						int index = nonzeroIndices [Random.Range (0, nonzeroIndices.Length)];
						if (!kvp.Value.Contains (index) && !preconfoundedIndices.Contains (index)) {
							if (index == -1) {
								node.b = ConfoundB (node.b);
							} else {
								ConfoundWeights (node.weights, index);
							}
							kvp.Value.Add (index);
							node.AddToConfoundedIndices (index);
						}
					}
				}
			}
			nextConfoundingTime = Time.time + confoundInterval;
		}
		Vector3 rangeSize = rangeIndicator.localScale;
		float rangeScale = rangeSize.x;
		if (rangeSize.x > 7 * 2 * range) {
			rangeScale = 0f;
		}
		Vector3 newRangeScale = new Vector3 (rangeScale + Time.deltaTime * rangeSpeed, 0, rangeScale + Time.deltaTime * rangeSpeed);
		rangeIndicator.localScale = newRangeScale;
	}

	public void RevertIndices(NeuralNode node, List<int> indicesReverted){
		List<int> indicesIveConfounded = towersToConfound[node];
		indicesIveConfounded.RemoveAll (i => indicesReverted.Contains(i));
	}

	protected virtual void ConfoundWeights(double[] weights, int index){

	}

	protected virtual double ConfoundB (double curB){
		return 0.0;
	}

	protected void OnTriggerEnter(Collider co) {
		if (co.tag == "Turret" && co is BoxCollider) {
			CannonFire cf = co.gameObject.GetComponent<CannonFire> ();
			if (cf != null) {
				NeuralNode node = cf.GetNode ();
				if (node != null) {
					towersToConfound [node] = new List<int> ();
				}
			}
		}
	}

	protected virtual void OnTriggerExit(Collider co){
		if (co.tag == "Turret" && co is BoxCollider) {
			CannonFire cf = co.gameObject.GetComponent<CannonFire> ();
			if (cf != null) {
				NeuralNode node = cf.GetNode ();
				if (node != null) {
					List<int> confoundedIndices = towersToConfound [node];
					node.ResetWeights (confoundedIndices);
					node.RemoveFromEnemiesToInform (this);
					node.InformEnemies (confoundedIndices);
				}
			}
		}
	}//OnTriggerExit()
}//Class
