﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CannonFire : MonoBehaviour {
	public NeuralNode node;
	public float range;
	private Unit myTarget;
	private List<Unit> targetsInRange;
	private int targetsSeen;
	private bool isAILearned;

	public Projectile projectile;
	public GameObject firePoint;

	float firePauseTime = .25f;

	float fireInterval = 0.5f;
	private float nextFireTime;
	public GameObject aimPoint;

	void Awake(){
		targetsInRange = new List<Unit> ();
		node = NeuralNode.create (NeuralNode.NodeType.COLORHIST);
		targetsSeen = 0;
		isAILearned = false;
		SetRange (this.range);
		transform.Find ("Range").gameObject.GetComponent<MeshRenderer> ().enabled = false;
		this.aimPoint = Instantiate (aimPoint);
	}

	void SetRange(float newRange){
		this.range = newRange;
		SphereCollider sc = GetComponent<SphereCollider> ();
		sc.radius = newRange;
		Transform rangeIndicator = transform.Find ("Range");
		rangeIndicator.localScale = new Vector3 (newRange * 2, 0, newRange * 2);
	}

	//Picks up the target that comes into the turrets range
	void OnTriggerEnter(Collider other)
	{
		Unit u = other.gameObject.GetComponent<Unit> ();
		if(u != null){
			targetsInRange.Add (u);
			if (targetsSeen < 4) {
				node.AddToTrainingSet (u);
			}
			targetsSeen++;
		}
	}

	void OnTriggerExit(Collider other){
		Unit u = other.gameObject.GetComponent<Unit> ();
		if(u != null){
			targetsInRange.Remove (u);
			if (myTarget == u) {
				myTarget = null;
			}
		}
	}

	void FireProjectile()
	{
		if (myTarget != null) {
			nextFireTime = Time.time + fireInterval;
			Transform targTrans = myTarget.transform;
			aimPoint.transform.position = new Vector3(targTrans.position.x, this.transform.position.y, targTrans.position.z);
			transform.LookAt(aimPoint.transform);
			Projectile proj = Instantiate (projectile, firePoint.transform.position, firePoint.transform.rotation) as Projectile;
			proj.setTarget (myTarget);
		}
	}﻿

	void Update(){
		if (targetsSeen >= 4 && !isAILearned) {
			node.LearnUnits ();
			Debug.Log ("Stuff learned");
			isAILearned = true;
		}
		if (isAILearned) {
			if (myTarget == null) {
				try{
					if (targetsInRange.Count > 0) {
						//TODO change this to be able to target strong/weak/first/last
						Unit u = targetsInRange [0];
						while (u == null) {
							targetsInRange.RemoveAt (0);
							if (targetsInRange.Count > 0) {
								u = targetsInRange [0];
							} else {
								throw new UnityException ();
							}
						}
						double z = node.calculateZ (u);
						//Do AI animation
						if (z < 0) {
							//target is an enemy
							myTarget = u;
						} else {
							//Target is an ally. Next update will find another target.
							targetsInRange.Remove(u);
						}
					}
				}catch(UnityException e){
					//There were no more valid targets to check.
				}
			}
		}
		if (Time.time >= nextFireTime) {
			FireProjectile ();
		}
	}

	void OnDestroy(){
		Destroy (aimPoint);
	}

//	public void KilledTarget(GameObject o){
//		Unit u = other.gameObject.GetComponent<Unit> ();
//		if(u != null){
//			targetsInRange.Remove (u);
//			if (myTarget == u) {
//				myTarget = null;
//			}
//		}
//	}
}//Cannonfire