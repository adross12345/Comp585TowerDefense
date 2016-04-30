using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CannonFire : MonoBehaviour {
	public NeuralNode node;
	public float range;
	protected Unit myTarget;
	protected List<Unit> targetsInRange;
	protected int targetsSeen;

	public Projectile projectile;
	public GameObject[] firePoints;

	public float fireInterval = 0.5f;
	public float myDamage = 0.5f;
	public float mySplashDamage = 0.25f;
	protected float nextFireTime;
	public GameObject aimPoint;

	protected Unit playerTarget;

	protected virtual void Awake(){
		targetsInRange = new List<Unit> ();
		node = NeuralNode.create (NeuralNode.NodeType.FULLCOLOR);
		targetsSeen = 0;
		SetRange (this.range);
		transform.Find ("Range").gameObject.GetComponent<MeshRenderer> ().enabled = false;
		this.aimPoint = Instantiate (aimPoint);
	}

	protected void SetRange(float newRange){
		this.range = newRange;
		SphereCollider sc = GetComponent<SphereCollider> ();
		sc.radius = newRange;
		Transform rangeIndicator = transform.Find ("Range");
		rangeIndicator.localScale = new Vector3 (newRange * 2, 0, newRange * 2);
	}

	//Picks up the target that comes into the turrets range
	protected virtual void OnTriggerEnter(Collider other)
	{
		if (other is BoxCollider) {
			Unit u = other.gameObject.GetComponent<Unit> ();
			if (u != null) {
				bool willDisappear = u.EnterTower ();
				if (!willDisappear) {
					targetsInRange.Add (u);
					targetsSeen++;
				}
			}
		}
	}

	protected virtual void OnTriggerExit(Collider other){
		if (other is BoxCollider) {
			Unit u = other.gameObject.GetComponent<Unit> ();
			if (u != null) {
				targetsInRange.Remove (u);
				if (myTarget == u) {
					myTarget = null;
				}
			}
		}
	}

	protected virtual void FireProjectile()
	{
		if (myTarget != null) {
			nextFireTime = Time.time + fireInterval;
			bool rotate = firePoints.Length <= 1;
			foreach (GameObject go in firePoints) {
				if (rotate) {
					Transform targTrans = myTarget.transform;
					aimPoint.transform.position = new Vector3 (targTrans.position.x, this.transform.position.y, targTrans.position.z);
					transform.LookAt (aimPoint.transform);
				}
				Projectile proj = Instantiate (projectile, go.transform.position, go.transform.rotation) as Projectile;
				proj.SetDamage (myDamage);
				proj.setTarget (myTarget);
				//Will do nothing with most projectiles.
				proj.SetSplashDamage (mySplashDamage);
			}
		}
	}﻿

	protected virtual void Update(){
		if (node.isAILearned) {
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
						node.lastZ = z;
						//						Debug.Log(z);
						//Do AI animation
						if (z <=0) {
							//target is an enemy
							myTarget = u;
						}else if (z == 0) {
							//Randomly picks if unit is ally or enemy.
							int rand = UnityEngine.Random.Range (0, 2);
							if (rand == 0) {
								myTarget = u;
							} else {
								targetsInRange.Remove(u);
							}
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

	public NeuralNode GetNode(){
		return node;
	}

	public virtual void SetPlayerTarget(Unit u){
	}

	protected void OnDestroy(){
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