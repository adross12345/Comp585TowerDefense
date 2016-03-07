using UnityEngine;
using System.Collections;

public class CannonFire : MonoBehaviour {


	public GameObject projectile;
	public GameObject firePoint;
	private Transform myTarget;
	float firePauseTime = .25f;

	float fireInterval = 1f;
	float turnSpeed = 5f;
	private float nextFireTime;
	private float nextMoveTime;
	private Vector3[] enemyPosition;
	private Quaternion desiredRotation;
	private float aimError;


	//Picks up the target that comes into the turrents range
	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "Enemy") 
		{
			nextFireTime = (float)(Time.time+(fireInterval*.5));
			Debug.Log ("Reached");
			Vector3 aimPoint = new Vector3(other.transform.position.x, other.transform.position.y, 
				other.transform.position.z);
			transform.rotation = Quaternion.LookRotation(aimPoint);
			FireProjectile ();
		}
	}

	void FireProjectile()
	{
		float nextFireTime = Time.time + fireInterval;
		float nextMoveTime = Time.time + firePauseTime;
		Instantiate(projectile, firePoint.transform.position, firePoint.transform.rotation);
	}﻿
}

	/*****
	public GameObject myProjectile;
	public GameObject firePoint;
	public GameObject enemy; 
	float reloadTime = 1f;
	float turnSpeed = 5f;
	float firePauseTime = .25f;
	float errorAmount = .001f;
	Transform myTarget;
	Transform turretBall;


	private float nextFireTime;
	private float nextMoveTime;
	private Quaternion desiredRotation;
	private float aimError;


	void Update () 
	{
		if(myTarget)
		{
			if(Time.time >= nextMoveTime)
			{
				CalculateAimPosition(enemy);
				turretBall.rotation = Quaternion.Lerp(turretBall.rotation, desiredRotation, Time.deltaTime*turnSpeed);
			}

			if(Time.time >= nextFireTime)
			{
				FireProjectile();
			}
		}
	}

	//Picks up the target that comes into the turrents range
	public void OnTriggerEnter(Collider enemy)
	{
		nextFireTime = (float)(Time.time+(reloadTime*.5));
		myTarget = enemy.gameObject.transform;
	}


	public void OnTriggerExit(Collider enemy)
	{
		if(enemy.gameObject.transform == myTarget)
		{
			myTarget = null;
		}
	}
	
	void CalculateAimError()
	{
		aimError = Random.Range(-errorAmount, errorAmount);
	}


	**/
