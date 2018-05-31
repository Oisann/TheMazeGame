using UnityEngine;

public class DoorController : MonoBehaviour {

	public enum DoorType {
		door,
		wall,
		key,
		puzzle
	}

	public enum DoorDirection {
		north,
		south,
		east,
		west
	}

	public DoorType type = DoorType.wall;
	public DoorDirection direction = DoorDirection.north;

	private RoomController rc;
	private Vector2 currentRoom;
	private string talkToRoom = "0, 0";
	public AudioSource NextRoom;

	void Start() {
		rc = transform.parent.GetComponent<RoomController>();
		transform.localEulerAngles = new Vector3(0f, rotation(direction), 0f);
		currentRoom = rc.roomPosition;
		talkToRoom = nextRoom(direction);
		if(type == DoorType.wall) {
			transform.Find("noDoor").gameObject.SetActive(true);
		} else if(type == DoorType.key) {
			transform.Find("keyDoor").gameObject.SetActive(true);
		} else if(type == DoorType.puzzle) {
			transform.Find("puzzleDoor").gameObject.SetActive(true);
		} else if(type == DoorType.door) {
			transform.Find("Door").gameObject.SetActive(true);
		}
	}

    public void open(PlayerController player) {
        Vector3 teleportRelative = Vector3.zero;
        if(direction == DoorDirection.north)
            teleportRelative += new Vector3(20f, 0f, 0f);
        if(direction == DoorDirection.south)
            teleportRelative += new Vector3(-20f, 0f, 0f);
        if(direction == DoorDirection.east)
            teleportRelative += new Vector3(0f, 0f, -20f);
        if(direction == DoorDirection.west)
            teleportRelative += new Vector3(0f, 0f, 20f);
        
        RoomController r = rc.parent().getController(talkToRoom);

        if(type == DoorType.wall) {
        } else if(type == DoorType.key) {
        } else if(type == DoorType.puzzle) {
			if(r != null && r.displayRoom) {
				player.Teleport(player.transform.position + teleportRelative, true);
				NextRoom.Play ();
				//AnalyticsController.DoAction("enterRoomThroughPuzzle", r.getRoomName() + " @ " + r.roomPosition.ToString());
				//AnalyticsController.DoAction("exitRoomThroughPuzzle", rc.getRoomName() + " @ " + rc.roomPosition.ToString());
			} else {
				if (rc.puzzleComplete) {
					r.displayRoom = true;


				} else {
					//Debug.Log("COMPLETE THE PUZZLE, YOU RETARD!");
				}
			}
        } else if(type == DoorType.door) {
            if(r != null && r.displayRoom) {
                player.Teleport(player.transform.position + teleportRelative, true);
				NextRoom.Play ();
				//AnalyticsController.DoAction("exitRoomThroughDoor", rc.getRoomName() + " @ " + rc.roomPosition.ToString());
				//AnalyticsController.DoAction("enterRoomThroughDoor", r.getRoomName() + " @ " + r.roomPosition.ToString());
            } else {
                r.displayRoom = true;
               

            }
        }
    }

	private float rotation(DoorDirection dir) {
		if(dir == DoorDirection.north)
			return -90;
		if(dir == DoorDirection.south)
			return 90;
		if(dir == DoorDirection.west)
			return 180;
		//east
		return 0;
	}

	private string nextRoom(DoorDirection dir) {
		int x = Mathf.RoundToInt(currentRoom.x) * rc.roomGap;
		int y = Mathf.RoundToInt(currentRoom.y) * rc.roomGap;

		if (dir == DoorDirection.north) {
			x += 1 * rc.roomGap;
		} else if (dir == DoorDirection.south) {
			x -= 1 * rc.roomGap;
		} else if (dir == DoorDirection.east) {
			y -= 1 * rc.roomGap;
		} else if (dir == DoorDirection.west) {
			y += 1 * rc.roomGap;
		}

		return Mathf.RoundToInt(x) + ", " + Mathf.RoundToInt(y);
	}
}
