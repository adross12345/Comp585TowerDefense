using UnityEngine;
using System.Collections;

public class CannonFire : MonoBehaviour {

	//Finds closest enemy and adds them to a list
	GameObject FindClosestEnemy() {
		GameObject[] gos;
		gos = GameObject.FindGameObjectsWithTag("Enemy");
		GameObject closest = null;
		float distance = Mathf.Infinity;
		Vector3 position = transform.position;
		foreach (GameObject go in gos) {
			Vector3 diff = go.transform.position - position;
			float curDistance = diff.sqrMagnitude;
			if (curDistance < distance) {
				closest = go;
				distance = curDistance;
			}
		}
		return closest;
	}

	public static Vector3 CalculateInterceptCourse(Vector3 aTargetPos, Vector3 aTargetSpeed,
											Vector3 aInterceptorPos, float aInterceptorSpeed)
	{
		Vector3 targetDir = aTargetPos - aInterceptorPos;
		float speed1 = aInterceptorSpeed * aInterceptorSpeed;
		float speed2 = aTargetSpeed.sqrMagnitude;
		float fDot1 = Vector3.Dot(targetDir, aTargetSpeed);
		float targetDist2 = targetDir.sqrMagnitude;
		float d = (fDot1 * fDot1) - targetDist2 * (tSpeed2 - iSpeed2);
		if (d < 0.1f)  
			return Vector3.zero;
		float sqrt = Mathf.Sqrt(d);
		float S1 = (-fDot1 - sqrt) / targetDist2;
		float S2 = (-fDot1 + sqrt) / targetDist2;
		if (S1 < 0.0001f)
		{
			if (S2 < 0.0001f)
				return Vector3.zero;
			else
				return (S2) * targetDir + aTargetSpeed;
		}
		else if (S2 < 0.0001f)
			return (S1) * targetDir + aTargetSpeed;
		else if (S1 < S2)
			return (S2) * targetDir + aTargetSpeed;
		else
			return (S1) * targetDir + aTargetSpeed;
	}

}