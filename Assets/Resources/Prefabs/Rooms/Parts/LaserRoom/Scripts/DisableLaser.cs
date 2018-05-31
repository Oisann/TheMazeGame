using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class DisableLaser : MonoBehaviour {
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
		Gizmos.color = Color.cyan;
		Gizmos.DrawWireSphere(transform.parent.position, radius);
	}

	void OnTriggerEnter(Collider coll) {
		if(!coll.CompareTag("Player"))
			return;

		PlayerSync ps = coll.transform.parent.parent.GetComponent<PlayerSync>();
		if(ps.isLocalPlayer) {
			transform.GetChild(0).gameObject.SetActive(true);
		}
	}

	void OnTriggerExit(Collider coll) {
		if(!coll.CompareTag("Player"))
			return;
		PlayerSync ps = coll.transform.parent.parent.GetComponent<PlayerSync>();
		if(ps.isLocalPlayer) {
			transform.GetChild(0).gameObject.SetActive(false);
		}
	}
}
