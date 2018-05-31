using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FoxBridgeCTRL : MonoBehaviour {

	public FoxTrayScript startBox;
	public FoxTrayScript targetBox;

    //public Text loseText;
    //public GameObject resetButton;

    void Start()
    {
        //Time.timeScale = 1;
        //resetButton.SetActive(false);
    }

	void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player") 
		{
            if (startBox.inside.Contains(startBox.foxObj) && startBox.inside.Contains(startBox.henObj))
            {
                print("Uff da hen");
                //loseText.text = "The hen was eaten by the fox";
                //resetButton.SetActive(true);
                //Time.timeScale = 0;
            }
            if (startBox.inside.Contains(startBox.grainObj) && startBox.inside.Contains(startBox.henObj))
            {
                print("Uff da grain");
                //loseText.text = "The grain was eaten by the hen";
                //resetButton.SetActive(true);
                //Time.timeScale = 0;
            }
            if (targetBox.inside.Contains(targetBox.foxObj) && targetBox.inside.Contains(targetBox.henObj))
            {
                print("Uff da hen");
                //loseText.text = "The hen was eaten by the fox";
                //resetButton.SetActive(true);
                //Time.timeScale = 0;
            }

            if (targetBox.inside.Contains(targetBox.grainObj) && targetBox.inside.Contains(targetBox.henObj))
            {
                print("Uff da grain");
                //loseText.text = "The grain was eaten by the hen";
                //resetButton.SetActive(true);
                //Time.timeScale = 0;
            }
        }
	}

    public void ResetLevel()
    {
        SceneManager.LoadScene("FoxRoom");
    }
}
