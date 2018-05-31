using UnityEngine;

[CreateAssetMenu(fileName = "New Room", menuName = "Rooms/New Room", order = 1)]
public class RoomObject : ScriptableObject {
	
	public enum RoomCategory {
		Logic,
		Math,
		Environment,
		Fun,
        Start,
        End,
		Nothing,
		DeadEnd
	}

    public bool disabled = false;

	public RoomCategory category = RoomCategory.Nothing;

	public bool hasDoorNorth = true;
	public bool hasDoorSouth = true;
	public bool hasDoorEast = true;
	public bool hasDoorWest = true;

    /*[Range(1, 4)]
    public int minPlayersRequirement = 1;*/
	[Header("Disable Doors ONLY when rooms are oddly shaped")]
	public bool disableDoorNorth = false;
	public bool disableDoorSouth = false;
	public bool disableDoorEast = false;
	public bool disableDoorWest = false;

    [Tooltip("Ignore Rotation According To Entry")]
    public bool ignoreRotationAccordingToEntry = false;
	public Vector3 rotation = Vector3.zero;

	public GameObject floor;
	public GameObject[] walls;
	public GameObject interior;
}