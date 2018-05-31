using UnityEngine;
using System.Collections.Generic;

public class PortalsWin : MonoBehaviour {
	public List<PlayerSync> playersInTheMiddle = new List<PlayerSync>();
	public AudioSource openDoorSound;

	void OnTriggerEnter(Collider coll) {
		//List of players ingame.
		if(coll.CompareTag("Player")) {
			PlayerSync player = coll.transform.parent.parent.parent.parent.parent.parent.parent.parent.GetComponent<PlayerSync>();
			if (player == null) {
				Debug.LogError("[PortalsWin] No Player Found OnTriggerEnter");
				return;
			}
			if(!playersInTheMiddle.Contains(player))
				playersInTheMiddle.Add(player);


            //Win check
			if(playersInTheMiddle.Count == GameObject.Find("Players").GetComponentsInChildren<PlayerSync>().Length) {
				RoomController rc = transform.parent.parent.parent.GetComponent<RoomController>();
				rc.puzzleComplete = true;
				if(openDoorSound != null)
					openDoorSound.Play();
                foreach(PortalPortals portal in transform.parent.parent.GetComponent<PortalLinker>().allPortalsExceptMiddle) {//transform.parent.parent.GetComponent<PortalLinker>().outside) {
                    if(portal != null) {
                        //Will add a red glow later, atm it didnt work for some reason!
                        portal.forceLinkPortal(null);
                    }
                }
                Destroy(GetComponent<BoxCollider>());
                Destroy(this);
			}
		}
	}

	void OnTriggerExit(Collider coll) {
		if(coll.CompareTag("Player")) {
			PlayerSync player = coll.transform.parent.parent.parent.parent.parent.parent.parent.parent.GetComponent<PlayerSync>();
			if(playersInTheMiddle.Contains(player))
				playersInTheMiddle.Remove(player);
		}
	}
}
