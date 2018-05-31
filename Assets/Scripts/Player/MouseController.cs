using UnityEngine;

[ExecuteInEditMode]
public class MouseController : MonoBehaviour {
	public GameObject walkHereParticle;
	public Color color;

    private PlayerController player;
	private PlayerSync syncPlayer;
    private Camera cam;
    private PostEffect pe;

	private DoorController walkToDoor;
	private Transform rotateMirror;
	private float rotateMirrorEuler = 0f;
	private Vector3 tutorial = Vector3.zero;
	private Vector3 walkToDoorPos;
    private NetworkVariables nvs;
	private LocalConnections localhost;
	public Cloth cape;


    void Start() {
        player = transform.parent.GetComponentInChildren<PlayerController>();
        cam = transform.parent.GetComponentInChildren<Camera>();
        pe = cam.GetComponent<PostEffect>();
		syncPlayer = GetComponentInParent<PlayerSync>();
		cape = transform.parent.GetComponentInChildren<Cloth>();
        nvs = GameObject.FindObjectOfType<NetworkVariables>();
		localhost = GameObject.FindObjectOfType<LocalConnections>();
    }

	private void EnableCape() {
		cape.enabled = true;
	}

    void Update() {
        if(IngameChat.showChat || nvs.finishTime != -1)
            return;

		if(cape == null)
			cape = transform.parent.GetComponentInChildren<Cloth>();

		if(walkToDoor != null) {
			float dist = Vector3.Distance(player.transform.position, walkToDoorPos);
			if(dist <= 3f) {
				cape.enabled = false;
				walkToDoor.open(player);
				Invoke("EnableCape", 0.2f);
				walkToDoor = null;
			}
		}

		if (rotateMirror != null) {
			float dist = Vector3.Distance(player.transform.position, new Vector3 (rotateMirror.position.x, player.transform.position.y, rotateMirror.position.z));
			if(dist <= 3f) {
				//rotateMirror.eulerAngles += new Vector3 (0f, rotateMirrorEuler, 0f);
				transform.parent.GetComponentInChildren<PickupController>().CmdRotate(rotateMirror.gameObject, rotateMirrorEuler);
				rotateMirror = null;
			}
		}

		if(tutorial != Vector3.zero) {
			float dist = Vector3.Distance(player.transform.position, tutorial);
			if(dist <= 3f) {
				localhost.Tutorial.SetActive(true);
				tutorial = Vector3.zero;
			}
		}

        Ray mouseRay = cam.ScreenPointToRay(Input.mousePosition);
        LayerMask interactableMask = 1 << LayerMask.NameToLayer("Interactable");
		interactableMask |= 1 << LayerMask.NameToLayer("Floor");
		interactableMask |= 1 << LayerMask.NameToLayer("InteractableHover");
		interactableMask |= 1 << LayerMask.NameToLayer("UI");
        RaycastHit hit;
        if(Physics.Raycast(mouseRay, out hit, 2000, interactableMask)) {
			if(hit.transform.gameObject.layer == LayerMask.NameToLayer("UI"))
				return;
            float dist = Vector3.Distance(mouseRay.origin, hit.point);
            Debug.DrawRay(mouseRay.origin, mouseRay.direction * dist, Color.red);
			pe.SetHover(null);
			if(hit.transform.gameObject.layer == LayerMask.NameToLayer("Interactable") || hit.transform.gameObject.layer == LayerMask.NameToLayer("InteractableHover")) {
				Renderer r = hit.transform.gameObject.GetComponent<Renderer>();
				if(r == null) {
					foreach(Renderer child in hit.transform.gameObject.GetComponentsInChildren<Renderer>()) {
						if(child.gameObject.layer == LayerMask.NameToLayer("Interactable") || child.gameObject.layer == LayerMask.NameToLayer("InteractableHover")) //Need to recheck this object, because it might not be on the same layer as its parent.
							pe.SetHover(child);
					}
				} else {
					pe.SetHover(r);
				}
			}

            if(syncPlayer.isAdmin && Input.GetMouseButtonDown(1)) {
                player.Teleport(hit.point);
				walkToDoor = null;
            }

			if(IntelligentMouseButton(hit)) {
				walkToDoor = null;
				rotateMirror = null;
				tutorial = Vector3.zero;
                /*MouseMenu menu = hit.collider.GetComponent<MouseMenu>();
                if(menu != null) {
                    menu.options[0].DoAction();
                } else {
                    Debug.Log("No menu item found");
                }*/

				if(Input.GetKey(KeyCode.LeftShift)) { //disabled?
					syncPlayer.Ping(hit.point);
					return;
				}
                
                if(hit.transform.CompareTag("Door")) {
                    if(Vector3.Distance(new Vector3(hit.point.x, player.transform.position.y, hit.point.z), player.transform.position) <= 3f) {
                        //player.Interact(hit.transform.gameObject, "Open");
						cape.enabled = false;
						hit.transform.GetComponent<DoorController>().open(player);
						Invoke("EnableCape", 0.2f);
                    } else {
						walkToDoor = hit.transform.GetComponent<DoorController>();
						walkToDoorPos = new Vector3(hit.point.x, player.transform.position.y, hit.point.z);
                        player.Walk(hit.point);
                    }
                } else if(hit.transform.gameObject.layer == LayerMask.NameToLayer("Floor")) {
					player.Walk(hit.point);
				} else if(hit.transform.CompareTag("Mirror")) {
					GameObject toRot = hit.transform.gameObject;
					if(hit.transform.GetComponent<Mirror>() == null)
						toRot = hit.transform.parent.gameObject;
					rotateMirrorEuler = Input.GetKey(KeyCode.LeftControl) ? -22.5f : 22.5f;
					if (Vector3.Distance (new Vector3 (hit.point.x, player.transform.position.y, hit.point.z), player.transform.position) <= 3f) {
						transform.parent.GetComponentInChildren<PickupController>().CmdRotate(toRot, rotateMirrorEuler);

					} else {
						player.Walk(hit.point);
						rotateMirror = hit.transform;
						if(hit.transform.GetComponent<Mirror>() == null)
							rotateMirror = hit.transform.parent;
					}
				} else if(hit.transform.CompareTag("TutBlock")) {
					if (Vector3.Distance (new Vector3 (hit.point.x, player.transform.position.y, hit.point.z), player.transform.position) <= 3f) {
						localhost.Tutorial.SetActive(true);
					} else {
						player.Walk(hit.point);
						tutorial = new Vector3(hit.point.x, player.transform.position.y, hit.point.z);
					}
				} else if(hit.transform.GetComponent<PortalPortals>() != null) {
					hit.transform.GetComponent<PortalPortals>().ClickedOnPortal();
				} else if(hit.transform.GetComponent<Fence>() != null) {
					hit.transform.GetComponent<Fence>().goThroughGate(syncPlayer);
				}
            }
        }
    }

	private bool holdDown = true;
	public bool IntelligentMouseButton(RaycastHit hit) {
		if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Floor")) {
			bool hold = holdDown;
			holdDown = false;
			Invoke("LimitMouseHold", 1f);
			if(Input.GetMouseButtonDown(0)) {
				GameObject walkHere = Instantiate(walkHereParticle, hit.point, Quaternion.identity) as GameObject;
				if (false) {
					try {
						Renderer r = hit.collider.gameObject.GetComponent<Renderer> ();

						if (r == null)
							r = hit.collider.gameObject.GetComponentInChildren<Renderer> ();
						if (r == null)
							r = hit.collider.gameObject.GetComponentInParent<Renderer> ();

						Texture2D tex = (Texture2D)r.material.mainTexture;
						Color c = tex.GetPixelBilinear (hit.textureCoord.x, hit.textureCoord.y);
						c.a = .75f;
						color = c;
						ParticleSystem.MinMaxGradient grad = new ParticleSystem.MinMaxGradient ();
						grad.colorMin = c;
						grad.colorMax = c;
						ParticleSystem.MainModule main = walkHere.GetComponent<ParticleSystem> ().main;
						main.startColor = grad;
					} catch (System.Exception e) {
					}
				}
				walkHere.transform.eulerAngles = new Vector3(-90f, 0f, 0f);
				Destroy(walkHere, 5f);
				return true;
			}
			return (Input.GetMouseButton(0) && hold);
		}

		return Input.GetMouseButtonDown(0);
	}

	private void LimitMouseHold() {
		holdDown = true;
	}
}