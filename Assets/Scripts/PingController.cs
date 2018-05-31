using UnityEngine;
using UnityEngine.Networking;

public class PingController : NetworkBehaviour {

    [SyncVar]
    public Color arrowColor = Color.white;
    public float lifetime = 5f;

    private MeshRenderer arrowMeshRenderer;

    void Start() {
        GameObject.FindObjectOfType<UIPingArrow>().ping = this;
        PingController[] pings = GameObject.FindObjectsOfType<PingController>();
        foreach(PingController ping in pings) {
            if(ping != this)
                DestroyImmediate(ping.gameObject);
        }
        arrowMeshRenderer = transform.GetChild(0).GetChild(0).GetComponent<MeshRenderer>();
        Destroy(gameObject, lifetime);
    }
	
	void Update() {
        if(arrowMeshRenderer.material.color != arrowColor)
            arrowMeshRenderer.material.color = arrowColor;
	}
}
