using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteDoors : MonoBehaviour {

	private bool hasDeleted = false;

	void FixedUpdate() {
		if(!hasDeleted) {
			Transform parent = transform.parent;
			int del = 0;
			foreach(DoorController dc in parent.GetComponentsInChildren<DoorController>()) {
				if(dc.type == DoorController.DoorType.wall) {
					del++;
					DestroyImmediate(dc.gameObject);
				}
			}
			if(del > 0) {
				hasDeleted = true;
			}
		}
	}
}
