using UnityEngine;
using System.Collections.Generic;

public class EndGame : MonoBehaviour {
	public GameObject winScreen;
    public List<PlayerSync> playersInTheMiddle = new List<PlayerSync>();

    void OnTriggerEnter(Collider coll) {
        //List of players ingame.
        if(coll.CompareTag("Player")) {
            PlayerSync player = coll.transform.parent.parent.parent.parent.parent.parent.parent.parent.GetComponent<PlayerSync>();
            if(!playersInTheMiddle.Contains(player))
                playersInTheMiddle.Add(player);


            //Win check
            if(playersInTheMiddle.Count == GameObject.Find("Players").GetComponentsInChildren<PlayerSync>().Length) {
                GameObject.Find("HUD").gameObject.SetActive(false);
                NetworkVariables nvs = GameObject.FindObjectOfType<NetworkVariables>();
				Vector3 walkTo = new Vector3(transform.position.x, player.getController().transform.position.y, transform.position.z);
				player.getController().Walk(walkTo);
                int startTime = nvs.startTimestamp;
                nvs.finishTime = getCurrentTimestamp();
                int userTime = nvs.finishTime - startTime;
                string formattedTime = string.Format("{0:00}:{1:00}:{2:00}", userTime / 3600, (userTime / 60) % 60, userTime % 60);
                int openedRooms = -2; //Not counting the start and end room
                int allRooms = -1; //This one will not count the start room, but the end room.
                int puzzleRooms = 0;
                int puzzleFinished = 0;
                
                foreach(RoomController rc in GameObject.FindObjectsOfType<RoomController>()) {
                    if(rc.displayRoom)
                        openedRooms++;
                    if(rc.cameFrom != -1) {
                        allRooms++;
                        //rc.displayRoom = true;
                    }
                    if(rc.getRoomCategory() != RoomObject.RoomCategory.DeadEnd &&
                       rc.getRoomCategory() != RoomObject.RoomCategory.End &&
                       rc.getRoomCategory() != RoomObject.RoomCategory.Start &&
                       rc.getRoomCategory() != RoomObject.RoomCategory.Nothing)
                        puzzleRooms++;
                    if(rc.puzzleComplete)
                        puzzleFinished++;
                }

				GameObject ws = Instantiate(winScreen, Vector3.zero, Quaternion.identity) as GameObject;
				WinScreen screen = ws.GetComponent<WinScreen>();
				screen.timeInSeconds = userTime;
				screen.roomsOpened = openedRooms;
				screen.roomCount = allRooms;
				screen.puzzlesCompleted = puzzleFinished;
				screen.puzzleCount = puzzleRooms;
				screen.sizeX = Mathf.RoundToInt(nvs.mazeSize.x);
				screen.sizeY = Mathf.RoundToInt(nvs.mazeSize.y);
				screen.mazeSeed = nvs.seed;

                Destroy(GetComponent<BoxCollider>());
                Destroy(this);
            }
        }
    }

    void OnTriggerExit(Collider coll) {
        if(coll.CompareTag("Player")) {
            PlayerSync player = coll.transform.parent.parent.parent.parent.parent.parent.parent.parent.GetComponent<PlayerSync>();
            if(playersInTheMiddle.Contains(player))
                playersInTheMiddle.Remove(player);
        }
    }

    private int getCurrentTimestamp() {
        return (int) (System.DateTime.UtcNow.Subtract(new System.DateTime(1970, 1, 1))).TotalSeconds;
    }
}
