using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;

public class RoomController : NetworkBehaviour {

    [SyncVar]
    public bool displayRoom = false;

    [SerializeField]
	public RoomObject currentRoom;
    [SyncVar]
	public bool hasDoorNorth = false;
    [SyncVar]
    public bool hasDoorSouth = false;
    [SyncVar]
    public bool hasDoorEast = false;
    [SyncVar]
    public bool hasDoorWest = false;
	[SyncVar]
	public int cameFrom = -1;
	[SyncVar]
	public string mazeSeed = "";

	public GameObject fireWorks;

    public List<RoomObject.RoomCategory> categories = new List<RoomObject.RoomCategory>();

    [HideInInspector]
    [SyncVar]
    public Vector2 roomPosition = Vector2.zero;
	[HideInInspector]
	public int roomGap = 60;
    [HideInInspector]
    public bool hasBeenScanned = false;
	[SyncVar]
	public bool puzzleComplete = false;
    [HideInInspector]
    public MapRoom mapRoom;
	[HideInInspector]
	public System.Random rand;

    [HideInInspector]
    [SyncVar]
    public int setRoom = -1;

    [HideInInspector]
    [SyncVar]
    public bool isStart = false;

	[HideInInspector]
	[SyncVar]
	public bool isEnd = false;

    private string roomsPath = "Prefabs/Rooms/Spawnable Rooms";
	private List<RoomObject> roomsThatMatch;
	private bool hasRoomSpawned = false;
	private UnetDoor doorsParent;
    private LocalConnections localhost;
    private MapRoom mr;

	void Start() {
        localhost = GameObject.FindWithTag("GameController").GetComponent<LocalConnections>();

        GameObject mapChild = Instantiate(localhost.defaultMapRoom, Vector3.zero, Quaternion.identity) as GameObject;
        mapChild.transform.SetParent(localhost.MapParent.transform);
        mapChild.name = gameObject.name;
        mr = mapChild.GetComponent<MapRoom>();
        mr.rc = this;
        mr.x = Mathf.RoundToInt(roomPosition.x);
        mr.y = Mathf.RoundToInt(roomPosition.y);
    }

    void Generate() {
		roomsThatMatch = new List<RoomObject>();
		RoomObject[] rooms = Resources.LoadAll<RoomObject>(roomsPath);

        if(setRoom != -1) {
            currentRoom = localhost.rooms[setRoom];
            return;
        }

		if(isStart && !categories.Contains(RoomObject.RoomCategory.Start))
			categories.Add(RoomObject.RoomCategory.Start);

		if(isEnd && !categories.Contains(RoomObject.RoomCategory.End))
			categories.Add(RoomObject.RoomCategory.End);

		if(isDeadEnd() && !isStart && !isEnd) {
			categories.Clear();
			categories.Add(RoomObject.RoomCategory.DeadEnd);
		}

		foreach(RoomObject room in rooms) {
            if(!isStart && room.category == RoomObject.RoomCategory.Start || room.disabled)
                continue;
			if(!isEnd && room.category == RoomObject.RoomCategory.End || room.disabled)
				continue;
            if(room.hasDoorNorth && room.hasDoorSouth && room.hasDoorEast && room.hasDoorWest) {
                //if the room takes every door, we can always use it!
            } else {
                /*if (room.hasDoorNorth != this.hasDoorNorth)
				    continue;
			    if (room.hasDoorSouth != this.hasDoorSouth)
				    continue;
			    if (room.hasDoorEast != this.hasDoorEast)
				    continue;
			    if (room.hasDoorWest != this.hasDoorWest)
				    continue;*/
				
				if(!room.hasDoorNorth) {
					if(hasDoorLocalRotation(0))
						continue;
				}

				if(!room.hasDoorSouth) {
					if(hasDoorLocalRotation(1))
						continue;
				}

				if(!room.hasDoorEast) {
					if(hasDoorLocalRotation(2))
						continue;
				}

				if(!room.hasDoorWest) {
					if(hasDoorLocalRotation(3))
						continue;
				}

            }

			if(categories.Count != 0) {
				if(!categories.Contains(room.category))
					continue;
			} else {
				//Dead end rooms should only spawn on dead ends. If the room is a dead end, the categories list will contain a category and this will not be run.
				if(room.category == RoomObject.RoomCategory.DeadEnd)
					continue;
			}

			roomsThatMatch.Add(room);
		}

        if(roomsThatMatch.Count > 0)
            currentRoom = roomsThatMatch[rand.Next(0, roomsThatMatch.Count)];
	}

    void Update() {
		if(rand == null && mazeSeed != "")
			rand = new System.Random(mazeSeed.GetHashCode());

        if(puzzleComplete) {
            CustomGlowColor[] doors = transform.GetComponentsInChildren<CustomGlowColor>();
            foreach(CustomGlowColor cgc in doors) {
				if(cgc.transform.parent.GetComponent<Mirror>() != null)
					continue;
				cgc.transform.GetComponent<Renderer>().material.color = new Color(.25f, .98f, .30f);
                Destroy(cgc);
				if(isServer) {
					GameObject fireWorkPref = Instantiate(fireWorks, transform.position, Quaternion.Euler(new Vector3 (-90f, 0f, 0f))) as GameObject;
					Destroy(fireWorkPref, 2.5f);
					NetworkServer.Spawn(fireWorkPref);
				}
            }
        }

        if(doorsParent != null)
            return;
        #pragma warning disable 0414
        try {
            doorsParent = transform.parent.GetComponent<UnetDoor>();
            doorsParent.AddRoom(gameObject.name, this);
        } catch(System.Exception e) {
            //Debug.LogWarning("Loading...");
        }
        #pragma warning restore 0414
    }

    void FixedUpdate() {
		if(displayRoom && !hasRoomSpawned && rand != null) {
			if(doorsParent != null)
				doorsParent.OpenDoor(gameObject.name);
			/*foreach (FieldInfo field in currentRoom.GetType().GetFields()) {
				if(field.FieldType.ToString() == "UnityEngine.GameObject") {
					Debug.Log(field.Name);
				} else if(field.FieldType.ToString() == "UnityEngine.GameObject[]") {
					Debug.Log("Array " + field.Name);
				}
			}*/

			//Generate a room that fits
			Generate();

            //mapRoom.show(hasDoorNorth, hasDoorSouth, hasDoorEast, hasDoorWest);

            //Spawns the floor, if any
			if (currentRoom.floor != null) {
				GameObject floor = spawnOpject(currentRoom.floor);
			} else {
				if(localhost != null) {
					GameObject floor = spawnOpject(localhost.defaultFloor);
				}
			}

			//Spawns the interior, if any
			if(currentRoom.interior != null) {
				GameObject interior = spawnOpject(currentRoom.interior);
			}

			//Spawns the walls
			foreach (GameObject w in currentRoom.walls) {
				GameObject wall = spawnOpject(w);
			}

			if (currentRoom.walls.Length == 0) {
				if(localhost != null) {
					GameObject wall = spawnOpject(localhost.defaultWall);
				}
			}

			if(localhost != null) {
				if(!currentRoom.disableDoorNorth) {
					GameObject n = spawnOpject(localhost.defaultDoor);
					DoorController dc = n.GetComponent<DoorController>();
					dc.direction = DoorController.DoorDirection.north;
					dc.type = cameFrom == 0 ? DoorController.DoorType.door : getDoorType(hasDoorNorth);
				}
				if(!currentRoom.disableDoorSouth) {
					GameObject s = spawnOpject(localhost.defaultDoor);
					DoorController dc = s.GetComponent<DoorController>();
					dc.direction = DoorController.DoorDirection.south;
					dc.type = cameFrom == 1 ? DoorController.DoorType.door : getDoorType(hasDoorSouth);
				}
				if(!currentRoom.disableDoorEast) {
					GameObject e = spawnOpject(localhost.defaultDoor);
					DoorController dc = e.GetComponent<DoorController>();
					dc.direction = DoorController.DoorDirection.east;
					dc.type = cameFrom == 2 ? DoorController.DoorType.door : getDoorType(hasDoorEast);
				}
				if(!currentRoom.disableDoorWest) {
					GameObject w = spawnOpject(localhost.defaultDoor);
					DoorController dc = w.GetComponent<DoorController>();
					dc.direction = DoorController.DoorDirection.west;
					dc.type = cameFrom == 3 ? DoorController.DoorType.door : getDoorType(hasDoorWest);
				}
			}

            hasRoomSpawned = true;
		}
	}

	private DoorController.DoorType getDoorType(bool hasDoorInDir) {
		DoorController.DoorType door = DoorController.DoorType.door;

		if(currentRoom.category != RoomObject.RoomCategory.Nothing
            && currentRoom.category != RoomObject.RoomCategory.Start
            && currentRoom.category != RoomObject.RoomCategory.End
			&& currentRoom.category != RoomObject.RoomCategory.DeadEnd) {
			door = DoorController.DoorType.puzzle;
		}

		return hasDoorInDir ? door : DoorController.DoorType.wall;
	}

	private GameObject spawnOpject(GameObject go) {
		GameObject obj = Instantiate (go) as GameObject;
		obj.transform.SetParent (transform);
		obj.transform.localPosition = go.transform.position;
		obj.transform.localEulerAngles = go.transform.eulerAngles + RotateAccordingEnterDoor() + currentRoom.rotation;
		return obj;
	}

	public bool hasDoorLocalRotation(int dir) {
		if(cameFrom == 0) {				//North
			if(dir == 0) {				//North
				return this.hasDoorSouth;
			} else if(dir == 1) {		//South
				return this.hasDoorNorth;
			} else if(dir == 2) {		//East
				return this.hasDoorWest;
			} else if(dir == 3) {		//West
				return this.hasDoorEast;
			}
		} else if(cameFrom == 1) {		//South
			if(dir == 0) {				//North
				return this.hasDoorNorth;
			} else if(dir == 1) {		//South
				return this.hasDoorSouth;
			} else if(dir == 2) {		//East
				return this.hasDoorEast;
			} else if(dir == 3) {		//West
				return this.hasDoorWest;
			}
		} else if(cameFrom == 2) {		//East
			if(dir == 0) {				//North
				return this.hasDoorWest;
			} else if(dir == 1) {		//South
				return this.hasDoorEast;
			} else if(dir == 2) {		//East
				return this.hasDoorNorth;
			} else if(dir == 3) {		//West
				return this.hasDoorSouth;
			}
		} else if(cameFrom == 3) {		//West
			if(dir == 0) {				//North
				return this.hasDoorEast;
			} else if(dir == 1) {		//South
				return this.hasDoorWest;
			} else if(dir == 2) {		//East
				return this.hasDoorSouth;
			} else if(dir == 3) {		//West
				return this.hasDoorNorth;
			}
		}
		//No cameFrom direction
		return false;
	}

    public Vector3 RotateAccordingEnterDoor() {
        if(currentRoom.ignoreRotationAccordingToEntry)
            return Vector3.zero;

        if(cameFrom == 0) //North
            return new Vector3(0f, 0f, 0f);

        if(cameFrom == 1) //South
            return new Vector3(0f, 180f, 0f);

        if(cameFrom == 2) //East
            return new Vector3(0f, 90f, 0f);

        if(cameFrom == 3) //West
            return new Vector3(0f, -90f, 0f);

        return Vector3.zero;
    }

    public UnetDoor parent() {
        return doorsParent;
    }

    public bool isDeadEnd() {
		//The only door is in the north wall
		if (hasDoorNorth && !hasDoorSouth && !hasDoorEast && !hasDoorWest)
			return true;
		
		//The only door is in the south wall
		if (!hasDoorNorth && hasDoorSouth && !hasDoorEast && !hasDoorWest)
			return true;

		//The only door is in the east wall
		if (!hasDoorNorth && !hasDoorSouth && hasDoorEast && !hasDoorWest)
			return true;

		//The only door is in the west wall
		if (!hasDoorNorth && !hasDoorSouth && !hasDoorEast && hasDoorWest)
			return true;

		//Not a dead end
		return false;
	}

    public RoomObject.RoomCategory getRoomCategory() {
        if(currentRoom == null)
            return RoomObject.RoomCategory.Nothing;
        return currentRoom.category;
    }

	public string getRoomName() {
		if(currentRoom == null)
			return "NoRoom";
		return currentRoom.name;
	}

    void OnDrawGizmos() {
        if(hasDoorNorth) {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(transform.position + new Vector3(25f, 0f, 0f), 5f);
        }
        if(hasDoorSouth) {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(transform.position + new Vector3(-25f, 0f, 0f), 5f);
        }
        if(hasDoorEast) {
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(transform.position + new Vector3(0f, 0f, -25f), 5f);
        }
        if(hasDoorWest) {
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(transform.position + new Vector3(0, 0f, 25f), 5f);
        }
    }
}
