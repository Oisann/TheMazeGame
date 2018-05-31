using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class AnimationMenu : NetworkBehaviour {


	public GameObject animationPanel;

	public int theAnimValue;

	public int tempAnimValue;

	private AnimMenuButtons animMenuScript;

	public bool animationAvailable = true;

	//Timer for the animation!
	public float elapsedTime;
	public float animRate = 5f;


	// Use this for initialization
	void Start () {

		transform.GetChild (0).gameObject.SetActive (false);
		animMenuScript = transform.GetChild (0).transform.GetChild (0).GetComponent<AnimMenuButtons> ();
	}
	
	// Update is called once per frame
	void Update () {

		if (!animationAvailable) 
		{
			elapsedTime += Time.deltaTime;
		}

		if (elapsedTime > animRate) 
		{
			elapsedTime = 0f;
			animationAvailable = true;
			// to reset the value to zero after the timer/animation is done!
			animMenuScript.OffButton ();
			theAnimValue = tempAnimValue;
		} 


		if (Input.GetKeyDown (KeyCode.C) && animationAvailable && !IngameChat.showChat) {
			animationPanel.SetActive (true);
		} else if (Input.GetKeyUp (KeyCode.C)) 
		{
			theAnimValue = tempAnimValue;
			//Disable the Animation Menu again!
			animationPanel.SetActive (false);

			if (theAnimValue == 0) {
				//print ("No Animation Selected");
				animationAvailable = true;
			} else 
			{
				animationAvailable = false;
			}
		}


		
	}


}
