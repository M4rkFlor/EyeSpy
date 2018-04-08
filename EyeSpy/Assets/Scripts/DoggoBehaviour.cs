using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tobii.Gaming;

public class DoggoBehaviour : MonoBehaviour {
	public float speed = 4;
	CharacterController Cc;
	Vector3 velocity;
	GameObject player;
	float timer;
	public bool UseMouse;
	public float VisualizationDistance = 10f;
	public float killTime;
	bool jump;
	float dir;
	public float jumpforce;
	public float gravity;
	// Use this for initialization
	void Start () {
		dir = -1;
		Cc = GetComponent<CharacterController> ();
		player = GameObject.FindGameObjectWithTag ("Player").gameObject;
		UseMouse = GameObject.Find("Debug").GetComponent<DebugScript>().UseMouse;
	}

	// Update is called once per frame
	void Update () {
		Movement ();

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

		//Looking into the soul of the dog
		if (null != focusedObject && focusedObject == this.gameObject) {
			timer += Time.deltaTime;
			transform.localScale = new Vector3 (transform.localScale.x + Time.deltaTime, transform.localScale.y + Time.deltaTime, transform.localScale.y + Time.deltaTime);
		} else {
			timer = 0;
			transform.localScale = new Vector3 (dir * 1.5f, 1.5f, 1.5f);
		}
		//destroy it
		if (timer > killTime) {
			Destroy (this.gameObject);
		}
	}

	private void ProjectToPlaneInWorld(GazePoint gazePoint)
	{
		Vector3 gazeOnScreen = gazePoint.Screen;
		gazeOnScreen += (transform.forward * VisualizationDistance);
	}

	void Movement() {
		int x = player.transform.position.x > transform.position.x ? -1 : 1;
		if (Mathf.Abs (dir) < 1) {
			if (x == -1) {
				dir -= Time.deltaTime;
			} else if (x == 1) {
				dir += Time.deltaTime;
			}
		} else {
			dir = x;
		}

		if (Cc.isGrounded) {
			if (jump) {
				jump = false;
			} else {
				velocity = new Vector3(-dir * speed,0,0);
			}
			if (player.transform.position.y > transform.position.y + 2 && (player.transform.position.x - 2 > transform.position.x || player.transform.position.x + 2 < transform.position.x)) {
				jump = true;
				velocity.y += jumpforce * .8f;
			}
		} else {
			velocity.x = -dir * speed;
		}
		if (dir > 0) {
			transform.localScale = new Vector3 (transform.localScale.y, transform.localScale.y, transform.localScale.y);
		} else {
			transform.localScale = new Vector3 (-transform.localScale.y, transform.localScale.y, transform.localScale.y);
		}


		velocity.y -= gravity * Time.deltaTime;
		Cc.Move (velocity * Time.deltaTime);
	}


		
}
