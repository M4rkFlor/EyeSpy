using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tobii.Gaming;

public class Key : MonoBehaviour {

	// Use this for initialization
	public float VisualizationDistance = 10f;
	Vector3 lookpositon;
	Vector3 lookpositonMouse;
	Vector3 MousePosition;
	Vector3 StartPosition;
	public bool UseMouse;
	public float platSpeed;
	bool stop = false;
	/// <summary>
	/// Start this instance.
	/// </summary>
	void Start () {
		StartPosition = this.transform.position;
		lookpositon=Vector3.zero;
		lookpositonMouse=Vector3.zero;
		MousePosition=Vector3.zero;
		UseMouse = GameObject.Find("Debug").GetComponent<DebugScript>().UseMouse;
	}
	
	// Update is called once per frame
	void Update () {
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
				lookpositon = hit.transform.position;
				focusedObject = hit.transform.gameObject;
			}
		}
		///////
		/// 
		if (null != focusedObject && focusedObject == this.gameObject) {
			this.transform.position=lookpositon;
		
		}

	}

	private void ProjectToPlaneInWorld(GazePoint gazePoint)
	{
		Vector3 gazeOnScreen = gazePoint.Screen;
		gazeOnScreen += (transform.forward * VisualizationDistance);
		lookpositon = Camera.main.ScreenToWorldPoint(gazeOnScreen);
	}
}
