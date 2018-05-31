using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimMenuButtons : MonoBehaviour {

	private Vector3 originalSize;

	public Vector2 scaleValues;

	public Text buttonText;

	public AnimationMenu animMenuScript;

	//Value 1 - Hello
	//Value 2 - Cheer
	//Value 3 - Hip Hop
	//Value 4 - Over Here
	//Value 5 - Sad
	//Value 6 - Gangnam Style
	//Value 7 - Rest
	//Value 8 - Angry
	//Value 9 - Pray

	[Range(1, 9)]
	public int myValue;

	public int animValue;

	// Use this for initialization
	void Start () {

		animMenuScript = transform.parent.parent.GetComponent<AnimationMenu> ();

		originalSize = transform.localScale;

		buttonText = GetComponentInChildren <Text>();

	}
	


	public void OnButton()
	{
		transform.localScale = originalSize;
		//Just to reset before scaling up again
		transform.localScale = new Vector2 (transform.localScale.x + scaleValues.x, transform.localScale.y + scaleValues.y);
		buttonText.color = Color.red;
		animValue = myValue;
		animMenuScript.tempAnimValue = animValue;
	
		//print (animValue);
	}

	public void OffButton()
	{
		animValue = 0;
		animMenuScript.tempAnimValue = animValue;
		transform.localScale = originalSize;
		buttonText.color = Color.cyan;
	}
}
