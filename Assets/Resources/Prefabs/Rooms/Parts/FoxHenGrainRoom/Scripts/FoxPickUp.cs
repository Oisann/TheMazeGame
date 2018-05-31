using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoxPickUp : MonoBehaviour {

	public bool canPlace;
	public bool pickupFlag;

	// Use this for initialization
	void Start()
	{
		canPlace = true;
		pickupFlag = true;
	}

	void Update(){
	

			PickUpStress ();

		if (GetComponent<PickupableObject> ().pickupObject == null)
			pickupFlag = true;

	}
	void PickUpStress()
	{	
		if (GetComponent<PickupableObject> ().pickupObject != null && pickupFlag) {
			canPlace = false;
			Invoke ("InvokeCanPlace", 3); 
		}
			
	}
	void InvokeCanPlace(){
	
		canPlace = true;
		pickupFlag = false;

	}
}
