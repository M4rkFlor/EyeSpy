using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorScript : MonoBehaviour {
	public string NextStage;

	void OnTriggerEnter() {
		SceneManager.LoadScene (NextStage);
	}
}
