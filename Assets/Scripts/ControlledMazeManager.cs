using UnityEngine;

[ExecuteInEditMode]
public class ControlledMazeManager : MonoBehaviour {

    [System.Serializable]
    public class RoomsRow {
        public RoomObject[] column;
        public RoomsRow(int i) {
            this.column = new RoomObject[i];
        }
    }

    public bool spawnWithAllRoomsOpen = false;
	[HideInInspector]
    public RoomsRow[] rows;

    private MazeManager manager;
    private int sizeX, sizeY;

    void Awake() {
        manager = GetComponent<MazeManager>();
        sizeX = Mathf.RoundToInt(manager.mazeSize.x);
        sizeY = Mathf.RoundToInt(manager.mazeSize.y);
    }
	
	void Update() {
        int x = Mathf.RoundToInt(manager.mazeSize.x);
        int y = Mathf.RoundToInt(manager.mazeSize.y);

        if(sizeX != x || sizeY != y) {
            sizeX = x;
            sizeY = y;
            UpdateGrid();
        }
	}

    public void UpdateGrid() {
        rows = new RoomsRow[sizeY];
        for(int i = 0; i < rows.Length; i++) {
            rows[i] = new RoomsRow(sizeX);
        }
    }

	public MazeManager getManager() {
		return manager;
	}
}
