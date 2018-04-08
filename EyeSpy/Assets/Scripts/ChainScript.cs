using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tobii.Gaming;

public class ChainScript : MonoBehaviour {
	public bool shake = false;
	public bool UseMouse;
	float timer;
	public float killTime;
	public float VisualizationDistance = 10f;
	int dir;
	// Use this for initialization
	void Start () {
		UseMouse = GameObject.Find("Debug").GetComponent<DebugScript>().UseMouse;
	}
	
	// Update is called once per frame
	void Update () {
		Animator[] x = GetComponentsInChildren<Animator> ();
		foreach (Animator y in x) {
			y.SetBool ("Shake", shake);
		}

		TobbiiStuff ();
	}

	private void TobbiiStuff() {
		UseMouse = GameObject.Find("Debug").GetComponent<DebugScript>().UseMouse;

		GameObject focusedObject;
		if (!UseMouse) {
			//LookpositonData
			GazePoint gazePoint = TobiiAPI.GetGazePoint ();
			if (gazePoint.IsRecent ()) { // Use IsValid property instead to process old but valid data
				ProjectToPlaneInWorld (gazePoint);
			}

			//FocusObject
			focusedObject = TobiiAPI.GetFocusedObject ();
		} 
		else {
			//for mouse
			focusedObject = null;
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			RaycastHit hit;
			if (Physics.Raycast(ray,out hit)) {
				print (hit.transform.name);
				focusedObject = hit.transform.gameObject;
			}
		}


		if (null != focusedObject && focusedObject == this.gameObject) {
			timer += Time.deltaTime;
			shake = true;
		} else {
			timer = 0;
			shake = false;
		}
		if (timer > killTime) {
			gameObject.SetActive (false);
		}
	}

	private void ProjectToPlaneInWorld(GazePoint gazePoint)
	{
		Vector3 gazeOnScreen = gazePoint.Screen;
		gazeOnScreen += (transform.forward * VisualizationDistance);
	}
}
