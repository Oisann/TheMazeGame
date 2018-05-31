using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;

public class UnetChat : ChatController {

	private const short chatCode = 132;

	private void Start() {
		//if the client is also the server
		if (NetworkServer.active) {
			//registering the server handler
			NetworkServer.RegisterHandler(chatCode, ServerReceiveMessage);
		}

		//registering the client handler
		NetworkManager.singleton.client.RegisterHandler(chatCode, ReceiveMessage);
	}

	private void ReceiveMessage(NetworkMessage message) {
		//reading message
		string messageValue = message.ReadMessage<StringMessage>().value;

		Message m = JsonUtility.FromJson<Message>(messageValue);


		AddChatMessage(m.username, m.message, m.playerColor);
	}

	private void ServerReceiveMessage(NetworkMessage message) {
		StringMessage myMessage = new StringMessage();
		myMessage.value = message.ReadMessage<StringMessage>().value;

		//sending to all connected clients
		NetworkServer.SendToAll(chatCode, myMessage);
	}

	public override void SendChat(string u, string s, Color c) {
		StringMessage myMessage = new StringMessage();
		Message m = new Message(u, s, c);
		myMessage.value = JsonUtility.ToJson(m);
		//AnalyticsController.DoAction("sendMessage", "Username: " + u + ", Message: " + s + ", Color: " + c.ToString());
		//sending to server
		NetworkManager.singleton.client.Send(chatCode, myMessage);
	}

	private class Message {
		public string username;
		public string message;
		public Color playerColor;

		public Message(string u, string s, Color c) {
			username = u;
			message = s;
			playerColor = c;
		}

	}
}