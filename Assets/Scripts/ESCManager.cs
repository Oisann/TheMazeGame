using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ESCManager : MonoBehaviour {

	public Button button;

	private Vector3 originalSize;

	public Vector2 scaleValues;

	// Use this for initialization
	void Start () {

		originalSize = transform.localScale;
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void OnButton()
	{
		transform.localScale = originalSize;
		//Just to reset before scaling up again
		transform.localScale = new Vector2 (transform.localScale.x + scaleValues.x, transform.localScale.y + scaleValues.y);

	}

	public void OffButton()
	{
		transform.localScale = originalSize;
	}

}
