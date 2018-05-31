using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.AI;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(NetworkTransform))]
public class PickupableObject : NetworkBehaviour {

	public AudioSource DropSound;
	public AudioSource PickUpSound;

	[Header("Current Room")]
    public int currentRoomX = 0;
    public int currentRoomY = 0;

	[Header("Settings")]
	[SyncVar]
	public bool canPlayerDrop = true;

    [SerializeField]
	[SyncVar]
    private float reachDistance = 3f;
    [SerializeField]
	[SyncVar]
    private Vector3 positionOffset = new Vector3(0f, 0f, 0f);
    [SerializeField]
	[SyncVar]
    private Vector3 rotationOffset = new Vector3(0f, 0f, 0f);
    public Transform pickupObject;
	[SyncVar]
	private float followSpeed = 50f;
	[SyncVar]
	private bool usesObstacle = false;

	public float throwForceY = 200f, throwForceZ = 250f;

    private NetworkTransform nTrans;
    private Rigidbody rb;

    private Transform roomParent;
    public Vector3 startPosition;
    private int startRoomX, startRoomY;
	private NavMeshObstacle obstacle;
	private NetworkVariables nvs;

    void Start() {
        nTrans = GetComponent<NetworkTransform>();
        nTrans.transformSyncMode = NetworkTransform.TransformSyncMode.SyncRigidbody3D;
		nvs = GameObject.FindObjectOfType<NetworkVariables>();

        rb = GetComponent<Rigidbody>();
        rb.collisionDetectionMode = CollisionDetectionMode.Discrete;
		obstacle = GetComponent<NavMeshObstacle>();
		if (obstacle != null) {
			if(obstacle.enabled)
				usesObstacle = true;
		}
        roomParent = GameObject.Find("Room").transform;
        checkStartPosition();
        //Invoke("checkStartPosition", .3f);
    }

	private Vector3 startBouncePos = Vector3.zero;
	private float bounceTimer = 0f;

	void bounceBack() {
		Vector3 center = (startBouncePos + startPosition) * .5f;
		center -= new Vector3(0f, 1f, 0f);
		Vector3 relStart = startBouncePos - center;
		Vector3 relEnd = startPosition - center;

		float frac = (Time.time - bounceTimer) / 1.5f;
		transform.position = Vector3.Slerp(relStart, relEnd, frac);
		transform.position += center;
		if(Vector3.Distance(transform.position, startPosition) <= 0.1f) {
			transform.position = startPosition;
			CancelInvoke("bounceBack");
		}
	}

    public void BounceItemBack() {

        if(pickupObject != null)
            pickupObject.parent.parent.GetComponent<PickupController>().CmdDrop(gameObject);
        rb.velocity = Vector3.zero;

        if(!IsInvoking("bounceBack")) {
            startBouncePos = transform.position;
            bounceTimer = Time.time;
            InvokeRepeating("bounceBack", 0f, 0.01f);
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

		if(transform.position.y <= -4f) { //-20f
            BounceItemBack();
            return;
		}

        if(startPosition != null) {
            if(startRoomX != currentRoomX || startRoomY != currentRoomY) {
                BounceItemBack();
                return;
            }
        }

        if(pickupObject != null) {
			if(DropSound != null)
				DropSound.Play();
			float dist = Vector3.Distance(transform.position, pickupObject.position + pickupObject.TransformVector(positionOffset));
			if (dist >= .05f)
			{
				transform.position = Vector3.Lerp(transform.position, pickupObject.position + pickupObject.TransformVector(positionOffset), followSpeed * Time.deltaTime);
		    }

            transform.eulerAngles = pickupObject.transform.parent.eulerAngles + rotationOffset;


        }
    }

    public void checkStartPosition() {
		if(roomParent == null) {
			Debug.LogWarning("No parent. Unknown startPosition");
			return;
		}
        startPosition = roomParent.InverseTransformVector(transform.position);

		if((Mathf.RoundToInt(nvs.mazeSize.x) % 2) != 0)
			startPosition.x += nvs.ROOM_GAP / 2;

		if(((Mathf.RoundToInt(nvs.mazeSize.y)) % 2) != 0)
			startPosition.z += nvs.ROOM_GAP / 2;
		
		startRoomX = Mathf.FloorToInt(startPosition.x / nvs.ROOM_GAP /*mazeManager.getRoomGap()*/) * nvs.ROOM_GAP /*mazeManager.getRoomGap()*/;
		startRoomY = Mathf.FloorToInt(startPosition.z / nvs.ROOM_GAP /*mazeManager.getRoomGap()*/) * nvs.ROOM_GAP /*mazeManager.getRoomGap()*/;
    }

    public bool pickUp(Transform toFollow) {
        /*float dist = Vector3.Distance(transform.position, toFollow.position);
		if(dist > this.reachDistance || pickupObject != null)
			return false;*/
		if(usesObstacle)
			obstacle.enabled = false;
		if(PickUpSound != null)
        	PickUpSound.Play();
		pickupObject = toFollow;
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
        rb.isKinematic = true;
        transform.tag = "Untagged";
        return true;
    }

    public bool drop() {
        if(pickupObject == null)
            return false;

		if(usesObstacle)
			obstacle.enabled = true;

        pickupObject = null;
        rb.collisionDetectionMode = CollisionDetectionMode.Discrete;
        rb.isKinematic = false;
        transform.tag = "InterObj";

		if(DropSound != null)
        	DropSound.Play();
		
        return true;
    }

	public void ThrowObj(Vector3 velocity){
	

		if (pickupObject == null) return;
		
		Debug.Log ("Hei");
		pickupObject = null;
		rb.collisionDetectionMode = CollisionDetectionMode.Discrete;
		rb.isKinematic = false;
		transform.tag = "InterObj";
		GetComponent<Rigidbody> ().AddRelativeForce (new Vector3 (0,throwForceY,throwForceZ + Mathf.Abs(velocity.x + velocity.z) ));


	}

    public float getReachDistance() {
        return reachDistance;
    }

	public void setReachDistance(float f) {
		reachDistance = f;
	}

	public Vector3 getPositionOffset() {
		return positionOffset;
	}

	public void setPositionOffset(Vector3 offset) {
		positionOffset = offset;
	}
}
