using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossScript : MonoBehaviour {
	public float floatheight;
	int dir;
	float timer;
	public float spwnTime = 1;
	float startY;
	public GameObject doggos;
	// Use this for initialization
	void Start () {
		startY = transform.position.y;
		dir = 1;
	}
	
	// Update is called once per frame
	void Update () {
		timer += Time.deltaTime;
		transform.Translate (0, 4 * dir * Time.deltaTime, 0);
		if (startY + floatheight < transform.position.y) {
			dir = -1;
		} else if (startY - floatheight > transform.position.y) {
			dir = 1;
		}

		if (timer > spwnTime) {
			timer = 0;
			Instantiate (doggos, transform.position, transform.rotation);
		}
	}
}
