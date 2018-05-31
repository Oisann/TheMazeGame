using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

public class PortalsLose : MonoBehaviour {
    public GameObject prefabLifeBar;
    public Transform deadPoint;

    public PlayerController player;
    public float life = 7.0f;

    private GameObject lifeBar;

    void Update() {
		return;
		if(player != null) {
            if(lifeBar == null) {
                lifeBar = Instantiate(prefabLifeBar, Vector3.zero, Quaternion.identity) as GameObject;
            }
            lifeBar.transform.position = new Vector3(player.transform.position.x, player.transform.position.y + 5f, player.transform.position.z);
            life -= Time.deltaTime;
            if(life <= 0f) {
                player.Teleport(deadPoint.position);
                player = null;
                if(lifeBar != null) {
                    Destroy(lifeBar);
                }
            } else {
                lifeBar.transform.GetChild(1).GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2((life/7f) * 5f, .5f);
            }
        } else {
			life = Mathf.Clamp(life + (Time.deltaTime * 2f), 0.0f, 7.00000f);
            if(lifeBar != null) {
                Destroy(lifeBar);
            }
        }
	}

    void OnTriggerEnter(Collider coll) {
		return;
        if(coll.CompareTag("Player")) {
            PlayerController pl = coll.transform.parent.parent.parent.parent.parent.parent.parent.GetComponent<PlayerController>();
            if(pl.GetComponent<NavMeshAgent>().enabled) {
                player = pl;
            }
        }
    }

    void OnTriggerExit(Collider coll) {
		return;
        if(coll.CompareTag("Player")) {
            PlayerController pl = coll.transform.parent.parent.parent.parent.parent.parent.parent.GetComponent<PlayerController>();
            if(pl.GetComponent<NavMeshAgent>().enabled) {
                player = null;
            }
        }
    }
}
