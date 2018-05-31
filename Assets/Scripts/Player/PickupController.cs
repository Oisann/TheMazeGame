using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Networking;

public class PickupController : NetworkBehaviour {

	private GameObject player;
	private Camera cam;
	private Transform pickupPoint;
	private PickupableObject pickedUp;
	public AudioSource MirrorMove;

	private GameObject walkAndPickup;

	void Start() {
		player = transform.Find("Player").gameObject;
		cam = transform.parent.GetComponentInChildren<Camera>();
		pickupPoint = transform.Find("Player").Find("PlayerPickup");
	}

	void Update() {
		if(isLocalPlayer) {
			PickupUpdate();
		}
	}

	void PickupUpdate() {
		if(walkAndPickup != null) {
			float dist = Vector3.Distance(player.transform.position, walkAndPickup.transform.position);
			PickupableObject pick = walkAndPickup.GetComponent<PickupableObject>();
			if(dist <= pick.getReachDistance()) {
				CmdPickup(walkAndPickup);
				walkAndPickup = null;
			}
		}
		if(pickedUp != null) {

			if (Input.GetKeyDown (KeyCode.G)) {
				CmdThrow (pickedUp.gameObject);
			}

			PickupableObject pick = pickedUp.GetComponent<PickupableObject>();
			if(Input.GetMouseButtonUp(1) && pick.canPlayerDrop) {
				CmdDrop(pickedUp.gameObject);

			}

		} else {
			if(Input.GetMouseButtonDown(0)) {
				Ray mouseRay = cam.ScreenPointToRay(Input.mousePosition);
				LayerMask interactableMask = 1 << LayerMask.NameToLayer("Interactable");
				interactableMask |= 1 << LayerMask.NameToLayer("InteractableHover");
				RaycastHit hit;
				if(Physics.Raycast(mouseRay, out hit, 2000, interactableMask)) {
					walkAndPickup = null;
					PickupableObject pick = hit.transform.GetComponent<PickupableObject>();
                    float dist = Vector3.Distance(player.transform.position, hit.point);
					if(pick != null && pick.pickupObject == null) {
						
						if(dist <= pick.getReachDistance()) {
							CmdPickup(hit.transform.gameObject);
						} else {
							PlayerController pc = player.GetComponent<PlayerController>();
							pc.Walk(hit.point);
							walkAndPickup = hit.transform.gameObject;
						}
					}
				}
			}
		}
	}

	[ClientRpc]
	void RpcPickup(GameObject obj) {
		PickupableObject pick = obj.GetComponent<PickupableObject>();
		pick.pickUp(pickupPoint);
        pickedUp = pick;
    }

	[Command]
    public void CmdPickup(GameObject obj) {
		PickupableObject pick = obj.GetComponent<PickupableObject>();
		if(pick.pickupObject != null)
			return;
		NetworkIdentity id = obj.GetComponent<NetworkIdentity>();
		id.AssignClientAuthority(connectionToClient);
		RpcPickup(obj);
	}

	[ClientRpc]
    void RpcDrop(GameObject obj) {
		PickupableObject pick = obj.GetComponent<PickupableObject>();
		pick.drop();
        pickedUp = null;
    }

	[Command]
	public void CmdDrop(GameObject obj) {
		PickupableObject pick = obj.GetComponent<PickupableObject>();
		if(pick.pickupObject == null)
			return;
        NetworkIdentity id = obj.GetComponent<NetworkIdentity>();
		RpcDrop(obj);
		id.RemoveClientAuthority(connectionToClient);
	}

	[Command]
	public void CmdRotate(GameObject obj, float euler) {
		NetworkIdentity id = obj.GetComponent<NetworkIdentity>();
		id.AssignClientAuthority(connectionToClient);
		RpcRotate(obj, euler);

	}

	[ClientRpc]
	void RpcRotate(GameObject obj, float euler) {
		obj.transform.eulerAngles += new Vector3(0f, euler, 0f);

	}

	[ClientRpc]
	void RpcThrow (GameObject obj, Vector3 velocity){
		PickupableObject pick = obj.GetComponent<PickupableObject>();
		pick.ThrowObj (velocity * 40);


		pickedUp = null;
	}

	[Command]
	public void CmdThrow (GameObject obj){
		PickupableObject pick = obj.GetComponent<PickupableObject>();
		if(pick.pickupObject == null)
			return;
		NetworkIdentity id = obj.GetComponent<NetworkIdentity>();
		NavMeshAgent agent = player.GetComponent<NavMeshAgent>();
		RpcThrow(obj, agent.velocity);
		id.RemoveClientAuthority(connectionToClient);
	}
		
}
