using UnityEngine;
using UnityEngine.Networking;

public class WinLaser : NetworkBehaviour {
	private bool opened = false;

	public void OpenRoom() {
		if(!isServer)
			return;
		if(!opened) {
			Debug.Log("Puzzle done");
			transform.parent.GetComponent<RoomController>().puzzleComplete = true;
		}
	}
}
