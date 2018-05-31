using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class Lobby : MonoBehaviour {
	public SceneField Offline;

	private NetworkManager manager;
	private PlayerController pc;
	private bool startedHosting = false;
	private GameObject HUD;

	void Awake() {
		manager = GetComponent<NetworkManager>();
		HUD = GameObject.Find("HUD");
		HUD.SetActive(false);
		SceneManager.LoadSceneAsync(Offline, LoadSceneMode.Additive);
		//Invoke("StartHost", 5f);
	}

	void FixedUpdate() {
		if(pc == null && startedHosting)
			pc = GameObject.FindObjectOfType<PlayerController>();

		if (pc != null && startedHosting) {
			pc.Kill();
			pc = null;
			startedHosting = false;
		}
	}

	public void StartHosting() {
		HUD.SetActive(true);
		manager.StartHost();
		SceneManager.UnloadSceneAsync(Offline);
		startedHosting = true;
	}

	public void ConnectTo(string address, int port = 7777) {
		if(string.IsNullOrEmpty(address))
			address = "localhost";
		manager.networkAddress = address;
		manager.networkPort = port;

		HUD.SetActive(true);
		manager.StartClient();
		SceneManager.UnloadSceneAsync(Offline);
	}

	public NetworkManager getManager() {
		return manager;
	}
}
