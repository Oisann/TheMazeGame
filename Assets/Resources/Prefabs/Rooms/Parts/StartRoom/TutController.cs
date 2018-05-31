using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutController : MonoBehaviour {

	public float distance = 50f;
	public GameObject tutathan;

	// Use this for initialization
	void Start () {
		//if (tutathan == null) {
			//tutathan = GameObject.FindObjectOfType<CloseMe> ().gameObject;
			tutathan.SetActive (false);
		//}
	}
	
	// Update is called once per frame
	void Update () {
		//if mouse button (left hand side) pressed instantiate a raycast
		if(Input.GetMouseButtonDown(0))
		{
			//create a ray cast and set it to the mouses cursor position in game
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			if (Physics.Raycast (ray, out hit, distance)) {
				if (hit.collider.tag == "TutBlock") {
					tutathan.SetActive (true);
				}
			}    
		}    
	}

	public void CloseTut(){
		tutathan.SetActive (false);
	}

}
