using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour {

	public Button playGame, loadTest;
	public AudioSource selectSound;


    
	public void LoadScene(string scene) {
        SceneManager.LoadScene(scene);
    }

	// For button selection
	public void OnPlayButton()
	{
		selectSound.Play ();
		playGame.transform.localScale = new Vector3 (1.026819f, 2.750344f, 1.026819f);
	}

	public void OffPlayButton()
	{
		playGame.transform.localScale = new Vector3 (0.8510708f, 2.2796f, 0.8510708f);
	}

	//////////////
	public void OnLoadTestButton()
	{
		selectSound.Play ();
		loadTest.transform.localScale = new Vector3 (1.308889f, 2.58962f, 0.9722857f);
	}

	public void OffLoadTestButton()
	{
		loadTest.transform.localScale = new Vector3 (1.145709f, 2.266772f, 0.8510708f);
	}
}
