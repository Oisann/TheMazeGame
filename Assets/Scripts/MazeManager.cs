using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;

[ExecuteInEditMode]
public class MazeManager : NetworkBehaviour {

    [Header("Network Prefabs")]
    public GameObject roomPrefab;
    public GameObject parentPrefab;
    public GameObject spawnPrefab;

    [Header("Variables")]
    [SyncVar]
    public string mazeSeed = "";

    [SyncVar]
    public Vector2 mazeSize = new Vector2(8, 8);

    [SerializeField]
    [SyncVar]
    private Vector2 startRoom;

    [SerializeField]
    [SyncVar]
    private int ROOM_GAP = 60; //FROM THE CENTER OF ONE ROOM TO THE NEXT

    private System.Random mazeRandom;
    private GameObject playersList;
	private NetworkVariables nvs;

    void Start() {
		if(!Application.isPlaying && Application.isEditor || !isServer)
            return;

		nvs = GameObject.FindObjectOfType<NetworkVariables>();
		nvs.mazeSize = this.mazeSize;
		nvs.ROOM_GAP = this.ROOM_GAP;
		nvs.startTimestamp = getCurrentTimestamp();

		if(GetComponent<ControlledMazeManager>() != null) {
            StartControlled();
            return;
        }

        if(isServer && string.IsNullOrEmpty(mazeSeed)) {
            string[] types = { "adjectives", "adjectives", "animals" };
            mazeSeed = Wordlist.GenerateWordFromList(types);
            //mazeSeed = Random.Range(int.MinValue, int.MaxValue) + "!";
        }

		PlayerPrefs.SetString("lastSeed", mazeSeed);
		PlayerPrefs.Save();

        if(isServer) {
            nvs.seed = mazeSeed;
            nvs.mazeID = AnalyticsController.GenerateMazeId(mazeSeed, mazeSize);
        }

        mazeRandom = new System.Random(mazeSeed.GetHashCode());

        playersList = GameObject.Find("Players");

        int msX = Mathf.RoundToInt(mazeSize.x), msY = Mathf.RoundToInt(mazeSize.y);
        float midPointX = ((msX - 1) * ROOM_GAP) / 2f;
        float midPointY = ((msY - 1) * ROOM_GAP) / 2f;
        startRoom = new Vector2(mazeRandom.Next(1, msX - 2), mazeRandom.Next(1, msY - 2));

        GameObject roomParent = Instantiate(parentPrefab, new Vector3(-midPointX, 0f, -midPointY), Quaternion.identity) as GameObject;
        NetworkInstanceID parentID = roomParent.GetComponent<NetworkInstanceID>();
        parentID.objectName = "Room";
        NetworkServer.Spawn(roomParent);

        RoomController[,] allRooms = new RoomController[msX, msY];

        for(int x = 0; x < msX; x++) {
            for(int y = 0; y < msY; y++) {
                //Generate basic rooms
                GameObject room = Instantiate(roomPrefab, Vector3.zero, Quaternion.identity) as GameObject;
                string _name = x * ROOM_GAP + ", " + y * ROOM_GAP;
                room.name = _name;
                RoomController rc = room.GetComponent<RoomController>();
                rc.mazeSeed = mazeSeed + "" + _name;
                rc.roomPosition = new Vector2(x, y);
                rc.roomGap = ROOM_GAP;
                room.transform.SetParent(roomParent.transform);
                room.transform.localPosition = new Vector3(x * ROOM_GAP, 0f, y * ROOM_GAP);

                if(Mathf.RoundToInt(startRoom.x) == x && Mathf.RoundToInt(startRoom.y) == y)
                    //rc.categories.Add(RoomObject.RoomCategory.Start);
                    rc.isStart = true; //a very dirty cheat to the start room bug where it can get another room on remote clients

                //Make it update it's parent and name
                NetworkInstanceID roomID = room.GetComponent<NetworkInstanceID>();
                roomID.parentNetId = roomParent.GetComponent<NetworkIdentity>().netId;
                roomID.objectName = _name;

                //Send it to every client
                NetworkServer.Spawn(room);

                if(Mathf.RoundToInt(startRoom.x) == x && Mathf.RoundToInt(startRoom.y) == y) {
                    //Spawn the spawnpoint
                    GameObject spawn = Instantiate(spawnPrefab) as GameObject;
                    spawn.transform.SetParent(rc.transform);
                    spawn.transform.localPosition = Vector3.zero;
                    string n = "Spawn Point";
                    spawn.name = n;

                    //Make it update it's parent and name
                    NetworkInstanceID spawnID = spawn.GetComponent<NetworkInstanceID>();
                    spawnID.parentNetId = roomID.netId;
                    spawnID.objectName = n;

                    //Send it to every client
                    NetworkServer.Spawn(spawn);
                }
                allRooms[x, y] = rc;
            }
        }

        int roomCountTarget = Mathf.RoundToInt((msX * msY) * .7f) * 2;
        List<RoomController> open_ends = new List<RoomController>();
        open_ends.Add(allRooms[Mathf.RoundToInt(startRoom.x), Mathf.RoundToInt(startRoom.y)]);

        while(open_ends.Count != 0) {
            int i = mazeRandom.Next(0, open_ends.Count);
            RoomController current = open_ends[i];
            roomCountTarget -= Mathf.Clamp(Mathf.RoundToInt(roomCountTarget / 10), 0, 100000);

            int roomX = Mathf.RoundToInt(current.roomPosition.x);
            int roomY = Mathf.RoundToInt(current.roomPosition.y);

            int doorChance = mazeRandom.Next(0, 100) + roomCountTarget;
            int doorsInStart = 4;

            if(doorChance < 10) {
                doorsInStart = 1;
            } else if(doorChance < 30) {
                doorsInStart = 2;
            } else if(doorChance < 60) {
                doorsInStart = 3;
            }

            List<int> dirs = new List<int>();
            dirs.Add(0);
            dirs.Add(1);
            dirs.Add(2);
            dirs.Add(3);
            for(int d = 0; d < doorsInStart; d++) {
                int t = mazeRandom.Next(0, dirs.Count);
                int dir = dirs[t];

                RoomController nextRoom = allRooms[roomX, roomY]; //Will never be this room, it's just to fix a 'unassigned' error.

                int setDirOnNext = -1;

                if(dir == 0) { //North +1x
                    if(allRooms.GetLength(0) - 1 >= roomX + 1) {
                        nextRoom = allRooms[roomX + 1, roomY];
                        setDirOnNext = 1;
                        if(nextRoom.hasBeenScanned)
                            continue;
                    } else {
                        continue;
                    }
                } else if(dir == 1) { //South -1x
                    if(0 <= roomX - 1) {
                        nextRoom = allRooms[roomX - 1, roomY];
                        setDirOnNext = 0;
                        if(nextRoom.hasBeenScanned)
                            continue;
                    } else {
                        continue;
                    }
                } else if(dir == 2) { //East -1y
                    if(0 <= roomY - 1) {
                        nextRoom = allRooms[roomX, roomY - 1];
                        setDirOnNext = 3;
                        if(nextRoom.hasBeenScanned)
                            continue;
                    } else {
                        continue;
                    }
                } else if(dir == 3) { //West +1y
                    if(allRooms.GetLength(1) - 1 >= roomY + 1) {
                        nextRoom = allRooms[roomX, roomY + 1];
                        setDirOnNext = 2;
                        if(nextRoom.hasBeenScanned)
                            continue;
                    } else {
                        continue;
                    }
                }
                setDoorBoolInRoom(dir, true, ref current);
                nextRoom.cameFrom = setDirOnNext;
                setDoorBoolInRoom(setDirOnNext, true, ref nextRoom);
                nextRoom.hasBeenScanned = true;
                open_ends.Add(nextRoom);
            }
            current.hasBeenScanned = true;
			if(open_ends.Count == 1)
				open_ends[0].isEnd = true;
            open_ends.Remove(current);
            if(current.roomPosition == new Vector2(Mathf.RoundToInt(startRoom.x), Mathf.RoundToInt(startRoom.y)))
                current.displayRoom = true;
        }
    }

    void StartControlled() {
        if(isServer && string.IsNullOrEmpty(mazeSeed)) {
            string[] types = { "adjectives", "adjectives", "animals" };
            mazeSeed = Wordlist.GenerateWordFromList(types);
        }

        if(isServer) {
            nvs.seed = mazeSeed;
            nvs.mazeID = AnalyticsController.GenerateMazeId(mazeSeed, mazeSize);
        }

        ControlledMazeManager controlledManager = GetComponent<ControlledMazeManager>();
        mazeRandom = new System.Random(mazeSeed.GetHashCode());
        playersList = GameObject.Find("Players");

        int startX = int.MinValue;
        int startY = int.MinValue;

        for(int y = 0; y < controlledManager.rows.Length; y++) {
            for(int x = 0; x < controlledManager.rows[y].column.Length; x++) {
                if(controlledManager.rows[y].column[x] == null)
                    continue;
                if(controlledManager.rows[y].column[x].category == RoomObject.RoomCategory.Start) {
                    startX = x;
                    startY = y;
                    break;
                }
            }
        }

        if(startX == int.MinValue || startY == int.MinValue) {
            Debug.LogError("No start room in the list of rooms");
            return;
        }

        int msX = Mathf.RoundToInt(mazeSize.x), msY = Mathf.RoundToInt(mazeSize.y);
        float midPointX = ((msX - 1) * ROOM_GAP) / 2f;
        float midPointY = ((msY - 1) * ROOM_GAP) / 2f;
        startRoom = new Vector2(startX, startY);

        GameObject roomParent = Instantiate(parentPrefab, new Vector3(-midPointX, 0f, -midPointY), Quaternion.identity) as GameObject;
        NetworkInstanceID parentID = roomParent.GetComponent<NetworkInstanceID>();
        parentID.objectName = "Room";
        NetworkServer.Spawn(roomParent);

        RoomController[,] allRooms = new RoomController[msX, msY];

        for(int x = 0; x < msX; x++) {
            for(int y = 0; y < msY; y++) {
                //Generate basic rooms
                GameObject room = Instantiate(roomPrefab, Vector3.zero, Quaternion.identity) as GameObject;
                string _name = x * ROOM_GAP + ", " + y * ROOM_GAP;
                room.name = _name;
                RoomController rc = room.GetComponent<RoomController>();
                rc.mazeSeed = mazeSeed + "" + _name;
                rc.roomPosition = new Vector2(x, y);
                rc.roomGap = ROOM_GAP;
                room.transform.SetParent(roomParent.transform);
                room.transform.localPosition = new Vector3(x * ROOM_GAP, 0f, y * ROOM_GAP);

                if(Mathf.RoundToInt(startRoom.x) == x && Mathf.RoundToInt(startRoom.y) == y)
                    //rc.categories.Add(RoomObject.RoomCategory.Start);
                    rc.isStart = true; //a very dirty cheat to the start room bug where it can get another room on remote clients

                //Make it update it's parent and name
                NetworkInstanceID roomID = room.GetComponent<NetworkInstanceID>();
                roomID.parentNetId = roomParent.GetComponent<NetworkIdentity>().netId;
                roomID.objectName = _name;

                //Send it to every client
                NetworkServer.Spawn(room);

                if(Mathf.RoundToInt(startRoom.x) == x && Mathf.RoundToInt(startRoom.y) == y) {
                    //Spawn the spawnpoint
                    GameObject spawn = Instantiate(spawnPrefab) as GameObject;
                    spawn.transform.SetParent(rc.transform);
					spawn.transform.localPosition = Vector3.zero + spawnPrefab.transform.position;
                    string n = "Spawn Point";
                    spawn.name = n;

                    //Make it update it's parent and name
                    NetworkInstanceID spawnID = spawn.GetComponent<NetworkInstanceID>();
                    spawnID.parentNetId = roomID.netId;
                    spawnID.objectName = n;

                    //Send it to every client
                    NetworkServer.Spawn(spawn);
                }
                allRooms[x, y] = rc;
            }
        }

        LocalConnections localhost = GameObject.FindObjectOfType<LocalConnections>();
        List<RoomController> open_ends = new List<RoomController>();
        open_ends.Add(allRooms[Mathf.RoundToInt(startRoom.x), Mathf.RoundToInt(startRoom.y)]);

        while(open_ends.Count != 0) {
            int i = mazeRandom.Next(0, open_ends.Count);
            RoomController current = open_ends[i];

            int roomX = Mathf.RoundToInt(current.roomPosition.x);
            int roomY = Mathf.RoundToInt(current.roomPosition.y);
            
            int doorsInStart = 4;

            for(int d = 0; d < doorsInStart; d++) {
                int dir = d;

                RoomController nextRoom = allRooms[roomX, roomY]; //Will never be this room, it's just to fix a 'unassigned' error.

                int setDirOnNext = -1;

                if(dir == 0) { //North +1x
                    if(allRooms.GetLength(0) - 1 >= roomX + 1) {
                        nextRoom = allRooms[roomX + 1, roomY];
                        setDirOnNext = 1;
                        if(nextRoom.hasBeenScanned) continue;
                        if(controlledManager.rows[roomY].column[roomX + 1] == null)
                            continue;
                    } else {
                        continue;
                    }
                } else if(dir == 1) { //South -1x
                    if(0 <= roomX - 1) {
                        nextRoom = allRooms[roomX - 1, roomY];
                        setDirOnNext = 0;
                        if(nextRoom.hasBeenScanned) continue;
                        if(controlledManager.rows[roomY].column[roomX - 1] == null)
                            continue;
                    } else {
                        continue;
                    }
                } else if(dir == 2) { //East -1y
                    if(0 <= roomY - 1) {
                        nextRoom = allRooms[roomX, roomY - 1];
                        setDirOnNext = 3;
                        if(nextRoom.hasBeenScanned) continue;
                        if(controlledManager.rows[roomY - 1].column[roomX] == null)
                            continue;
                    } else {
                        continue;
                    }
                } else if(dir == 3) { //West +1y
                    if(allRooms.GetLength(1) - 1 >= roomY + 1) {
                        nextRoom = allRooms[roomX, roomY + 1];
                        setDirOnNext = 2;
                        if(nextRoom.hasBeenScanned) continue;
                        if(controlledManager.rows[roomY + 1].column[roomX] == null)
                            continue;
                    } else {
                        continue;
                    }
                }

                setDoorBoolInRoom(dir, true, ref current);
                nextRoom.cameFrom = setDirOnNext;
                nextRoom.setRoom = localhost.rooms.IndexOf(controlledManager.rows[Mathf.RoundToInt(nextRoom.roomPosition.y)].column[Mathf.RoundToInt(nextRoom.roomPosition.x)]);
                setDoorBoolInRoom(setDirOnNext, true, ref nextRoom);
                nextRoom.hasBeenScanned = true;
                open_ends.Add(nextRoom);
            }
            current.hasBeenScanned = true;
            open_ends.Remove(current);
            if(controlledManager.spawnWithAllRoomsOpen || current.roomPosition == new Vector2(Mathf.RoundToInt(startRoom.x), Mathf.RoundToInt(startRoom.y)))
                current.displayRoom = true;

        }
    }

    public int getRoomGap() {
        return ROOM_GAP;
    }

    public void registerPlayer(GameObject player) {
        player.transform.SetParent(playersList.transform);
    }

    private void setDoorBoolInRoom(int dir, bool b, ref RoomController room) {
        if(dir == 0)
            room.hasDoorNorth = b;
        if(dir == 1)
            room.hasDoorSouth = b;
        if(dir == 2)
            room.hasDoorEast = b;
        if(dir == 3)
            room.hasDoorWest = b;
    }

	private int getCurrentTimestamp() {
		return (int)(System.DateTime.UtcNow.Subtract(new System.DateTime(1970, 1, 1))).TotalSeconds;
	}
}
