using UnityEngine;
using System.Collections.Generic;

public class PortalLinker : MonoBehaviour {

	[Header("Outside")]
	public PortalPortals[] outside;

	[Header("North East")]
	public PortalPortals[] ne;

	[Header("North West")]
	public PortalPortals[] nw;

	[Header("Middle West")]
	public PortalPortals[] mw;

	[Header("Middle Middle")]
	public PortalPortals[] mm;

	[Header("Middle East")]
	public PortalPortals[] me;

	[Header("South West")]
	public PortalPortals[] sw;

	[Header("South Middle")]
	public PortalPortals[] sm;

	[Header("South East")]
	public PortalPortals[] se;

    [HideInInspector]
    public List<PortalPortals> allPortalsExceptMiddle;

	private System.Random rand;
	public string mazeSeed;
	private RoomController rc;
	
	void Start() {
		rc = transform.parent.GetComponent<RoomController>();
        allPortalsExceptMiddle = new List<PortalPortals>();
        allPortalsExceptMiddle.AddRange(outside);
        allPortalsExceptMiddle.AddRange(nw);
        allPortalsExceptMiddle.AddRange(ne);
        allPortalsExceptMiddle.AddRange(mw);
        allPortalsExceptMiddle.AddRange(me);
        allPortalsExceptMiddle.AddRange(sw);
        allPortalsExceptMiddle.AddRange(sm);
        allPortalsExceptMiddle.AddRange(se);
        if(rc == null) {
			Debug.LogError("No RoomController found!");
			return;
		}

        mazeSeed = rc.mazeSeed;
        rand = new System.Random(mazeSeed.GetHashCode());
        LinkCorrectPath();
    }

	private void LinkCorrectPath() {
		List<PortalPortals[]> rooms = new List<PortalPortals[]>();
		rooms.Add(outside);
		rooms.Add(nw);
		rooms.Add(ne);
		rooms.Add(mw);
		//rooms.Add(mm); //We don't want to go to the center by accident
		rooms.Add(me);
		rooms.Add(sw);
		rooms.Add(sm);
		rooms.Add(se);

        //Just a copy for later
        List<PortalPortals[]> rooms_COPY = rooms;

        //Generate Correct Route
        int startPortal = rand.Next(1, 4);
		PortalPortals start = getRoomFromNumber(0)[startPortal];

		int firstRoom = rand.Next(1, rooms.Count);
		int firstDoor = rand.Next(0, 4);
		PortalPortals first = rooms[firstRoom][firstDoor];
		PortalPortals first_opps = rooms[firstRoom][getOpposite(firstDoor)];
		rooms.RemoveAt(firstRoom);

		int secondRoom = rand.Next(1, rooms.Count);
		int secondDoor = rand.Next(0, 4);
		PortalPortals second = rooms[secondRoom][secondDoor];
		PortalPortals second_opps = rooms[secondRoom][getOpposite(secondDoor)];
		rooms.RemoveAt(secondRoom);

		int thirdDoor = rand.Next(0, 4);
		PortalPortals third = getRoomFromNumber(4)[thirdDoor];

		start.forceLinkPortal(first);
		first.forceLinkPortal(start);
		first_opps.forceLinkPortal(second);
		second.forceLinkPortal(first_opps);
		second_opps.forceLinkPortal(third);

        //Second doable
        int startTwoPortal = 0;
        if(startPortal == 1)
            startTwoPortal = rand.Next(2, 4);
        if(startPortal == 3)
            startTwoPortal = rand.Next(1, 4);
        if(startPortal == 2)
            startTwoPortal = rand.Next(1, 3) == 2 ? 3 : 1;
        PortalPortals startTwo = getRoomFromNumber(0)[startTwoPortal];

        int firstRoom_TWO = rand.Next(1, rooms.Count);
        int firstDoor_TWO = rand.Next(0, 4);
        PortalPortals first_TWO = rooms[firstRoom_TWO][firstDoor_TWO];
        PortalPortals first_opps_TWO = rooms[firstRoom_TWO][getOpposite(firstDoor_TWO)];
        rooms.RemoveAt(firstRoom_TWO);

        int secondRoom_TWO = rand.Next(1, rooms.Count);
        int secondDoor_TWO = rand.Next(0, 4);
        PortalPortals second_TWO = rooms[secondRoom_TWO][secondDoor_TWO];
        PortalPortals second_opps_TWO = rooms[secondRoom_TWO][getOpposite(secondDoor_TWO)];
        rooms.RemoveAt(secondRoom_TWO);

        int thirdRoom_TWO = rand.Next(1, rooms.Count);
        int thirdDoor_TWO = rand.Next(0, 4);
        PortalPortals third_TWO = rooms[thirdRoom_TWO][thirdDoor_TWO];
        PortalPortals third_opps_TWO = rooms[thirdRoom_TWO][getOpposite(thirdDoor_TWO)];
        rooms.RemoveAt(thirdRoom_TWO);

		// THERE IS A BUG IN THIS CODE SOMEWHERE!
        int fourthDoor = 0;
        if(thirdDoor == 0)
            fourthDoor = rand.Next(1, 4);
        if(thirdDoor == 1) {
            fourthDoor = rand.Next(2, 5);
            if(fourthDoor == 4)
                fourthDoor = 0;
        }
        if(thirdDoor == 2) {
            fourthDoor = rand.Next(3, 6);
            if(fourthDoor == 4)
                fourthDoor = 0;
            if(fourthDoor == 5)
                fourthDoor = 1;
        }
        if(thirdDoor == 3)
            fourthDoor = rand.Next(0, 3);
        PortalPortals fourth = getRoomFromNumber(4)[fourthDoor];

        startTwo.forceLinkPortal(first_TWO);
        first_TWO.forceLinkPortal(startTwo);
        first_opps_TWO.forceLinkPortal(second_TWO);
        second_TWO.forceLinkPortal(first_opps_TWO);
        second_opps_TWO.forceLinkPortal(third_TWO);
        third_TWO.forceLinkPortal(second_opps_TWO);
        third_opps_TWO.forceLinkPortal(fourth);

        PortalPortals last = outside[1];
        foreach(PortalPortals outs in outside) {
            if(outs == null)
                continue;
            if(!outs.isLinked())
                last = outs;
        }
        int firstRoom_THREE = rand.Next(1, rooms.Count);
        int firstDoor_THREE = rand.Next(0, 4);
        PortalPortals first_THREE = rooms[firstRoom_THREE][firstDoor_THREE];
        PortalPortals first_opps_THREE = rooms[firstRoom_THREE][getOpposite(firstDoor_THREE)];
        rooms.RemoveAt(firstRoom_THREE);

        int secondRoom_THREE = rand.Next(1, rooms.Count);
        int secondDoor_THREE = rand.Next(0, 4);
        PortalPortals second_THREE = rooms[secondRoom_THREE][secondDoor_THREE];
        PortalPortals second_opps_THREE = rooms[secondRoom_THREE][getOpposite(secondDoor_THREE)];
        rooms.RemoveAt(secondRoom_THREE);

        last.forceLinkPortal(first_THREE);
        first_THREE.forceLinkPortal(last);

        first_opps_THREE.forceLinkPortal(second_THREE);
        second_THREE.forceLinkPortal(first_opps_THREE);

        second_opps_THREE.forceLinkPortal(outside[rand.Next(1, 4)]);
        
        foreach(PortalPortals port in transform.GetComponentsInChildren<PortalPortals>()) {
            if(port != null) {
                if(!port.isLinked())
                    port.linkPortal(outside[rand.Next(1, 4)]);
            }
        }
    }

	private int getOpposite(int doorIndex) {
		if(doorIndex <= 1)
			return doorIndex + 2;
		return doorIndex - 2;
	}

	private PortalPortals[] getRoomFromNumber(int roomID) {
		switch(roomID) {
			case 0:
				return outside;
			case 1:
				return nw;
			case 2:
				return ne;
			case 3:
				return mw;
			case 4:
				return mm;
			case 5:
				return me;
			case 6:
				return sw;
			case 7:
				return sm;
			case 8:
				return se;
		}
		return null;
	}
}
