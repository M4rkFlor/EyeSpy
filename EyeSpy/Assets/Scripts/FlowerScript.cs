using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tobii.Gaming;

public class FlowerScript : MonoBehaviour {
	bool youWIN;
	public Transform win;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		if (!transform.GetChild (0).gameObject.activeSelf && !transform.GetChild (1).gameObject.activeSelf) {
			transform.Translate (0, -7 * Time.deltaTime, 0);
			if (GetComponent<Rigidbody> () == null) {
				gameObject.AddComponent<Rigidbody> ().useGravity = false;
				GetComponent<Rigidbody> ().isKinematic = true;
			}
		}
	}

	void OnTriggerEnter(Collider obj) {
		if (obj.tag == "Boss" || obj.tag == "Doggo") {
			Destroy (obj.gameObject);
			youWIN = true;
			win.gameObject.SetActive (true);
		}
	}



}
