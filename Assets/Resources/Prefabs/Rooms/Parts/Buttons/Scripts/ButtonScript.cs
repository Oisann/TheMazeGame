using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ButtonScript : NetworkBehaviour {
	[SyncVar]
	public Color color = Color.red;
	[SyncVar]
	public bool isCompleted = false;

	[SyncVar]
	private bool isEnabled = false;

	private MeshRenderer button;
	private MeshRenderer floorGlass;
	private ButtonRoomController brc;

	void Start() {
		button = transform.GetChild(1).GetComponentInChildren<MeshRenderer>();
		floorGlass = transform.GetChild(2).GetComponentInChildren<MeshRenderer>();
		if(isServer) {
			brc = transform.parent.GetComponentInChildren<ButtonRoomController>();
			brc.RegisterButton(this);
		}
	}

	void Update() {
		if(!isEnabled) {
			transform.GetChild (0).gameObject.SetActive(false);
			transform.GetChild(1).gameObject.SetActive(false);
			Color glass = Color.green;
			glass.a = (1f / 256f) * 127f;
			floorGlass.material.color = glass;
			return;
		} else {
			transform.GetChild(0).gameObject.SetActive(true);
			transform.GetChild(1).gameObject.SetActive(true);
		}
        
		button.material.color = color;
		Color g = color;
		g.a = (1f / 256f) * 127f;
		floorGlass.material.color = g;
	}

	public void SetEnabled(bool enabled = true) {
		isEnabled = enabled;
	}

	void OnTriggerEnter(Collider other) {
		if(other.tag == "Player" && !isCompleted && isServer && isEnabled) {
			brc.AddButton(this);
		}
	}

	void OnTriggerExit(Collider other) {
		if(other.tag == "Player" && !isCompleted && isServer && isEnabled) {
			brc.RemoveButton(this);
		}
	}
}
