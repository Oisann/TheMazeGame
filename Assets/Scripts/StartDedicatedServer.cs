using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Rendering;

public class StartDedicatedServer : MonoBehaviour {
    
	NetworkManager manager;

	void Awake() {
		manager = GetComponent<NetworkManager>();
		if (isHeadless()) {
			print("No graphics found. Starting in headless mode.");
			//manager.maxConnections = 4;
			manager.StartServer();
		}
	}
	
	bool isHeadless() {
        return SystemInfo.graphicsDeviceType == GraphicsDeviceType.Null;
    }
}
