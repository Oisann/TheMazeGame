using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour {
    public int currentRoomX = 0;
    public int currentRoomY = 0;

	public RoomController currentRoomController;

    public RectTransform UI;

    //private MazeManager mazeManager;
    private NavMeshAgent agent;
    private Cameraman cam;
    private PlayerSync pSync;
	//public AudioSource NextPortal;

	private Vector3 dst;
    private Transform roomParent;
	private NetworkVariables nvs;
	//private Animator playerAnim;

    void Start() {
        //mazeManager = GameObject.FindObjectOfType<MazeManager>();
        //mazeManager.registerPlayer(transform.parent.gameObject);
        agent = GetComponent<NavMeshAgent>();
		nvs = GameObject.FindObjectOfType<NetworkVariables>();
        cam = transform.parent.GetComponentInChildren<Cameraman>();
        pSync = transform.parent.GetComponent<PlayerSync>();
        //playerAnim = GameObject.FindGameObjectWithTag ("Player").GetComponentInChildren<Animator> ();
        roomParent = GameObject.Find("Room").transform;
		InvokeRepeating("updateCurrentRoom", 0f, .25f);
    }

	void updateCurrentRoom() {
		RaycastHit crc;
		LayerMask mask = (1 << LayerMask.NameToLayer("Floor"));
		if(Physics.Raycast(transform.position + transform.up, -transform.up, out crc, 5f, mask)) {
			RoomController current_rc = crc.transform.GetComponentInParent<RoomController>();
			Transform c = crc.transform.parent;
			do {
				current_rc = c.GetComponentInParent<RoomController>();
				c = c.parent;
			} while(current_rc == null);
			currentRoomController = current_rc;
		}
	}

    void Update() {
		Vector3 posInMaze = roomParent.InverseTransformVector(transform.position);

		if((Mathf.RoundToInt(nvs.mazeSize.x) % 2) != 0)
			posInMaze.x += nvs.ROOM_GAP / 2;

		if(((Mathf.RoundToInt(nvs.mazeSize.y)) % 2) != 0)
			posInMaze.z += nvs.ROOM_GAP / 2;

		currentRoomX = Mathf.FloorToInt (posInMaze.x / nvs.ROOM_GAP) * nvs.ROOM_GAP;
		currentRoomY = Mathf.FloorToInt (posInMaze.z / nvs.ROOM_GAP) * nvs.ROOM_GAP;

        if(UI != null) {
            Vector3 start = new Vector3(30f, 30f, 0f);
            if((Mathf.RoundToInt(nvs.mazeSize.x) % 2) != 0)
                start.x -= 30f;
            if(((Mathf.RoundToInt(nvs.mazeSize.y)) % 2) != 0)
                start.y -= 30f;
            UI.anchoredPosition = start + new Vector3(currentRoomX, currentRoomY, 0f);
        }

        /*dst = agent.destination;

		if (transform.position == dst) 
		{
			playerAnim.SetBool ("Running", false);
		}*/

		if(isWalking()) {
            Vector3 pos = agent.steeringTarget - transform.position;
            Quaternion newRot = Quaternion.LookRotation(pos);
            if(Quaternion.Angle(transform.rotation, newRot) > .1f) {
                transform.rotation = Quaternion.Lerp(transform.rotation, newRot, Time.deltaTime * agent.speed);
				//playerAnim.SetBool ("Running", true);
                //transform.LookAt(new Vector3(agent.steeringTarget.x, transform.position.y, agent.steeringTarget.z));
            }
        }
    }

    void OnDrawGizmos() {
        Gizmos.color = Color.magenta;
        if(agent != null && agent.hasPath) {
            for(int i = 0; i < agent.path.corners.Length - 1; i++) {
                Vector3 curr = agent.path.corners[i];
                Vector3 next = agent.path.corners[i + 1];
                Gizmos.DrawLine(curr, next);
            }
            Gizmos.DrawWireSphere(agent.pathEndPosition, 1f);
        }
    }

	public bool isWalking() {
		return agent.hasPath;
	}

	public Vector3 getDestination() {
		return agent.destination;
	}

	public bool canReach(Vector3 pos) {
		NavMeshPath path = new NavMeshPath();
		agent.CalculatePath(pos, path);
		return path.status != NavMeshPathStatus.PathPartial;
	}

    public bool isLocal() {
		if(agent == null)
			return false;
        return agent.enabled;
    }

    public Cameraman getCameraman() {
        return cam;
    }
    
    public void Walk(Vector3 location) {
        agent.SetDestination(location);
    }

	public void Kill() {
		Teleport(pSync.startPosition);
	}

    public void Teleport(Vector3 location, bool noParticles=false) {
        if(!noParticles)
            pSync.SpawnTeleportSmoke(transform.position, transform.rotation);
        agent.Warp(location);
		agent.SetDestination(location);
        if(cam != null) {
            //Slide the cam the last few meters
            TeleportCameraToLocationSmoothly(location);
        }   
    }

    private void TeleportCameraToLocationSmoothly(Vector3 location) {
        Vector3 pos = cam.transform.position;
        Vector3 dir = (location - cam.transform.position).normalized;
        float dist = Vector3.Distance(cam.transform.position, location);
        float one = dist / 100f;
        float perc = 20f;
        cam.transform.position = location - ((one * perc) * dir);
    }
}
