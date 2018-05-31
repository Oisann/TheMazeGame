using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RPSManager : MonoBehaviour {

	public int score = 0;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		if (score == 3) 
		{
			print("Won!");

		}
		
	}


	public void AddScore()
	{
		score = score + 1;
	}
}
