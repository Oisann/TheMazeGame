using UnityEngine;
using System.Collections.Generic;

public class DoorOpener : MonoBehaviour {
	
	[SerializeField]
	private Dictionary<string, RoomController> roomControllers = new Dictionary<string, RoomController>();

	internal void ActivateRoom(string pos) {
		if(roomControllers.ContainsKey(pos)) {
			RoomController cont;
			roomControllers.TryGetValue(pos, out cont);

			cont.displayRoom = true;
		}
	}

    public RoomController getController(string pos) {
        if(roomControllers.ContainsKey(pos)) {
            RoomController cont;
            roomControllers.TryGetValue(pos, out cont);

            return cont;
        }
        return null;
    }

    public void AddRoom(string pos, RoomController rc) {
		roomControllers.Add(pos, rc);
	}

	public virtual void OpenDoor(string pos) {}
}