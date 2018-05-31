using UnityEngine;
using UnityEngine.Networking;

public class ComponentRemover : NetworkBehaviour {
    public bool removeIfLocal = false;
    public bool destroy = false;

    public Behaviour[] removeComponents;
    public GameObject[] removeGameObjects;

    void Start() {
        if(!removeIfLocal == isLocalPlayer) {
            foreach(Behaviour b in removeComponents) {
                if(destroy) {
                    Destroy(b);
                } else {
                    b.enabled = false;
                }
            }

            foreach(GameObject b in removeGameObjects) {
                if(destroy) {
                    Destroy(b);
                } else {
                    b.SetActive(false);
                }
            }
        }
	}
}
