using UnityEngine;
using System.Collections;

public class CannonFire : MonoBehaviour {


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

	//these errors are because of the onTriggerEnter method
	void CalculateAimPosition(Vector3 targetPos)
	{
		Debug.Log ("Reached");
		Vector3 aimPoint = new Vector3(enemy.transform.position.x + aimError, enemy.transform.position.y + aimError, 
			enemy.transform.position.z + aimError);
		//transform.rotation.SetLookRotation = Quaternion.LookRotation(aimPoint);

		//SetLookRotation
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
		Instantiate(myProjectile, firePoint.transform.position, firePoint.transform.rotation);

	}﻿

}