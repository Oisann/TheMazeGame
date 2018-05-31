using UnityEngine;
using System.Collections.Generic;

public class LocalConnections : MonoBehaviour {

    [Header("Default Room Prefabs")]
    public GameObject defaultDoor;
    public GameObject defaultWall;
    public GameObject defaultFloor;
    public GameObject defaultMapRoom;

    [Header("Game Objects")]
    public GameObject MapParent;
	public GameObject Tutorial;

    private string roomsPath = "Prefabs/Rooms/Spawnable Rooms";
    public List<RoomObject> rooms = new List<RoomObject>();

    void Awake() {
        rooms.AddRange(Resources.LoadAll<RoomObject>(roomsPath));
    }
}
