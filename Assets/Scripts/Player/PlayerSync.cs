using UnityEngine;
//using UnityEditor;
using UnityEngine.Networking;
using UnityEngine.AI;

public class PlayerSync : NetworkBehaviour {

    [SyncVar]
    private Vector3 syncPlayerPosition;
    [SyncVar]
    private Quaternion syncPlayerRotation;

    [SyncVar]
    private string syncDisplayName;
    [SyncVar]
    private Color syncDisplayNameColor;

    [SyncVar]
    private int speed = 10;

    [SyncVar]
    public bool isAdmin = false;

    //[SyncVar]
    //public int playerModel = -1;

    [SyncVar]
	private bool canPing = true;
	public GameObject pingObject;

	[SyncVar]
	public int animValue;

	private AnimationMenu animMenuScript;

    private Transform playerTransform;
    private TextMesh username;
	private NavMeshAgent agent;

	[SyncVar]
	private Vector3 agentDest;
    private Vector3 lastDest;

    [SerializeField]
    private GameObject teleportSmoke;
	private Animator playerAnim;

    private NetworkVariables nvs;

	[HideInInspector]
	public Vector3 startPosition;

    //[SerializeField]
    //private GameObject[] models;

    void Start() {
        playerTransform = transform.Find("Player");
        nvs = GameObject.FindObjectOfType<NetworkVariables>();
        username = playerTransform.GetComponentInChildren<TextMesh>();
		//models = Resources.LoadAll<GameObject>("Prefabs/Player Models");

        transform.SetParent(GameObject.Find("Players").transform);

		isAdmin = (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.LinuxEditor || Application.platform == RuntimePlatform.OSXEditor);

        if(teleportSmoke != null)
            ClientScene.RegisterPrefab(teleportSmoke);
		if (isLocalPlayer) {
			//playerModel = Random.Range (0, models.Length);
			animMenuScript = GameObject.Find("HUD").transform.GetComponentInChildren<AnimationMenu>();

            if(username.text == "USERNAME") {
				string[] types = { "adjectives", "animals" };
                username.text = Wordlist.GenerateWordFromList(types);

                Color[] colors = { Color.red, Color.yellow, Color.green, Color.blue, Color.cyan };
                username.color = colors[Random.Range(0, colors.Length)];
                //transform.GetChild(1).transform.GetChild(0).transform.GetChild(0).transform.GetChild(3).GetComponent<Renderer>().material.color = username.color;
				transform.GetChild(1).GetChild(0).GetChild(0).GetChild(2).GetChild(0).GetChild(0).GetChild(1).GetChild(1).GetComponent<Renderer>().material.color = username.color;
            }
			username.transform.localScale = Vector3.zero;
			agent = playerTransform.GetComponent<NavMeshAgent>();
            AnalyticsController.JoinMaze(username.text, username.color, nvs.mazeID);
            IngameChat.postLoginLogOut(true, username.text, username.color);
			startPosition = FindObjectOfType<NetworkStartPosition>().transform.position;
		}
    }

	void OnDestroy() {
		IngameChat.postLoginLogOut(false, username.text, username.color);
	}

	//private bool hasSpawned = false;
	void Update() {
		/*if (playerModel < 0) {
			return;
		} else {
			if (!hasSpawned) {
				GameObject model = Instantiate(models[playerModel], Vector3.zero, Quaternion.identity) as GameObject;
				model.transform.SetParent(playerTransform);
				model.transform.localPosition = Vector3.zero;
				model.name = "Model";
				hasSpawned = true;
				if(isLocalPlayer) {
					transform.GetChild(1).GetChild(transform.GetChild(1).childCount - 1).GetChild(0).GetChild(2).GetChild(0).GetChild(0).GetChild(1).GetChild(1).GetComponent<Renderer>().material.color = username.color;
				}
			}
		}*/

		if (playerAnim == null) {
			playerAnim = transform.GetChild(1).GetComponentInChildren<Animator>();
		}

		if (animMenuScript != null) {
			if (animValue != animMenuScript.theAnimValue) {
				UpdateEmote(animMenuScript.theAnimValue);
			}
		}

		float dist = Vector3.Distance(playerTransform.position, agentDest);
		playerAnim.SetBool ("Running", dist > 0.1f);

		if (dist > 0.1f) {
			if (animMenuScript != null) {
				animMenuScript.theAnimValue = 0;
				UpdateEmote(animMenuScript.theAnimValue);
			} 
		}

		playerAnim.SetInteger("Emotes", animValue);

        if(agentDest != lastDest) {
			System.Random r = new System.Random((agentDest.x + ", " + agentDest.y + ", " + agentDest.z + " " + username.text).GetHashCode());
			int idleAnimationCount = 3;
			playerAnim.SetInteger("RandomIdle", r.Next(0, idleAnimationCount));
			lastDest = agentDest;
        }

        if(gameObject.name != username.text)
            gameObject.name = username.text;
		
    }



    void FixedUpdate() {
		/*if(!hasSpawned)
			return;*/
        if(isLocalPlayer) {
			if(!IngameChat.showChat && Input.GetKey(KeyCode.LeftShift) && !agent.hasPath) {
                //Rotate Left and Right
                if(Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
                    playerTransform.eulerAngles -= new Vector3(0f, 1f, 0f) * 1.5f;
                if(Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
                    playerTransform.eulerAngles += new Vector3(0f, 1f, 0f) * 1.5f;

                //Scrollwheel rotate
                if(Input.GetMouseButton(2)) {
                    playerTransform.eulerAngles -= new Vector3(0f, Input.GetAxis("Mouse X") * 1.5f, 0f) * 5f;
                }
            }

			if(agent.destination != agentDest) {
				TransmitAgentDestination();
			}

            if(syncDisplayNameColor != username.color || syncDisplayName != username.text) {
                TransmitDisplayName();
            }

            if(Vector3.Distance(playerTransform.position, syncPlayerPosition) > .1f) {
                TransmitPlayerPosition();
            }

            if(Quaternion.Angle(playerTransform.rotation, syncPlayerRotation) > 5f) {
                TransmitPlayerRotation();
            }
        } else {
            LerpPosition();
            LerpRotation();
            UpdateDisplayName();
        }
    }

	public void ToggleCanPing() {
		canPing = !canPing;
	}

	public string getUsername() {
		return syncDisplayName;
	}

	public Color getUserColor() {
		return syncDisplayNameColor;
	}

	public PlayerController getController() {
		return playerTransform.GetComponent<PlayerController>();
	}

    [Command]
    public void CmdSpawnTeleportSmoke(Vector3 pos, Quaternion rot) {
        if(teleportSmoke != null) {
            Vector3 p = new Vector3(pos.x, pos.y + 1.5f, pos.z);
            GameObject instance = Instantiate(teleportSmoke, p, rot) as GameObject;
            instance.GetComponent<ParticleSystem>();
            Destroy(instance, 10f);
            NetworkServer.Spawn(instance);
        }
    }

    [Command]
    public void CmdProvideServerWithPosition(Vector3 pos) {
        syncPlayerPosition = pos;
    }

	[Command]
	public void CmdProvideServerWithDestination(Vector3 dest) {
		agentDest = dest;
	}

    [Command]
    public void CmdProvideServerWithRotation(Quaternion rot) {
        syncPlayerRotation = rot;
    }

    [Command]
    public void CmdProvideServerWithDisplayName(string name, Color color) {
        syncDisplayName = name;
        syncDisplayNameColor = color;
    }

	[Command]
	public void CmdPingAtPosition(Vector3 location, Color _color) {
		GameObject ping = Instantiate(pingObject, location, Quaternion.identity) as GameObject;
		ping.GetComponent<PingController>().arrowColor = _color;
		NetworkServer.Spawn(ping);
	}

	[Command]
	public void CmdUpdateEmote(int emote) {
		animValue = emote;
	}

	[Client]
	public void UpdateEmote(int emote) {
		if(isLocalPlayer && animMenuScript != null) {
			CmdUpdateEmote(emote);
		}
	}

	[Client]
	public void Ping(Vector3 location) {
		if(isLocalPlayer && canPing && pingObject != null) {
			canPing = false;
			CmdPingAtPosition(location, syncDisplayNameColor);
			Invoke("ToggleCanPing", 5f);
		}
	}

    [Client]
    public void TransmitPlayerPosition() {
        if(isLocalPlayer) {
            CmdProvideServerWithPosition(playerTransform.position);
        }
    }

	[Client]
	public void TransmitAgentDestination() {
		if(isLocalPlayer) {
			CmdProvideServerWithDestination(agent.destination);
		}
	}

    [Client]
    public void SpawnTeleportSmoke(Vector3 pos, Quaternion rot) {
        if(isLocalPlayer) {
            CmdSpawnTeleportSmoke(pos, rot);
        }
    }

    [Client]
    public void TransmitPlayerRotation() {
        if(isLocalPlayer) {
            CmdProvideServerWithRotation(playerTransform.rotation);
        }
    }

    [Client]
    public void TransmitDisplayName() {
        if(isLocalPlayer) {
            CmdProvideServerWithDisplayName(username.text, username.color);
        }
    }

    void LerpPosition() {
        if(Vector3.Distance(playerTransform.position, syncPlayerPosition) < 2f) {
            playerTransform.position = Vector3.Lerp(playerTransform.position, syncPlayerPosition, Time.deltaTime * speed);
        } else {
            playerTransform.position = syncPlayerPosition;
        }
    }

    void LerpRotation() {
        playerTransform.rotation = Quaternion.Lerp(playerTransform.rotation, syncPlayerRotation, Time.deltaTime * speed);
    }

    void UpdateDisplayName() {
        if(username.text != syncDisplayName)
            username.text = syncDisplayName;
		if(username.color != syncDisplayNameColor) {
			//transform.GetChild(1).transform.GetChild(0).transform.GetChild(0).transform.GetChild(3).GetComponent<Renderer>().material.color = syncDisplayNameColor;
			username.color = syncDisplayNameColor;
			transform.GetChild(1).GetChild(0).GetChild(0).GetChild(2).GetChild(0).GetChild(0).GetChild(1).GetChild(1).GetComponent<Renderer>().material.color = username.color;
		}
    }
}