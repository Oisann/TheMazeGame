using UnityEngine;
using UnityEngine.UI;

public class LobbyLinker : MonoBehaviour {
	public InputField IF;

	private Lobby lobby;

	void Update() {
		if(lobby == null)
			lobby = GameObject.FindObjectOfType<Lobby>();

		//Check if we found one
		if(lobby == null)
			Debug.Log("Still no NetworkManager found...");
	}

	public void Host() {
		if(lobby == null) {
			Debug.LogError("No NetworkManager found");
			return;
		}

		lobby.StartHosting();
	}

	public void ConnectToField() {
		Connect(IF.text); 
	}

	public void Connect(string field) {
		if(lobby == null) {
			Debug.LogError("No NetworkManager found");
			return;
		}

		if (field.Contains(":")) {
			int port = 7777;
			int.TryParse(field.Split(":".ToCharArray())[1], out port);
			string ip = field.Split(":".ToCharArray())[0];
			lobby.ConnectTo(ip, port);
		} else {
			lobby.ConnectTo(field);
		}
	}
}
