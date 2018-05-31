using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class MapController : MonoBehaviour {
    public GameObject p1, p2, p3, p4;

    private NetworkVariables nvs;
    private Transform map;
    private RoomController[,] rooms;
    private MapRoom[,] mapRooms;
    private int sizeX, sizeY;
    private bool fixStuff = true;
    private GameObject players;

    [HideInInspector]
    public List<GameObject> unusedP = new List<GameObject>();

    void Start() {
        map = transform.GetChild(0);
        players = GameObject.Find("Players");
        Transform ps = transform.GetChild(1);
        p1 = ps.GetChild(0).gameObject;
        p2 = ps.GetChild(1).gameObject;
        p3 = ps.GetChild(2).gameObject;
        p4 = ps.GetChild(3).gameObject;
        p1.SetActive(false);
        p2.SetActive(false);
        p3.SetActive(false);
        p4.SetActive(false);
        unusedP.Add(p1);
        unusedP.Add(p2);
        unusedP.Add(p3);
        unusedP.Add(p4);
    }
	
	void Update() {
        if(fixStuff) {
            if(nvs != null) {
                if(nvs.startTimestamp == -1)
                    return;
                sizeX = Mathf.RoundToInt(nvs.mazeSize.x);
                sizeY = Mathf.RoundToInt(nvs.mazeSize.y);

                rooms = new RoomController[sizeX, sizeY];
                mapRooms = new MapRoom[sizeX, sizeY];
                map.GetComponent<RectTransform>().sizeDelta = new Vector2(sizeX * nvs.ROOM_GAP, sizeY * nvs.ROOM_GAP);
                map.GetComponent<RectTransform>().anchoredPosition = Vector3.zero;
                fixStuff = false;
            } else {
                nvs = GameObject.FindObjectOfType<NetworkVariables>();
                return;
            }
        }
        bool showMap = Input.GetKey(KeyCode.Tab);

        if(p1.activeInHierarchy != showMap && !unusedP.Contains(p1))
            p1.SetActive(showMap);
        if(p2.activeInHierarchy != showMap && !unusedP.Contains(p2))
            p2.SetActive(showMap);
        if(p3.activeInHierarchy != showMap && !unusedP.Contains(p3))
            p3.SetActive(showMap);
        if(p4.activeInHierarchy != showMap && !unusedP.Contains(p4))
            p4.SetActive(showMap);

        if(map.gameObject.activeInHierarchy != showMap)
            map.gameObject.SetActive(showMap);
    }

    void FixedUpdate() {
        foreach(PlayerController pc in players.GetComponentsInChildren<PlayerController>()) {
            if(pc.UI == null) {
                unusedP[0].GetComponent<PlayerIndicators>().player = pc;
                pc.UI = unusedP[0].GetComponent<RectTransform>();
                unusedP.RemoveAt(0);
            }
        }
    }

    public bool isReady() {
        return rooms != null;
    }

    public void SetRoom(int x, int y, RoomController rc, MapRoom mr) {
        rooms[x, y] = rc;
        mapRooms[x, y] = mr;
    }

    public Vector3 getRoomScreenPos(int x, int y) {
        float midx = ((Mathf.RoundToInt(nvs.mazeSize.x) - 1) * nvs.ROOM_GAP) / 2f;
        float midy = ((Mathf.RoundToInt(nvs.mazeSize.y) - 1) * nvs.ROOM_GAP) / 2f;
        return new Vector3((x * nvs.ROOM_GAP) - midx, (y * nvs.ROOM_GAP) - midy, 0f);
    }

    //This function makes no sense at all, but it works...
    public void UpdateRoom(int x, int y) {
        RoomController current = rooms[x, y];
        int[] f = from(x, y, current.cameFrom);
        //RoomController cameFrom = rooms[f[0], f[1]];

        //Debug.Log("New Room  " + x + ", " + y + " came from " + current.cameFrom + ", which is room " + f[0] + ", " + f[1]);

        if(current.cameFrom == 0) {
            mapRooms[x, y].questionMarks.n = false;
            mapRooms[f[0], f[1]].questionMarks.s = false;
        } else if(current.cameFrom == 1) {
            mapRooms[x, y].questionMarks.s = false;
            mapRooms[f[0], f[1]].questionMarks.n = false;
        } else if(current.cameFrom == 2) {
            mapRooms[x, y].questionMarks.e = false;
            mapRooms[f[0], f[1]].questionMarks.w = false;
        } else if(current.cameFrom == 3) {
            mapRooms[x, y].questionMarks.w = false;
            mapRooms[f[0], f[1]].questionMarks.e = false;
        }
    }

    public int[] from(int x, int y, int cameFrom) {
        int[] res = new int[2];

        res[0] = x;
        res[1] = y;

        if(cameFrom == 0) {
            res[0] = x + 1;
            res[1] = y;
        } else if(cameFrom == 1) {
            res[0] = x - 1;
            res[1] = y;
        } else if(cameFrom == 2) {
            res[0] = x;
            res[1] = y - 1;
        } else if(cameFrom == 3) {
            res[0] = x;
            res[1] = y + 1;
        }

        return res;
    }
}
