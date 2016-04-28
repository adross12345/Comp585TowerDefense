using UnityEngine;
using System.Collections;

public class UnitHydra : Unit {
	public int splitsRemaining=2;

	public override void SetSplitsRemaining(int numSplits){
		this.splitsRemaining = numSplits;
	}

	protected override IEnumerator DestroySelf(){
		foreach (Projectile p in aimedAtMe) {
			p.killYourself ();
		}
		NavMeshAgent nav = gameObject.GetComponent<NavMeshAgent> ();
		if (!nav.enabled) {
			splitsRemaining = 0;
		}
		nav.enabled = false;
		Vector3 curPos = transform.position;
		Vector3 farOff = new Vector3 (-500, -500, -500);
		transform.position = farOff;

		yield return new WaitForSeconds(0.1f);

		if(splitsRemaining <= 0){
			Destroy (gameObject);
			return false;
		}
		Texture2D myTex = (Texture2D) GetComponent<MeshRenderer> ().material.mainTexture;
		Color[] myPixels = myTex.GetPixels ();
		int myWidth = myTex.width;
		int myHeight = myTex.height;
		Color blackPixel = new Color (0f, 0f, 0f);

		UnitGenerator uGen = Camera.main.GetComponent<UnitGenerator> ();
		Unit[] subUnits = new Unit[3];
		for (int i = 0; i < 3; i++) {
			subUnits[i] = uGen.MakeUnit(true, 4, farOff, 0f, true);
			subUnits [i].SetSplitsRemaining (splitsRemaining - 1);
			Texture2D tex = (Texture2D) subUnits[i].GetComponent<MeshRenderer> ().material.mainTexture;
			Color[] newPixels = new Color[myPixels.Length];
			for (int j = 0; j < myPixels.Length; j++) {
				if (splitsRemaining == 2) {
					if ((j >= i * myPixels.Length / 3) && j < ((i + 1) * myPixels.Length / 3)) {
						newPixels [j] = myPixels [j];
					} else {
						newPixels [j] = blackPixel;
					}
				} else if (splitsRemaining == 1) {
					if ((j % myHeight >= i * myWidth / 3) && (j % myHeight < (i + 1) * myWidth / 3)) {
						newPixels [j] = myPixels [j];
					} else {
						newPixels [j] = blackPixel;
					}
				}//splitsRemaining
			}//for(j)
			tex.SetPixels(newPixels);
			tex.Apply ();
			NavMeshAgent newNav = subUnits [i].GetComponent<NavMeshAgent> ();
			Vector3 offsetPos = new Vector3 (curPos.x + ((i - 1) * 0.3f), curPos.y, curPos.z + ((i - 1) * 0.3f));
			subUnits [i].transform.position = offsetPos;
			subUnits [i].SetSpeedAndAccel (this.speed, this.acceleration + ((i-1) * 1f));
			subUnits [i].SetArmor (this.armor / 2);
			subUnits [i].damage = this.damage/2;
			newNav.enabled = true;
			newNav.destination = destination;

		}//for(i<3)



		Destroy (gameObject);
	}
}
