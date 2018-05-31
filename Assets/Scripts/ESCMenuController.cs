using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.Networking;

public class ESCMenuController : MonoBehaviour {

    public bool open, closed;
    public GameObject escMenu;

    public GameObject exitOP;
    public GameObject soundOP;
    public GameObject renderDistanceOP;
    private GameObject renderDist;
    public Slider renderDistanceSlider;
    public float dist = 150f;
    public Text renderValueText;
	public AudioMixerSnapshot Master;
	public AudioMixerSnapshot NoVolume;
	public bool muted;
	public Toggle muteOption;
	public AudioMixer mainMixer;
	public float masterLvl;
	public Slider volumeSlider;

    private Cameraman camMan;
    
    void Start() {
        open = false;
        closed = true;
		muted = false;
    }
    
    void Update() {
        if(camMan == null) {
            camMan = GameObject.FindObjectOfType<Cameraman>();
            if(camMan != null) {
                camMan.renderDistance = dist;
            } else {
                return;
            }
        } else {
            camMan.renderDistance = dist;
        }

        //WHY?!?!?!
        //GameObject.FindObjectOfType<Cameraman> ().renderDistance = dist;

        dist = renderDistanceSlider.value;

        renderValueText.text = dist.ToString();

		if(Input.GetKeyDown(KeyCode.Escape) && !IngameChat.showChat) {
            escMenu.SetActive(true);
            open = true;
        }

        if(Input.GetKeyUp(KeyCode.Escape) && open) {
            closed = false;
        }

        if(Input.GetKeyDown(KeyCode.Escape) && !closed) {
            open = false;
            escMenu.SetActive(false);
            closed = true;

            //Close Everything previously open
            exitOP.SetActive(false);
            soundOP.SetActive(false);
            renderDistanceOP.SetActive(false);
        }

		masterLvl = volumeSlider.value;
		Master.audioMixer.SetFloat ("Master", masterLvl);

		if (muteOption.isOn) {
			NoVolume.TransitionTo (.01f);
			muted = true;
			volumeSlider.value = -80;
			volumeSlider.enabled = false;
		} else if(!muteOption.isOn)
		{
			muted = false;
			Master.TransitionTo (.01f);
			volumeSlider.enabled = true;
		}



    }
    // Exit!

    public void LoadExit() {
        exitOP.SetActive(true);
    }

    public void ExitYes() {
        //...
        //Application.LoadLevel ("MainMenu");
		GameObject.FindObjectOfType<NetworkManager>().StopHost();
		GameObject.FindObjectOfType<NetworkManager>().StopClient();
        SceneManager.LoadScene("MainMenu");
    }

    public void ExitNo() {
        exitOP.SetActive(false);
    }


    // Sound!

    public void LoadSound() {
        soundOP.SetActive(true);
    }

    public void CloseSound() {
        soundOP.SetActive(false);
    }

    // Graphics

    public void LoadGraphics() {
        renderDistanceOP.SetActive(true);
    }

    public void CloseGraphics() {
        renderDistanceOP.SetActive(false);
    }

    public void Restore() {
        renderDistanceSlider.value = 150f;
    }

	public void Mute()
	{
		if (!muted) {
			muted = true;
		} else if (muted)
		{
			muted = false;
		}
	}

	public void setMasterVol()
	{
		Master.audioMixer.SetFloat ("Master", masterLvl);
	}

	public void RestoreSound()
	{
		if (!muted) {
			volumeSlider.value = -14f;
		} else 
		{
			muted = false;
			muteOption.isOn = false;
			volumeSlider.value = -14f;
		}

	}


	public void CloseMenu()
	{
		open = false;
		escMenu.SetActive(false);
		closed = true;

		//Close Everything previously open
		exitOP.SetActive(false);
		soundOP.SetActive(false);
		renderDistanceOP.SetActive(false);
	}
}
