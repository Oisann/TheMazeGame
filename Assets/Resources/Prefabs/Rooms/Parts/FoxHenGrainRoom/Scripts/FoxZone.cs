using UnityEngine;
using UnityEngine.Networking;

public class FoxZone : NetworkBehaviour {
    public Vector2 roomSize = new Vector2(50f, 50f);
    public bool crossedGrain, crossedFox, crossedHen, crossedPlayer,
				pickedGrain, pickedFox, pickedHen,
				insideGrain, insideFox, insideHen;
    public FoxController fox, hen, grain;

	public GameObject player_list;
	public PlayerSync player;

    private Vector3 nullNull, nullOne, oneNull, oneOne;

    public bool isInsideRoom(Vector3 world_pos) {
        Vector3 ignoreY = new Vector3(world_pos.x, transform.position.y, world_pos.z);
        Vector3 localPos = transform.InverseTransformPoint(ignoreY);

        return (localPos.x <= (roomSize.x / 2f) && localPos.x >= (roomSize.x / 2f) * -1f) &&
               (localPos.z <= (roomSize.y / 2f) && localPos.z >= (roomSize.y / 2f) * -1f);
    }

	public bool isLineCrossed(Vector3 world_pos) {
		return lineCross("Middle", world_pos);
    }

	public bool isOutside(Vector3 world_pos) {
		return lineCross("Outside", world_pos);
	}

	public bool lineCross(string child, Vector3 world_pos) {
		Transform mid = transform.Find(child);
		Vector3 ignoreY = new Vector3(world_pos.x, transform.position.y, world_pos.z);
		Vector3 localPos = transform.InverseTransformPoint(ignoreY);

		Vector3 l = Vector3.zero,
		l2 = Vector3.zero;

		for(int i = mid.childCount-1; i > 0; i--) {
			if(localPos.z > mid.GetChild(i).localPosition.z) {
				l = mid.GetChild(i - 1).position;
				l2 = mid.GetChild(i).position;
			}
		}

		Vector3 l_up = new Vector3(l2.x, 1f, l2.z);

		Vector3 line = transform.InverseTransformDirection((l2 - l).normalized);
		Vector3 line2 = transform.InverseTransformDirection((l_up + l2).normalized);
		Vector3 line3 = Vector3.Cross(line, line2);

		Vector3 purp = transform.InverseTransformDirection((ignoreY - l).normalized);

		float dotProduct = Vector3.Dot(line3, purp);
		return dotProduct < 0;
	}

    void BounceThemBack() {
        if(fox != null)
            fox.GetComponent<PickupableObject>().BounceItemBack();
        if(hen != null)
            hen.GetComponent<PickupableObject>().BounceItemBack();
        if(grain != null)
            grain.GetComponent<PickupableObject>().BounceItemBack();
    }

	void Start() {
		player_list = GameObject.Find("Players");
		InvokeRepeating("UpdatePlayers", 0f, .5f);
	}

	void UpdatePlayers() {
		if (player != null) {
			Vector3 pos = player.getController().transform.position;
			bool outs = isOutside(pos);
			bool inRoom = isInsideRoom(pos);
			if(!outs || !inRoom)
				player = null;
		}
		foreach(PlayerSync p in player_list.GetComponentsInChildren<PlayerSync>()) {
			bool inRoom = isInsideRoom(p.getController().transform.position);
			if(!inRoom)
				continue;
			bool outs = isOutside(p.getController().transform.position);
			if (player == null && outs) {
				player = p;
			} else if(player != p && outs) {
				p.getController().Teleport(transform.Find("Outside").position);
			}
		}
	}

    void FixedUpdate() {
        nullNull = transform.position + new Vector3(roomSize.x / 2, 0f, -roomSize.y / 2);
        nullOne = transform.position + new Vector3(roomSize.x / 2, 0f, roomSize.y / 2);
        oneOne = transform.position - new Vector3(roomSize.x / 2, 0f, -roomSize.y / 2);
        oneNull = transform.position - new Vector3(roomSize.x / 2, 0f, roomSize.y / 2);

        if(fox != null) {
            crossedFox = isLineCrossed(fox.transform.position);
			insideFox = !isOutside(fox.transform.position);
            pickedFox = fox.GetComponent<PickupableObject>().pickupObject != null;
        }

        if(hen != null) {
            crossedHen = isLineCrossed(hen.transform.position);
			insideHen = !isOutside(hen.transform.position);
            pickedHen = hen.GetComponent<PickupableObject>().pickupObject != null;
        }

        if(grain != null) {
            crossedGrain = isLineCrossed(grain.transform.position);
			insideGrain = !isOutside(grain.transform.position);
            pickedGrain = grain.GetComponent<PickupableObject>().pickupObject != null;
        }

		if(player != null)
			crossedPlayer = isLineCrossed(player.getController ().transform.position);
		else
			crossedPlayer = false;

		if(insideFox || insideGrain || insideHen)
			BounceThemBack();

        if(!pickedFox && !pickedHen && !pickedGrain) {
			if(crossedHen && crossedGrain && crossedFox) {
				transform.parent.parent.GetComponent<RoomController> ().puzzleComplete = true;
				CancelInvoke("UpdatePlayers");
			}
        }

        if(crossedPlayer) {
            if(!pickedFox && !pickedHen && !pickedGrain) {
                if(!crossedHen && !crossedFox && crossedGrain)
                    BounceThemBack();
                if(!crossedHen && !crossedGrain && crossedFox)
                    BounceThemBack();
            }
        } else {
            if(!pickedFox && !pickedHen && !pickedGrain) {
                if(crossedFox && crossedHen && !crossedGrain)
                    BounceThemBack();
                if(crossedGrain && crossedHen && !crossedFox)
                    BounceThemBack();
            }
        }
    }

    void OnDrawGizmos() {
        Gizmos.color = Color.green;

        Gizmos.DrawLine(nullNull, nullOne);
        Gizmos.DrawLine(nullNull, oneNull);
        Gizmos.DrawLine(oneNull, oneOne);
        Gizmos.DrawLine(nullOne, oneOne);

        if(transform.childCount <= 1)
            return;

        Gizmos.color = Color.red;
		Transform mid = transform.Find("Middle");
		for(int i = 1; i < mid.childCount; i++) {
			Gizmos.DrawLine(mid.GetChild(i-1).position, mid.GetChild(i).position);
        }

		Gizmos.color = Color.cyan;
		Transform outside = transform.Find("Outside");
		for(int i = 1; i < outside.childCount; i++) {
			Gizmos.DrawLine(outside.GetChild(i-1).position, outside.GetChild(i).position);
		}
    }
}
