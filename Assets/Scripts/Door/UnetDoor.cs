using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;

public class UnetDoor : DoorOpener {

	private const short doorOpenCode = 131;

	private void Start() {
		//if the client is also the server
		if (NetworkServer.active) {
			//registering the server handler
			NetworkServer.RegisterHandler(doorOpenCode, ServerReceiveMessage);
		}

		//registering the client handler
		NetworkManager.singleton.client.RegisterHandler (doorOpenCode, ReceiveMessage);
	}

	private void ReceiveMessage(NetworkMessage message) {
		//reading message
		string text = message.ReadMessage<StringMessage> ().value;

		ActivateRoom(text);
	}

	private void ServerReceiveMessage(NetworkMessage message) {
		StringMessage myMessage = new StringMessage();
		myMessage.value = message.ReadMessage<StringMessage>().value;

		//sending to all connected clients
		NetworkServer.SendToAll (doorOpenCode, myMessage);
	}

	public override void OpenDoor(string pos) {
		StringMessage myMessage = new StringMessage ();
		myMessage.value = pos;

		//sending to server
		NetworkManager.singleton.client.Send (doorOpenCode, myMessage);
	}
}