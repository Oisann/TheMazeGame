using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ButtonRoomController : NetworkBehaviour {

	private MeshRenderer middle;
	[SyncVar]
	private Color middleColor = Color.red;
	private List<ButtonScript> pressedButtons = new List<ButtonScript>();
	private System.Random rand;
	private RoomController rc;
	private List<ButtonScript> buttons = new List<ButtonScript>();
	private int playerCount, buttonsCount = 0, pressedButtonsCount = 0;
	private ParticleSystem ps;
	public AudioSource openDoorSound;

	void Start() {
		playerCount = GameObject.Find("Players").GetComponentsInChildren<PlayerSync>().Length;
		if (isServer) {
			rc = transform.parent.parent.GetComponent<RoomController> ();
			rand = new System.Random (transform.parent.name.GetHashCode ());
		}
		middle = transform.GetChild(1).GetComponent<MeshRenderer>();
		ps = transform.GetChild(0).GetComponent<ParticleSystem>();
	}

	void Update() {

		if(middle != null) {
			Color glass = middleColor;
			glass.a = (1f / 256f) * 127f;
			middle.material.color = glass;
		}

		if(ps != null) {
			if(middleColor == Color.green && !ps.isPlaying) {
				ps.Play();

			}
		}

		if(!isServer)
			return;

		if(buttonsCount != buttons.Count) {
			List<ButtonScript> buttonsToPick = new List<ButtonScript>();
			ButtonScript[] temp = new ButtonScript[buttons.Count];
			buttons.CopyTo(temp);
			buttonsToPick.AddRange(temp);

			for(int i = 0; i < buttons.Count; i++) {
				buttonsToPick[i].SetEnabled(false);
			}
			for(int i = 0; i < playerCount; i++) {
                int index = rand.Next(0, buttonsToPick.Count);
				buttonsToPick[index].SetEnabled(true);
				buttonsToPick.RemoveAt(index);
			}
			buttonsCount = buttons.Count;
		}

		if(pressedButtonsCount != pressedButtons.Count) {

			for(int i = 0; i < buttons.Count; i++) {
				buttons[i].color = pressedButtons.Contains(buttons[i]) ? Color.green : Color.red;

				if(pressedButtons.Count == playerCount)
					buttons[i].isCompleted = true;
				
			}

			pressedButtonsCount = pressedButtons.Count;
		}

		if(pressedButtons.Count == playerCount && !rc.puzzleComplete) {
            middleColor = Color.green;
            rc.puzzleComplete = true;
			openDoorSound.Play ();

		}
	}

	public void RegisterButton(ButtonScript bs) {
		if(!buttons.Contains(bs))
			buttons.Add(bs);
	}

	public void AddButton(ButtonScript bs) {
		if(!pressedButtons.Contains(bs))
			pressedButtons.Add(bs);
	}

	public void RemoveButton(ButtonScript bs) {
		pressedButtons.Remove(bs);
	}
}
