using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NumSequenceController : MonoBehaviour 
{

	public GameObject numberTile1, numberTile2, numberTile3;

	public int number1, number2, number3;

	public Mesh[] numberMesh;

	public int tarNumb;
	public int parsNum;
	public GameObject winscreen;

	GameObject[] numberPads;

	void Start()
	{
		winscreen.SetActive (false);
		number1 = Random.Range (0, 10);
		number2 = Random.Range (0, 10);
		number3 = Random.Range (0, 10);

		numberTile1.GetComponent<MeshFilter> ().mesh = numberMesh [number1];
		numberTile2.GetComponent<MeshFilter> ().mesh = numberMesh [number2];
		numberTile3.GetComponent<MeshFilter> ().mesh = numberMesh [number3];

		tarNumb = number1 + number2 + number3;

	}

	void OnTriggerEnter(Collider other)
	{
		if (gameObject.tag == "AnswerBox")
			{

			int.TryParse (other.gameObject.name.ToString(), out parsNum);

			if (parsNum == tarNumb) 
			{
				print ("DoorOpen");
				//hello
				winscreen.SetActive (true);
			}
		}
	}
}
