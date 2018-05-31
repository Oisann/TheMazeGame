using UnityEngine;

public class HeatDisableOnOpenGL : MonoBehaviour {

	void FixedUpdate() {
		gameObject.SetActive(!SystemInfo.graphicsDeviceVersion.StartsWith("Open"));
	}
}
