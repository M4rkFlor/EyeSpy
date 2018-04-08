using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tobii.Gaming;
public class MoveGear : MonoBehaviour {
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
		if (null != focusedObject && focusedObject == this.gameObject)
		{
			if (focusedObject.tag == "RotateOnLook")
			{
				print ("lookingatgear");
				transform.Rotate(0, 0, -30 * Time.deltaTime);  //LIKELY CAUSE OF PROBLEM
			}
		}
	}


	private void ProjectToPlaneInWorld(GazePoint gazePoint)
	{
		Vector3 gazeOnScreen = gazePoint.Screen;
		gazeOnScreen += (transform.forward * VisualizationDistance);
		lookpositon = Camera.main.ScreenToWorldPoint(gazeOnScreen);
	}
}
