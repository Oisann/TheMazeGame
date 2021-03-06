using UnityEngine;
using UnityEngine.UI;

public class TimerUI : MonoBehaviour {
	public int currentTimestamp = -1;

	public NetworkVariables nvs;
	public Text timerText;
	
	void Start() {
		timerText = GetComponent<Text>();
	}

	void Update() {
		currentTimestamp = getCurrentTimestamp();
		if(nvs != null) {
			if (nvs.startTimestamp != -1) {
				int seconds = currentTimestamp - nvs.startTimestamp;
				timerText.text = string.Format("{0:00}:{1:00}:{2:00}", seconds / 3600, (seconds / 60) % 60, seconds % 60);
			}
		} else {
			nvs = GameObject.FindObjectOfType<NetworkVariables>();
		}
	}

	private int getCurrentTimestamp() {
		return (int)(System.DateTime.UtcNow.Subtract(new System.DateTime(1970, 1, 1))).TotalSeconds;
	}
}
