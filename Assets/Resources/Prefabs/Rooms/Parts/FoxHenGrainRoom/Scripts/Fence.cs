using UnityEngine;
using UnityEngine.Networking;

public class Fence : NetworkBehaviour {
	[SyncVar]
	public Color glowColor = Color.green;

	private FoxZone zone;
	private CustomGlowColor glow;

	void Start() {
		zone = transform.parent.parent.GetComponentInChildren<FoxZone>();
		glow = GetComponent<CustomGlowColor>();
	}

	void Update() {
		if (zone != null) {
			glowColor = (zone.player != null) ? Color.red : Color.green;
		}
		if (glow != null) {
			glow.glowColor = glowColor;
		}
	}

	public void goThroughGate(PlayerSync player) {
		if(glowColor == Color.green) {
			player.getController().Teleport(transform.position - (transform.right * 3f));
		} else {
			player.getController().Teleport(transform.position + (transform.right * 3f));
		}
	}
}
