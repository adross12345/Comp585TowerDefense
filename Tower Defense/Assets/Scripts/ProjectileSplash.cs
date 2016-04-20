using UnityEngine;
using System.Collections;

public class ProjectileSplash : Projectile {
	public SplashRadius splashObject;
	public float splashRadius = 2f;
	
	protected override void OnTriggerEnter(Collider other){	
		Unit u = other.gameObject.GetComponent<Unit> ();
		if(u == target && other is BoxCollider){
			if (u != null) {
				u.takeDamage (myDamage);
			}
			target.removeFromAimedAtMe (this);
			TriggerSplash ();
			Destroy (gameObject);
		}
	}

	private void TriggerSplash(){
		SplashRadius spl = Instantiate (splashObject, transform.position, Quaternion.identity) as SplashRadius;
		spl.SetMyDamage (splashDamage);
		spl.SetSplashRadius (splashRadius);
	}
}//ProjectileSplash
