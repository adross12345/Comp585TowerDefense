using UnityEngine;
using System.Collections;

public class CannonFire : Unit {


	public GameObject myProjectile;
	public GameObject muzzelPos;
	public GameObject enemy; 
	float reloadTime = 1f;
	float turnSpeed = 5f;
	float firePauseTime = .25f;
	float errorAmount = .001f;
	Transform myTarget;
	Transform[] muzzlePositions;
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
				CalculateAimPosition(myTarget.position);
				turretBall.rotation = Quaternion.Lerp(turretBall.rotation, desiredRotation, Time.deltaTime*turnSpeed);
			}

			if(Time.time >= nextFireTime)
			{
				FireProjectile();
			}
		}
	}

	//Picks up the target that comes into the turrents range
	public override void OnTriggerEnter(Collider other)
	{
		nextFireTime = (float)(Time.time+(reloadTime*.5));
		myTarget = enemy.gameObject.transform;
	}


	public void OnTriggerExit(Collider other)
	{
		if(other.gameObject.transform == myTarget)
		{
			myTarget = null;
		}
	}

	//these errors are because of the onTriggerEnter method
	void CalculateAimPosition(Vector3 targetPos)
	{
		Vector3 aimPoint;
		//aimPoint = Vector3(enemy.x + aimError, enemy.y + aimError, enemy.z + aimError);
		//desiredRotation = Quaternion.LookRotation(aimPoint);
	}


	void CalculateAimError()
	{
		aimError = Random.Range(-errorAmount, errorAmount);
	}


	void FireProjectile()
	{
		
		nextFireTime = Time.time+reloadTime;
		nextMoveTime = Time.time+firePauseTime;
		CalculateAimError();


		//Instantiate(myProjectile, muzzlePos.position, muzzlePos.rotation);

	}﻿

}