using UnityEngine;
using System.Collections;

public class SpeedChangeButton : MonoBehaviour {
	public bool doubleSpeed;

	public void ChangeSpeed(){
		if (doubleSpeed) {
			Time.timeScale *= 2;
		} else {
			Time.timeScale = 1;
		}
	}
}
