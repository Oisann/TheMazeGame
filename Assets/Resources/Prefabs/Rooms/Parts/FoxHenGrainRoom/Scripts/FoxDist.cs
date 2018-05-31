using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoxDist : MonoBehaviour {

	public Transform foxStart, henStart, grainStart;
	public Transform foxTar, henTar, grainTar;
	public Transform fox, hen, grain;
	GameObject playerPickup;

	public List<GameObject> insideStart = new List<GameObject>();
	public List<GameObject> insideTarget = new List<GameObject>();
	public bool canPlace;
	//public FoxPickUp foxPickup;


	// Use this for initialization
	void Start () {
		canPlace = true;
	}
	
	// Update is called once per frame
	void Update () {
		if (fox == null)
			fox = transform.parent.Find ("Fox");
		if (hen == null)
			hen = transform.parent.Find ("Hen");
		if (grain == null)
			grain = transform.parent.Find ("Grain");

		placeThing (fox, foxStart, foxTar);

		placeThing (hen, henStart, henTar);

		placeThing (grain, grainStart, grainTar);

		if (insideTarget.Count == 3) {
			transform.parent.parent.GetComponent<RoomController> ().puzzleComplete = true;

		}
			

	}



	void placeThing(Transform thing, Transform start, Transform target){

		if (thing.GetComponent<PickupableObject> ().pickupObject != null) {
			canPlace = thing.GetComponent<FoxPickUp> ().canPlace;
		}

		if (Vector3.Distance(thing.position, start.position) <= 2)
		{
			
			if (!canPlace)
				return;
			thing.transform.position = start.transform.position;
			PickupableObject po = thing.gameObject.GetComponent<PickupableObject> ();
			if (po.pickupObject != null) {
				PickupController pc = po.pickupObject.parent.parent.GetComponent<PickupController> ();
				if (pc != null)
					pc.CmdDrop (thing.gameObject);
			}
			if (insideStart.Contains (thing.gameObject))
				return;
			insideStart.Add (thing.gameObject);


		}

		if (Vector3.Distance(thing.position, target.position) <= 2) 
		{
			if (!canPlace)
				return;
			thing.transform.position = target.transform.position;
			PickupableObject po = thing.gameObject.GetComponent<PickupableObject> ();
			if (po.pickupObject != null) {
				PickupController pc = po.pickupObject.parent.parent.GetComponent<PickupController> ();
				if (pc != null)
					pc.CmdDrop (thing.gameObject);
			}

			if (insideTarget.Contains (thing.gameObject))
				return;
			insideTarget.Add (thing.gameObject);



		}

		if (Vector3.Distance(thing.position, start.position) >= 2)
		{
			insideStart.Remove (thing.gameObject);
		}

		if (Vector3.Distance(thing.position, target.position) >= 2)
		{
			insideTarget.Remove (thing.gameObject);
		}

		/*if (thing.position != start.position && thing.position != target.position && GetComponent<PickupableObject>().pickupObject == null) {

			thing.position = start.position;
		}*/
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.layer == LayerMask.NameToLayer ("Interactable")) {
			if (insideStart.Count == 3)
				return;
			
			if (insideStart.Contains (fox.gameObject) && insideStart.Contains (hen.gameObject)) {
				//Hen Got eaten
				PickupableObject po = other.gameObject.GetComponent<PickupableObject> ();
				if (po.pickupObject != null) {
					PickupController pc = po.pickupObject.parent.parent.GetComponent<PickupController> ();
					if (pc != null)
						pc.CmdDrop (other.gameObject);
				}
				hen.position = henStart.transform.position;
				fox.position = foxStart.transform.position;
				grain.position = grainStart.transform.position;

		
			}
			if (insideStart.Contains (grain.gameObject) && insideStart.Contains (hen.gameObject)) {
				//Grain got eaten
				PickupableObject po = other.gameObject.GetComponent<PickupableObject> ();
				if (po.pickupObject != null) {
					PickupController pc = po.pickupObject.parent.parent.GetComponent<PickupController> ();
					if (pc != null)
						pc.CmdDrop (other.gameObject);
				}
				hen.position = henStart.transform.position;
				fox.position = foxStart.transform.position;
				grain.position = grainStart.transform.position;
			}
			if (insideTarget.Contains (fox.gameObject) && insideTarget.Contains (hen.gameObject)) {
				//Hen got eaten
				PickupableObject po = other.gameObject.GetComponent<PickupableObject> ();
				if (po.pickupObject != null) {
					PickupController pc = po.pickupObject.parent.parent.GetComponent<PickupController> ();
					if (pc != null)
						pc.CmdDrop (other.gameObject);
				}
				hen.position = henStart.transform.position;
				fox.position = foxStart.transform.position;
				grain.position = grainStart.transform.position;
			}

			if (insideTarget.Contains (grain.gameObject) && insideTarget.Contains (hen.gameObject)) {
				//Grain got eaten
				PickupableObject po = other.gameObject.GetComponent<PickupableObject> ();
				if (po.pickupObject != null) {
					PickupController pc = po.pickupObject.parent.parent.GetComponent<PickupController> ();
					if (pc != null)
						pc.CmdDrop (other.gameObject);
				}
				hen.position = henStart.transform.position;
				fox.position = foxStart.transform.position;
				grain.position = grainStart.transform.position;

			}
		}
	}
}
