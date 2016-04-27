using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class CannonFireAOE : CannonFire {
	private Dictionary<Unit, UnitID> identities;
	public int numEnemiesInRange;
	public int numAlliesInRange;
	public float cutoffRatio = 0;

	enum UnitID{Enemy, Ally	};

	// Use this for initialization
	protected override void Awake () {
		base.Awake ();
		identities = new Dictionary<Unit, UnitID> ();
		numEnemiesInRange = 0;
		numAlliesInRange = 0;
	}//Start

	protected override void Update(){
		if (node.isAILearned && myTarget == null && targetsInRange.Count > 0) {
			try{
				if(cutoffRatio < 0){
					myTarget = targetsInRange[0];
				}else if(numEnemiesInRange/numAlliesInRange >= cutoffRatio && numEnemiesInRange>0){
					foreach(Unit u in targetsInRange){
						if(identities.ContainsKey(u)){
							UnitID uID = identities[u];
							if(uID == UnitID.Enemy){
								myTarget = u;
								break;
							}//if(enemy)
						}//if(contains(u))
					}//foreach
				}
			}catch(DivideByZeroException e){
				if(numEnemiesInRange > 0){
					foreach(Unit u in targetsInRange){
						if(identities.ContainsKey(u)){
							UnitID uID = identities[u];
							if(uID == UnitID.Enemy){
								myTarget = u;
								break;
							}//if(enemy)
						}//if(contains(u))
					}//foreach
				}
			}
			//TODO change this to be able to target strong/weak/first/last
		}
		if (Time.time >= nextFireTime) {
			FireProjectile ();
		}
	}

	//Picks up the target that comes into the turrets range
	protected override void OnTriggerEnter(Collider other)
	{
		Unit u = other.gameObject.GetComponent<Unit> ();
		if(u != null && other is BoxCollider){
			bool willDisappear = u.EnterTower ();
			if (!willDisappear) {
				targetsInRange.Add (u);
				if (node.isAILearned) { // else if (isAILearned)
					double z = node.calculateZ (u);
					node.lastZ = z;
					node.SetTargetTex ((Texture2D) u.GetComponent<MeshRenderer> ().material.mainTexture);
//					Debug.Log (z+" "+node.b);
					UnitID uID = UnitID.Enemy;
					//Do AI animation
					if (z < 0) {
						//Target is an enemy
						numEnemiesInRange++;
					} else if (z == 0) {
						//Randomly picks if unit is ally or enemy.
						int rand = UnityEngine.Random.Range (0, 2);
						if (rand == 0) {
							numEnemiesInRange++;
						} else {
							numAlliesInRange++;
							uID = UnitID.Ally;
						}
					}else {
						//Target is an ally. Next update will find another target.
						numAlliesInRange++;
						uID = UnitID.Ally;
					}
					identities [u] = uID;
				}
				//			Debug.Log (targetsSeen + " " + identities.Keys.Count);
				//				foreach (UnitID uID in identities.Values) {
				//				numEnemiesInRange = 0;
				//				numAlliesInRange = 0;
				//				if(uID == UnitID.Enemy){
				//					numEnemiesInRange++;
				//				}else{
				//					numAlliesInRange++;
				//				}
				//				Debug.Log (targetsSeen + " " + uID);
				//
				//				}
				targetsSeen++;
			}
		}
	}

	protected override void OnTriggerExit(Collider other){
		Unit u = other.gameObject.GetComponent<Unit> ();
		if (u != null && other is BoxCollider) {
			targetsInRange.Remove (u);
			if (myTarget == u) {
				myTarget = null;
			}
			if (identities.ContainsKey (u)) {
				UnitID uID = identities [u];

				if (uID == UnitID.Enemy) {
					numEnemiesInRange--;
				} else {
					numAlliesInRange--;
				}
				if (!identities.Remove (u)) {
					Debug.Log ("Unit unsuccessfully removed");
				} 
			} else {
				Debug.Log ("Unit " + u + " not found in Dictionary");
				//The unit was not in the dictionary list
			}
		} 
	}

    public void setCutoff(string input) {
        float value;
        if (float.TryParse(input, out value)) {
            cutoffRatio = Convert.ToSingle(input);
            Debug.Log("Cutoff Ratio is now " + cutoffRatio);
        } else {
            Debug.Log("Number in input field is not a number");
        }
    }
}
