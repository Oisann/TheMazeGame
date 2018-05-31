using UnityEngine;
using UnityEngine.Networking;

public class NetworkInstanceID : NetworkBehaviour {
    
    [SyncVar]
    public NetworkInstanceId parentNetId;

    [SyncVar]
    public string objectName;

    public override void OnStartClient() {
        try {
            GameObject parentObject = ClientScene.FindLocalObject(parentNetId);
            transform.SetParent(parentObject.transform);
        } catch(System.Exception e) {}
        if(!string.IsNullOrEmpty(objectName))
            gameObject.name = objectName;
    }
}
