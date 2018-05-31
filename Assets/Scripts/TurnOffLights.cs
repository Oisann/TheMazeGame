using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class TurnOffLights : MonoBehaviour {
    [Range(0f, 600f)]
    public float radius = 120f;

    private SphereCollider sc;

    void Start() {
        sc = GetComponent<SphereCollider>();
        sc.isTrigger = true;
        sc.radius = radius;
    }

    void Update() {
        if(sc.radius != radius)
            sc.radius = radius;
    }

    void OnDrawGizmos() {
        if(transform.childCount <= 1)
            return;
        Gizmos.color = Color.gray;
        Gizmos.DrawWireSphere(transform.position, radius);
    }

    void OnTriggerEnter(Collider coll) {
        if(!coll.CompareTag("Player"))
            return;

        PlayerSync ps = coll.transform.parent.parent.GetComponent<PlayerSync>();
        if(ps.isLocalPlayer) {
            Light[] lights = transform.GetComponentsInChildren<Light>();
            foreach(Light light in lights) {
                light.enabled = true;
            }
        }
    }

    void OnTriggerExit(Collider coll) {
        if(!coll.CompareTag("Player"))
            return;
        PlayerSync ps = coll.transform.parent.parent.GetComponent<PlayerSync>();
        if(ps.isLocalPlayer) {
            Light[] lights = transform.GetComponentsInChildren<Light>();
            foreach(Light light in lights) {
                light.enabled = false;
            }
        }
    }
}

