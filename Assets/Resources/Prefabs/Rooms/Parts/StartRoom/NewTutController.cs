using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewTutController : MonoBehaviour {
	public static NewTutController instance;

	//Has the player played this tutorial before?
	public bool hasPlayedBefore = false;
	public string tutorialVersion = "1.0";

    int rotateButtons;
    
	bool Tut0Complete, Tut1Complete, Tut2Complete, Tut3Complete, Tut4Complete, Tut5Complete, Tut6Complete, Tut7Complete, Tut8Complete, Tut9Complete;
    bool aButton, sButton, dButton, wButton;
    
	Transform player;

	public GameObject[] wasdGreens;
	public GameObject[] TutRings;
	public GameObject[] walls;
	public GameObject tutImage;
	public GameObject DINImage;
	public GameObject canvans;
	public GameObject tutSkip;

	public AudioSource DoorDown;
	public AudioSource PuzzleComplete;
	public AudioSource TutRing;

	public Animator tutAnim;

	public PickupableObject PO;

	public string getTutorialVersion() {
		return tutorialVersion;
	}
    
	void Start () {
		instance = this;
		hasPlayedBefore = PlayerPrefs.HasKey(tutorialVersion + "tutorialCompleted");
        aButton = false;
        sButton = false;
        dButton = false;
        wButton = false;
    
		for (int i = 1; i < TutRings.Length; i++) {
			TutRings [i].SetActive (false);
		}

		for (int i = 0; i < wasdGreens.Length; i++) {
			wasdGreens [i].SetActive (false);
		}

		if(!hasPlayedBefore && tutSkip != null) {
			Destroy(tutSkip);
		}
	}

	void Update () {

		if (PO == null) 
		{
			PO = GameObject.FindObjectOfType<PickupableObject> ();
		}

		if (player == null) 
		{
		player = FindObjectOfType<PlayerController> ().transform;
		}
		ImageController ();
		DestroyWalls ();

		if (Input.GetKey(KeyCode.Space) && hasPlayedBefore && !IngameChat.showChat)
        {
			for (int i = 0; i < TutRings.Length; i++) {
				Destroy (TutRings [i]);
			}

			for (int i = 0; i < walls.Length; i++) {
				Destroy (walls [i]);
			}

			Destroy(canvans);
            //Destroy(this);
        }
       
		if (TutRings [0] != null) {
			if (Vector3.Distance (player.transform.position, TutRings [0].transform.position) < 2) {
				DoorDown.Play ();
				TutRing.Play ();
				Invoke ("DestroyDoor1", 5);
				Tut1Complete = true;
				Tut0Complete = true;
			}
		}
			
        if (Tut1Complete)
        {
           
			if (Input.GetKeyDown(KeyCode.A) && !aButton && Vector3.Distance (player.transform.position, TutRings [1].transform.position) < 2)
            {
                aButton = true;
                rotateButtons++;
				wasdGreens [0].SetActive (true);
            }

			if (Input.GetKeyDown(KeyCode.S) && !sButton &&Vector3.Distance (player.transform.position, TutRings [1].transform.position) < 2)
            {
                sButton = true;
                rotateButtons++;
				wasdGreens [1].SetActive (true);
            }

			if (Input.GetKeyDown(KeyCode.D) && !dButton && Vector3.Distance (player.transform.position, TutRings [1].transform.position) < 2)
            {
                dButton = true;
                rotateButtons++;
				wasdGreens [2].SetActive (true);
            }

			if (Input.GetKeyDown(KeyCode.W) && !wButton && Vector3.Distance (player.transform.position, TutRings [1].transform.position) < 2)
            {
                wButton = true;
                rotateButtons++;
				wasdGreens [3].SetActive (true);
            }
        }

		if (TutRings [1] != null) 
		{
			if (rotateButtons == 4 && Vector3.Distance (player.transform.position, TutRings [1].transform.position) < 2) {
				DoorDown.Play ();
				TutRing.Play ();
				Invoke ("DestroyDoor2", 5);
				Tut2Complete = true;
				rotateButtons = 0;


				for (int i = 0; i < wasdGreens.Length; i++) {
					Destroy (wasdGreens [i]);

				}
			}
		}

		if (TutRings [2] != null) 
		{
			if (Input.GetKeyDown (KeyCode.Tab) && Tut2Complete && Vector3.Distance (player.transform.position, TutRings [2].transform.position) < 2) {
				DoorDown.Play ();
				TutRing.Play ();
				Tut3Complete = true;
			}
		}

		if (TutRings [3] != null) 
		{
			if (Input.GetKey (KeyCode.LeftShift) && Input.GetMouseButtonDown (0) && Tut3Complete && Vector3.Distance (player.transform.position, TutRings [3].transform.position) < 2) {
				DoorDown.Play ();
				TutRing.Play ();
				Tut4Complete = true;
			}
		}

		if (TutRings [4] != null)
		{
			if (Input.GetKey(KeyCode.T) && Tut4Complete && Vector3.Distance (player.transform.position, TutRings [4].transform.position) < 2)
	        {
				DoorDown.Play ();
				Tut5Complete = true;
	        }
        }

		if (TutRings [5] != null)
		{
			if (Tut5Complete && Vector3.Distance (player.transform.position, TutRings [5].transform.position) < 7)
			{
				if (PO.pickupObject != null) {
					Tut6Complete = true;
				}
			}
		}


		if (TutRings [6] != null)
		{
			if (Tut6Complete && Vector3.Distance (player.transform.position, TutRings [6].transform.position) < 7)
			{
				if (PO.pickupObject == null) {
					Tut6Complete = false;
					Tut7Complete = true;
				}
			}
		}

		if (TutRings [7] != null)
		{
			if (Tut7Complete && Vector3.Distance (player.transform.position, TutRings [7].transform.position) < 2)
			{
				if (Input.GetKey (KeyCode.C)) {
					Tut8Complete = true;
				}
			}
		}

		if (TutRings [8] != null)
		{
			if (Tut8Complete && Vector3.Distance (player.transform.position, TutRings [8].transform.position) < 2)
			{
				if (Input.GetKey (KeyCode.LeftShift) && Input.GetKey (KeyCode.A) || Input.GetKey (KeyCode.LeftShift) && Input.GetKey (KeyCode.D)) 
				{
					Tut9Complete = true;
				}
			}
		}

	}

	void DestroyWalls()
	{


		if (Tut1Complete) 
		{
			if (walls [0] != null) 
			{
				walls [0].transform.Translate (Vector3.down * Time.deltaTime * 2);
				Invoke ("DestroyDoor1", 5);
				Destroy(TutRings[0]);
				if (TutRings [1] != null) {
					TutRings [1].SetActive (true);

				}
			}
		}
			
		if (Tut2Complete) 
		{
			if (walls [1] != null) 
			{
				walls [1].transform.Translate (Vector3.down * Time.deltaTime * 2);
				Invoke ("DestroyDoor2", 5);
				Destroy(TutRings[1]);
				if (TutRings [2] != null) {
					TutRings [2].SetActive (true);
				}
			}
		}

		if (Tut3Complete) 
		{
			if (walls [2] != null) 
			{
				walls [2].transform.Translate (Vector3.down * Time.deltaTime * 2);
				Invoke ("DestroyDoor3", 5);
				Destroy(TutRings[2]);
				if (TutRings [3] != null) {
					TutRings [3].SetActive (true);
				}
			}
		}

		if (Tut4Complete) 
		{
			if (walls [3] != null) 
			{
				walls [3].transform.Translate (Vector3.down * Time.deltaTime * 2);
				Invoke ("DestroyDoor4", 5);
				Destroy(TutRings[3]);
				if (TutRings [4] != null) {
					TutRings [4].SetActive (true);
				}
			}
		}

		if (Tut5Complete) 
		{
			if (walls [4] != null) 
			{
				walls [4].transform.Translate (Vector3.down * Time.deltaTime * 2);
				Invoke ("DestroyDoor5", 5);
				Destroy(TutRings[4]);
				if (TutRings [5] != null) {
					TutRings [5].SetActive (true);
				}
			}
		}

		if (Tut6Complete) 
		{
			if (TutRings [5] != null) {
				Destroy (TutRings [5]);
				TutRings [6].SetActive (true);
			}
		}

		if (Tut7Complete) 
		{
			if (TutRings [6] != null) {
				Destroy (TutRings [6]);
				TutRings [7].SetActive (true);
			}
		}

		if (Tut8Complete) 
		{
			if (TutRings [7] != null) {
				Destroy (TutRings [7]);
				TutRings [8].SetActive (true);

			}
		}

		if (Tut9Complete) 
		{
			if (TutRings [8] != null) {
				tutAnim.SetInteger ("ChangeTut", 9);

				//Saves the fact that you have completed this tutorial once
				PlayerPrefs.SetInt(tutorialVersion + "tutorialCompleted", 1);
				PlayerPrefs.Save();

				tutImage.SetActive (true);
				Invoke ("EndTutorial", 5);
				Destroy (TutRings [8]);

			}
		}

	}

	void DestroyDoor1()
	{
		Destroy(walls[0]);

	}

	void DestroyDoor2()
	{
		Destroy(walls[1]);
	}

	void DestroyDoor3()
	{
		Destroy(walls[2]);
	}

	void DestroyDoor4()
	{
		Destroy(walls[3]);
	}

	void DestroyDoor5()
	{
		Destroy(walls[4]);
	}

	void EndTutorial()
	{
		Destroy(canvans);
		//Destroy(this);
	}

	void ImageController()
	{

		if (Tut1Complete) {
			//Change to WASD image
			if (TutRings [1] != null) 
			{
				if (Vector3.Distance (player.transform.position, TutRings [1].transform.position) < 2) {
					tutAnim.SetInteger ("ChangeTut", 1);
					tutImage.SetActive (true);
					DINImage.SetActive (true);
				} else {
					tutImage.SetActive (false);
					DINImage.SetActive (false);

				}
			} 
			//Change to map image
			else if (TutRings [2] != null) 
			{
				if (Vector3.Distance (player.transform.position, TutRings [2].transform.position) < 2) {
					tutAnim.SetInteger ("ChangeTut", 2);
					tutImage.SetActive (true);
					DINImage.SetActive (true);
				} else {
					tutImage.SetActive (false);
					DINImage.SetActive (false);
				}
			} 
			//Change to ping image
			else if (TutRings [3] != null) 
			{
				if (Vector3.Distance (player.transform.position, TutRings [3].transform.position) < 2) {
					tutAnim.SetInteger ("ChangeTut", 3);
					tutImage.SetActive (true);
					DINImage.SetActive (true);
				} else {
					tutImage.SetActive (false);
					DINImage.SetActive (false);
				}
			} 
			//Change to chat image
			else if (TutRings [4] != null) 
			{
				if (Vector3.Distance (player.transform.position, TutRings [4].transform.position) < 2) {
					tutAnim.SetInteger ("ChangeTut", 4);
					tutImage.SetActive (true);
					DINImage.SetActive (true);
				} else {
					tutImage.SetActive (false);
					DINImage.SetActive (false);
				}
			} 
			//Change to pickup image
			else if (TutRings [5] != null) 
			{
				if (Vector3.Distance (player.transform.position, TutRings [5].transform.position) < 7) {
					tutAnim.SetInteger ("ChangeTut", 5);
					tutImage.SetActive (true);
					DINImage.SetActive (true);
				} else {
					tutImage.SetActive (false);
					DINImage.SetActive (false);
				}
			}

			else if (TutRings [6] != null) 
			{
				//Change to drop image
				if (Vector3.Distance (player.transform.position, TutRings [6].transform.position) < 7) {
					tutAnim.SetInteger ("ChangeTut", 6);
					tutImage.SetActive (true);
					DINImage.SetActive (true);
				} else {
					tutImage.SetActive (false);
					DINImage.SetActive (false);
				}
			}

			else if (TutRings [7] != null) 
			{
				//Change to motion image
				if (Vector3.Distance (player.transform.position, TutRings [7].transform.position) < 2) {
					tutAnim.SetInteger ("ChangeTut", 7);
					tutImage.SetActive (true);
					DINImage.SetActive (true);
				} else {
					tutImage.SetActive (false);
					DINImage.SetActive (false);
				}
			}

			else if (TutRings [8] != null) 
			{
				//Change to motion image
				if (Vector3.Distance (player.transform.position, TutRings [8].transform.position) < 2) {
					tutAnim.SetInteger ("ChangeTut", 8);
					tutImage.SetActive (true);
					DINImage.SetActive (true);
				} else {
					tutImage.SetActive (false);
					DINImage.SetActive (false);
				}
			}

			if (TutRings.Length == 0) 
		    {
				PuzzleComplete.Play();
			}
		}
	}
}
