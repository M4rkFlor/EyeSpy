using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UltraBossScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		transform.eulerAngles = new Vector3 (0, 0, 180);

	}
	
	// Update is called once per frame
	void Update () {
		transform.Rotate (0, -10, 0);
	}
}
