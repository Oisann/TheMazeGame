using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Compass : MonoBehaviour {
	public bool local = false;

	private Cameraman cameraman;

	void Start() {
		foreach(PlayerController player in GameObject.FindObjectsOfType<PlayerController>()) {
			if(player.isLocal()) {
				cameraman = player.getCameraman();
			}
		}
	}

	void Update() {
		//Sometimes it cant find the camera in the start function, so we have to double check
		if(cameraman == null) {
			foreach(PlayerController player in GameObject.FindObjectsOfType<PlayerController>()) {
				if(player.isLocal()) {
					cameraman = player.getCameraman();
				}
			}
		} else {
			if(local) {
				transform.localEulerAngles = new Vector3(0f, 0f, cameraman.RotationsTransform.eulerAngles.y);
			} else {
				transform.eulerAngles = new Vector3(0f, 0f, cameraman.RotationsTransform.eulerAngles.y);
			}
		}
	}

	public Cameraman getCameraman() {
		return cameraman;
	}
}
