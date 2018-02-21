using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KolobokMoving : MonoBehaviour {
	private Rigidbody body;
	// Use this for initialization
	void Start () {
		body = GetComponent<Rigidbody> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey ("escape")) {
			Application.Quit ();
		}
	}

	void FixedUpdate(){
		#if(UNITY_EDITOR || UNITY_STANDALONE)
		float hor = Input.GetAxis ("Horizontal");
		float ver = Input.GetAxis ("Vertical");
		Vector3 VC = new Vector3 (hor, 0, ver);
		body.AddForce (VC * 3);
		#elif(UNITY_ANDROID || UNITY_IOS)
		//float hor = -Input.acceleration.y;
		//float ver = -Input.acceleration.x;
		float hor = Input.acceleration.x;
		float ver = Input.acceleration.y;
		Vector3 VC = new Vector3 (hor,0,ver);
		if(VC.sqrMagnitude > 1) VC.Normalize();
		body.AddForce (VC * 3);
		#endif
	}

	void OnTriggerEnter(Collider col){
		if (col.gameObject.CompareTag ("Candy")) {
			Destroy (col.gameObject);
		}
	}
}
