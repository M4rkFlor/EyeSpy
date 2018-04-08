using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecurityBehavior : MonoBehaviour {
	public bool caught;
	float speed = 4;
	CharacterController charCont;
	Vector3 velocity;
	Animator anim;
	bool dead;
	// Use this for initialization
	void Start () {
		charCont = GetComponent<CharacterController> ();
		velocity.x = speed;
		this.GetComponent<AudioSource> ().Play ();
		anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		if (!caught && !dead)
			charCont.Move (velocity * Time.deltaTime);
		else
			anim.SetTrigger ("found");
	}


	public void taken()
	{
		dead = true;
		anim.SetTrigger ("ko");
		for(int i=0;i<transform.childCount;i++)
		Destroy (transform.GetChild (i).gameObject);
	}

	void OnTriggerEnter (Collider obj)
	{
		if (obj.tag == "bound") {
			speed = -speed;
			velocity.x = speed;
			transform.localScale = new Vector3 (-transform.localScale.x, 1, 1);
		}
	}
}
