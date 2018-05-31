using UnityEngine;
using UnityEngine.Networking;

public class FoxController : NetworkBehaviour {
    public enum foxType {
        fox,
        hen,
        grain
    };
    public foxType myType = foxType.fox;
    
	void Start() {
		if(isServer) {
            FoxZone fz = transform.parent.GetComponentInChildren<FoxZone>();
            if(myType == foxType.fox)
                fz.fox = this;
            if(myType == foxType.hen)
                fz.hen = this;
            if(myType == foxType.grain)
                fz.grain = this;
        }
	}
}
