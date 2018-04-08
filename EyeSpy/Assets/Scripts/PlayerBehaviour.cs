using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tobii.Gaming;
using UnityEngine.SceneManagement;
[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(GazeAware))]
public class PlayerBehaviour : MonoBehaviour {
	//Movement Variables
	CharacterController Cc;
	public float speed;
	public float jumpforce;
	Vector3 velocity = Vector3.zero;
	public float gravity;
	bool jump;
	Animator anim;

	//Blink Variables
	private GazeAware _gazeAwareComponent;
	private float xpos;
	private float ypos;
	private float blinkTime;
	public bool invisible;
	public bool UseMouse;
	bool dead;
	//Misc
	private GameObject light;
	void Start () {
		Cc = GetComponent<CharacterController>();
		anim = GetComponent<Animator>();
		UseMouse = GameObject.Find("Debug").GetComponent<DebugScript>().UseMouse;
		light = GetComponentInChildren<Light> ().gameObject;
	}
	
	// Update is called once per frame
	void Update () {
		UseMouse = GameObject.Find("Debug").GetComponent<DebugScript>().UseMouse;
		if(!dead)
		Movement ();
		EyesClosed ();

		if (invisible) {
			GetComponent<SpriteRenderer> ().material.color = Color.black;
		} else {
			GetComponent<SpriteRenderer> ().material.color = Color.white;
		}
			
	}

	void Movement() {
		if (Cc.isGrounded) {
			if (jump) {
				jump = false;
			} else {
				velocity = new Vector3 (Input.GetAxis ("Horizontal") * speed, 0, 0);
			}
			if (Input.GetKeyDown (KeyCode.W)) {
				jump = true;
				velocity.y += jumpforce * .8f;
			}
		} else {
			velocity.x = speed * Input.GetAxis ("Horizontal");
		}

		if (velocity.x != 0) {
			int scale = Input.GetAxis("Horizontal") > 0 ? 1 : -1;
			transform.localScale = new Vector3 (scale, 1, 1);
		}

		velocity.y -= gravity * Time.deltaTime;
		Cc.Move (velocity * Time.deltaTime);
		anim.SetFloat ("x-speed", Mathf.Abs(velocity.x));
		if (Cc.isGrounded)
			anim.SetFloat ("y-speed", 0);
		else
			anim.SetFloat ("y-speed", Mathf.Abs (velocity.y));
	}

	void EyesClosed() {
		if (!UseMouse) {
			GazePoint gazePoint = TobiiAPI.GetGazePoint ();

			if (xpos == gazePoint.Screen.x && ypos == gazePoint.Screen.y) {
				blinkTime += Time.deltaTime;
			} else {
				invisible = false;
				blinkTime = 0;
			}
			xpos = gazePoint.Screen.x;
			ypos = gazePoint.Screen.y;

			if (blinkTime > .4f) {
				invisible = true;
			}
		} else {
			if (Input.GetKeyDown (KeyCode.Space)) {
				invisible = true;
				light.SetActive (false);
			} else if(Input.GetKeyUp(KeyCode.Space)){
				invisible = false;
				light.SetActive (true);
			}
		}
	}

	void OnTriggerEnter(Collider obj) {
		if (obj.tag == "MoveHitBox") {
			transform.SetParent (obj.transform);
		}
		if (obj.tag == "flashlight" && !invisible && !dead) {
			obj.GetComponentInParent<SecurityBehavior> ().caught = true;
			//Death
			StartCoroutine(Death());
		}
		if (obj.tag == "flashlightA" && !invisible && !dead) {
			//Death
			obj.GetComponentInParent<AgentBehavior> ().caught();
			StartCoroutine(Death());
		}
		if (obj.tag == "Doggo" && !dead) {
			
			StartCoroutine(Death());
		}
	}
	void OnTriggerExit(Collider obj) {
		if (obj.tag == "MoveHitBox") {
			obj.transform.DetachChildren();
		}
	}
	void OnTriggerStay(Collider obj) {
		if (obj.tag == "flashlight" && !invisible && !dead) {
			obj.GetComponentInParent<SecurityBehavior> ().caught = true;
			StartCoroutine(Death());
		}
		if (obj.tag == "takedown" && Input.GetKeyDown(KeyCode.E) && !invisible && velocity.y<0.001f){
			anim.SetTrigger ("attack");
			obj.GetComponentInParent<SecurityBehavior> ().taken();
		}
		if (obj.tag == "flashlightA" && !invisible && !dead) {
			//Death
			obj.GetComponentInParent<AgentBehavior> ().caught();
			StartCoroutine(Death());
		}
	
	}
	IEnumerator Death()
	{
		dead = true;
		anim.SetTrigger ("gameover");
		yield return new WaitForSeconds(2);
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}
}
