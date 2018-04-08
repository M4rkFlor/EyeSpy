using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tobii.Gaming;
public class MoveWithEyes : MonoBehaviour {
	/// <usefull commands>
	/// gazePoint.IsRecent(float maxAge)//Returns true if the head pose is valid and no older than maxAge seconds, false otherwise.
	/// 
	/// 
	/// GameObject focusedObject = TobiiAPI.GetFocusedObject();
	///if (null != focusedObject)
	///{
	///	print("The focused game object is: " + focusedObject.name + " (ID: " + focusedObject.GetInstanceID() + ")");
	///}
	/// 
	/// <end of commands>
	// Use this for initialization
	public float VisualizationDistance = 10f;
	Vector3 lookpositon;
	Vector3 lookpositonMouse;
	Vector3 MousePosition;
	Vector3 StartPosition;
	public bool UseMouse;
	public float platSpeed;
	bool stop = false;

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
				focusedObject = hit.transform.gameObject;
			}
		}

		//MoveObject Code
		if (null != focusedObject && focusedObject == this.gameObject) {
			if (focusedObject.tag == "MoveUpOnLook" && !stop) {
				transform.Translate (0, platSpeed * Time.deltaTime, 0);
			} else {
				if (this.transform.position.y > StartPosition.y)
					transform.Translate (0, -platSpeed * Time.deltaTime, 0);
				if (this.transform.position.y < StartPosition.y)
					this.transform.position = StartPosition;
			}
		}
		else {
			if(this.transform.position.y>StartPosition.y)
				transform.Translate (0, -platSpeed*Time.deltaTime, 0);
			if (this.transform.position.y < StartPosition.y)
				this.transform.position = StartPosition;
		}
	}


	private void ProjectToPlaneInWorld(GazePoint gazePoint)
	{
		Vector3 gazeOnScreen = gazePoint.Screen;
		gazeOnScreen += (transform.forward * VisualizationDistance);
		lookpositon = Camera.main.ScreenToWorldPoint(gazeOnScreen);
	}

	void OnTriggerEnter(Collider obj){
		if (obj.tag == "limiter") {
			stop = true;
		}
	}
	void OnTriggerExit(Collider obj){
		if (obj.tag == "limiter") {
			stop = false;
		}
	}
}
