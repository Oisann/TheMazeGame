using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class LeverRoomController : MonoBehaviour {

	private GameObject child;
	public List<Transform> leverSpawnPoints = new List<Transform>();
	public int playerCount;
	public GameObject lever;
	private System.Random rand;

	// Use this for initialization
	void Start () {
		
		child = transform.GetChild(1).gameObject;

		for(int i = 0; i < child.transform.childCount; i++)
		{
			leverSpawnPoints.Add(child.transform.GetChild(i).transform);
		}

		playerCount = GameObject.FindObjectsOfType<PlayerSync>().Length;
		rand = new System.Random(transform.parent.name.GetHashCode());

		for (int i = 0; i < playerCount; i++) 
		{
			GameObject s = Instantiate (lever) as GameObject;
			s.transform.SetParent(transform);
			Transform temp = leverSpawnPoints [rand.Next (0, leverSpawnPoints.Count)];
			s.transform.position = temp.position;
			s.transform.rotation = temp.rotation;
			leverSpawnPoints.Remove (temp);
			NetworkServer.Spawn (s);
		}
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
