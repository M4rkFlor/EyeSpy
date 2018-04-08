using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentBehavior : MonoBehaviour {
	Animator anim;
	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator> ();
	}

	public void caught()
	{
		anim.SetTrigger ("found");
	}

}
