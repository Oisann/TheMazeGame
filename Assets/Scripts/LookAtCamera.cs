using UnityEngine;

public class LookAtCamera : MonoBehaviour {
    public bool ignoreY = false;

    private Camera cam;
    
	void Start() {
        foreach(PlayerController player in GameObject.FindObjectsOfType<PlayerController>()) {
            if(player.isLocal()) {
                cam = player.getCameraman().cam;
            }
        }
	}
	
	void Update() {
		//Sometimes it cant find the camera in the start function, so we have to double check
        if(cam == null) {
            foreach(PlayerController player in GameObject.FindObjectsOfType<PlayerController>()) {
                if(player.isLocal()) {
                    cam = player.getCameraman().cam;
                }
            }
        } else {
            if(ignoreY) {
                Vector3 pos = cam.transform.position;
                pos.y = transform.position.y;
                transform.LookAt(pos);
            } else {
                transform.LookAt(cam.transform.position);
            }
        }
	}
}