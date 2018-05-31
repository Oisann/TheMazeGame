using UnityEngine;
using UnityEngine.AI;

public class PortalPortals : MonoBehaviour {

    [SerializeField]
    private PortalPortals linkedPortal;
    
    private Transform spawnPoint;
    public PlayerController pc;

	private PortalPortals walkToLinked;
    private ParticleSystem ps;

    void Start() {
        spawnPoint = transform.Find("Spawn Point");
        ps = GetComponentInChildren<ParticleSystem>();
        foreach(NavMeshAgent ag in GameObject.FindObjectsOfType<NavMeshAgent>()) {
            if(ag.enabled)
                pc = ag.GetComponent<PlayerController>();
        }
    }

	void Update() {
        if(pc == null) {
            foreach(NavMeshAgent ag in GameObject.FindObjectsOfType<NavMeshAgent>()) {
                if(ag.enabled)
                    pc = ag.GetComponent<PlayerController>();
            }
            return;
        }

        if(linkedPortal != null) {
            if(ps.isStopped)
                ps.Play();
        } else {
            if(!ps.isStopped)
                ps.Stop();
        }

        if(walkToLinked != null) {
            float destDist = Vector3.Distance(pc.getDestination(), walkToLinked.transform.position);
			if(destDist <= 1f) {
				float dist = Vector3.Distance(pc.transform.position, walkToLinked.transform.position);
				if (dist <= 2f) {
					pc.Teleport(linkedPortal.getTeleportPosition(), true);
					walkToLinked = null;
				}
			} else {
                walkToLinked = null;
			}
		}
	}

    //void OnMouseDown() {
	public void ClickedOnPortal() {
		if(pc == null)
            return;
        if(linkedPortal != null) {
            //TODO: CHECK DISTANCE BETWEEN PLAYER AND PORTAL
			float dist = Vector3.Distance(pc.transform.position, transform.position);
			if(pc.canReach(transform.position) && dist <= 2f) {
				pc.Teleport(linkedPortal.getTeleportPosition(), true);

			} else {
				if(pc.canReach(transform.position)) {
					pc.Walk(transform.position);
					walkToLinked = this;

				}
			}
        } else {
            Debug.LogError("Portal is missing its destination");
        }
    }

    public Vector3 getTeleportPosition() {
        return spawnPoint.position;
    }

	public bool linkPortal(PortalPortals leadTo) {
		if(linkedPortal != null)
			return false;
		linkedPortal = leadTo;
		return true;
	}

	public void forceLinkPortal(PortalPortals leadTo) {
		linkedPortal = leadTo;
	}

	public bool isLinked() {
		return linkedPortal != null;
	}

    void OnDrawGizmos() {
        Vector3 offset = new Vector3(0f, 3f, 0f);
        if(isLinked()) {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position + offset, linkedPortal.getTeleportPosition() + offset);
            Gizmos.DrawSphere(transform.position + offset, .5f);
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(linkedPortal.getTeleportPosition() + offset, .25f);
        } else {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(transform.position + offset, .5f);
        }
    }
}
