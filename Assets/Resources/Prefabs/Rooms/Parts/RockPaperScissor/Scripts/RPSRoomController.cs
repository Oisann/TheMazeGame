using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RPSRoomController : MonoBehaviour {

	public bool rock,paper,scissors;

	private GameObject firstTray;

	public GameObject[] interactbleTrays;

	private GameObject tray1;
	private GameObject tray2;
	private GameObject tray3;

	public bool placed;
	private bool drop;


	public int score = 0;



	// Use this for initialization
	void Start () {

		firstTray = GameObject.Find ("InteractableTrays");


		for(int i = 0; i < firstTray.transform.childCount; i++)
		{
			interactbleTrays[i] = firstTray.transform.GetChild(i).gameObject;
		}

		tray1 = interactbleTrays [0].gameObject;
		tray2 = interactbleTrays [1].gameObject;
		tray3 = interactbleTrays [2].gameObject;

		tray1.transform.parent = firstTray.transform;
		tray1.transform.parent = firstTray.transform;
		tray1.transform.parent = firstTray.transform;
		
	}
	
	// Update is called once per frame
	void Update () {
		

		Place (tray1);
		Place (tray2);
		Place (tray3);

		if (score == 3) 
		{
			print ("Door opens!");	
		}
		
	}


	public void Place(GameObject obj)
	{
		if (!placed) {

			if (Vector3.Distance (transform.localPosition, obj.transform.localPosition) < 5f && !placed) {
				print ("placing");
				gameObject.transform.localPosition = obj.transform.localPosition;



				//Letting go
				if (GetComponent<PickupableObject> () != null && !drop) {
					if (GetComponent<PickupableObject> ().pickupObject != null && !placed && !drop) {
						GetComponent<PickupableObject> ().pickupObject.parent.parent.GetComponent<PickupController> ().CmdDrop (gameObject);
					}

					transform.localRotation = Quaternion.Euler (0f, 0f, 0f);


					// Placing correctly
					if (scissors == true) 
					{
						transform.localPosition = new Vector3 (transform.localPosition.x +.79f, -1f, transform.localPosition.z);
					}

					if (rock == true) 
					{
						transform.localPosition = new Vector3 (transform.localPosition.x, -1f, transform.localPosition.z);
					}

					if (paper == true) 
					{
						transform.localPosition = new Vector3 (transform.localPosition.x + 1.15f, -1f, transform.localPosition.z);
					}


					GetComponent<Rigidbody> ().isKinematic = true;
					GetComponent<PickupableObject> ().enabled = false;
					placed = true;
					drop = true;
				}
			} else {
				placed = false;
			}


		} else 
		{
			GetComponent<Rigidbody> ().isKinematic = true;
		}
	}

	public void AddScore()
	{/*
		tray1.GetComponent<RPSManager> ().AddScore ();
		print (tray1.GetComponent<RPSManager> ().score);
		*/
	}

}
