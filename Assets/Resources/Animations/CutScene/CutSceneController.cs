using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CutSceneController : MonoBehaviour {

	public GameObject Skip;

	public bool sceneOver;



	void Start(){

	}


	
	// Update is called once per frame
	void Update () {

		if (Input.GetKeyDown (KeyCode.Space) || sceneOver) 
		{
			Skip.SetActive (true);
			Application.LoadLevel ("game");
			gameObject.SetActive (false);
		}



		
	}
}
