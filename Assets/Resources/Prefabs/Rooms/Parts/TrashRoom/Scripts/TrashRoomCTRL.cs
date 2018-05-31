using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashRoomCTRL : MonoBehaviour {

	public List <GameObject> trashList;
	public AudioSource TrashInBin;
	public GameObject trashPS;


	// Use this for initialization
	void Start () {


		for (int i = 1; i < 9; i++) {
			trashList.Add (transform.parent.parent.GetChild(i).gameObject);
		}

	}
	
	// Update is called once per frame
	void Update () {
		if (trashList.Count == 0) {
			transform.parent.parent.parent.GetComponent<RoomController> ().puzzleComplete = true;

		}
	}

	void OnTriggerEnter(Collider other){
		if (other.GetComponent<TrashIdentity>().identityNum == GetComponent<TrashIdentity>().identityNum) {
			
			PickupableObject po = other.gameObject.GetComponent<PickupableObject> ();
			if (po.pickupObject != null) {
				PickupController pc = po.pickupObject.parent.parent.GetComponent<PickupController> ();
				if (pc != null)
					pc.CmdDrop (other.gameObject);
			}

			if(TrashInBin != null)
				TrashInBin.Play ();
			
			GameObject correctTrash = (Instantiate (trashPS, transform.position, Quaternion.identity) as GameObject);
			correctTrash.transform.eulerAngles = new Vector3(-90f, 0f, 0f);
			Destroy (correctTrash, 2f);

			Destroy (other.gameObject, 0.1f);



		}
		for (int i = 0; i < 4; i++) {
			transform.parent.GetChild (i).GetComponent<TrashRoomCTRL> ().trashList.Remove (other.gameObject);
		}
	}
}
