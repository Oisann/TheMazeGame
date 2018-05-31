using UnityEngine;
using UnityEngine.Networking;

public class NetworkVariables : NetworkBehaviour {
	
	[SyncVar]
	public Vector2 mazeSize;

	[SyncVar]
	public int ROOM_GAP = 60;

    [SyncVar]
    public string seed = "";

    [SyncVar]
	public int startTimestamp = -1;

    [SyncVar]
    public int finishTime = -1;

    [SyncVar]
    public string mazeID = "";
}
