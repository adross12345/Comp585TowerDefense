using UnityEngine;
using System.Collections;

public class DisableableButton : MonoBehaviour {
	public void Disable(){
		gameObject.SetActive (false);
	}
}
