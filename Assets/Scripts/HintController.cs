using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HintController : MonoBehaviour
{

	public Sprite[] hintSprite;
	RoomController rc;
	public Image im;
	bool laserFlag, buttonFlag, trashFlag, weightFlag, foxFlag, portalFlag;

	void Start ()
	{
		rc = transform.GetComponentInParent<RoomController> ();
		im = GameObject.Find ("HintImage").GetComponent<Image> ();
	}

	void Update ()
	{
		if (rc.currentRoom == null) {
			return;
		} 
			
		if (rc.currentRoom.name == "StartRoom") {
			im.CrossFadeAlpha (0, .5f, false);

		}
		if (rc.currentRoom.name == "LaserRoom") {
			im.CrossFadeAlpha (2, .5f, false);
			if (!laserFlag) {
				im.sprite = hintSprite [0];
			}
			if (Input.GetKey (KeyCode.Space)) {
				im.CrossFadeAlpha (0, .5f, false); 
				Destroy (this);
			}
			laserFlag = true;
		}
		if (rc.currentRoom.name == "Buttons") {
			im.CrossFadeAlpha (2, .5f, false);
			if (!buttonFlag) {
				im.sprite = hintSprite [1];
			}
			if (Input.GetKey (KeyCode.Space)) {
				im.CrossFadeAlpha (0, .5f, false); 
				Destroy (this);
			}
			buttonFlag = true;
		}
		if (rc.currentRoom.name == "TrashRoom") {
			im.CrossFadeAlpha (2, .5f, false);
			if (!trashFlag) {
				im.sprite = hintSprite [2];
			}
			if (Input.GetKey (KeyCode.Space)) {
				im.CrossFadeAlpha (0, .5f, false); 
				Destroy (this);
			}
			trashFlag = true;
		}
		if (rc.currentRoom.name == "WeightRoom") {
			im.CrossFadeAlpha (2, .5f, false);
			if (!weightFlag) {
				im.sprite = hintSprite [3];
			}
			if (Input.GetKey (KeyCode.Space)) {
				im.CrossFadeAlpha (0, .5f, false); 
				Destroy (this);
			}
			weightFlag = true;
		}
		if (rc.currentRoom.name == "FoxHenGrainRoom") {
			im.CrossFadeAlpha (2, .5f, false);
			if (!foxFlag) {
				im.sprite = hintSprite [4];
			}
			if (Input.GetKey (KeyCode.Space)) {
				im.CrossFadeAlpha (0, .5f, false); 
				Destroy (this);
			}
			foxFlag = true;
		}
		if (rc.currentRoom.name == "Portals") { 
			im.CrossFadeAlpha (2, .5f, false);
			if (!portalFlag) {
				im.sprite = hintSprite [5];
			}
			if (Input.GetKey (KeyCode.Space)) {
				im.CrossFadeAlpha (0, .5f, false);
				Destroy (this);
			}
			portalFlag = true;
		}
	}
}
