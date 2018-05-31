using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class RPSDetection : NetworkBehaviour {

	public bool paperTray, rockTray, scissorTray;

	void Start()
	{
		//print (isServer);
	}
		

	void OnTriggerEnter(Collider other)
	{
		if (!isServer) 
		{
			return;
		}
			

		if (other.GetComponent<RPSRoomController> () == null)
			return;


        //Winning conditions!
		if (other.GetComponent<RPSRoomController> ().paper && paperTray) 
		{
			print ("Correct!");
			other.GetComponent<RPSRoomController> ().AddScore ();
		}

		if (other.GetComponent<RPSRoomController> ().rock && rockTray) 
		{
			print ("Correct!");
			other.GetComponent<RPSRoomController> ().AddScore ();
		}

		if (other.GetComponent<RPSRoomController> ().scissors && scissorTray) 
		{
			print ("Correct!");
			other.GetComponent<RPSRoomController> ().AddScore ();
		}

	}

}
