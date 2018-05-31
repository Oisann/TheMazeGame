using UnityEngine;

public class DisplayLobbyRoom : MonoBehaviour {

	public int currentRoom = 0;

	void Start() {
		InvokeRepeating("UpdateRoom", 0f, 30f);
	}

	void UpdateRoom() {
		currentRoom = Random.Range(0, transform.childCount);
		for(int i = 0; i < transform.childCount; i++) {
			Transform child = transform.GetChild(i);
			child.gameObject.SetActive(currentRoom == i);
		}
	}
}
