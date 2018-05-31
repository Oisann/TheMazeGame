using UnityEngine;
using UnityEngine.Networking;

public class ChatController : NetworkBehaviour {

	[SerializeField]
	private IngameChat chat;

	internal void AddChatMessage(string u, string s, Color c) {
		chat.AddMessage(u, s, c);
	}

	public virtual void SendChat(string u, string s, Color c) {}
}