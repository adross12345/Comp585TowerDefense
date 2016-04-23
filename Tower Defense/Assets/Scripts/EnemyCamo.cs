using UnityEngine;
using System.Collections;

public class EnemyCamo : Unit {
	protected float nextUnstealthTime;
	protected float nextStealthTime;
	public float stealthTime;
	public float rechargeTime;
	public float disappearTime = 0.25f;
	protected UnitGenerator uGen;
	protected Texture2D myTex;
	protected bool stealthed;

	// Use this for initialization
	protected override void Awake () {
		base.Awake ();
		uGen = Camera.main.GetComponent<UnitGenerator> ();
		nextStealthTime = -10f;
		stealthed = false;
	}

	void Start(){
		MeshRenderer mr = this.GetComponent<MeshRenderer> ();
		myTex = (Texture2D) mr.material.mainTexture;
	}

	public override bool EnterTower(){
		bool res = false;
		if (!stealthed && Time.time >= nextStealthTime) {
			Unit dummyUnit = uGen.MakeUnit (false, new Vector3 (-500, -500, -500), 0f, true);
			Texture2D dummyTex = (Texture2D) dummyUnit.GetComponent<MeshRenderer> ().material.mainTexture;

			StartCoroutine (ChangeTexture (dummyTex));
			dummyUnit.DestroyMe ();

			stealthed = true;
			nextUnstealthTime = Time.time + stealthTime;
			res = true;
		}
		return res;
	}
	
	// Update is called once per frame
	void Update () {
		if (stealthed && Time.time >= nextUnstealthTime) {
			StartCoroutine (ChangeTexture (myTex));
			stealthed = false;
			nextStealthTime = Time.time + rechargeTime;
		}
	}

	protected IEnumerator ChangeTexture(Texture2D newTex){
		NavMeshAgent nav = gameObject.GetComponent<NavMeshAgent> ();
		nav.enabled = false;
		Vector3 pos = transform.position;
		transform.position = new Vector3 (-500, -500, -500);
		MeshRenderer mr = this.GetComponent<MeshRenderer> ();
		mr.material.mainTexture = newTex;
		yield return new WaitForSeconds(disappearTime);
		transform.position = pos;
		nav.enabled = true;
		nav.destination = destination;
	}
}
