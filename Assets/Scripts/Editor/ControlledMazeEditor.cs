using UnityEngine;
using UnityEditor;
public class ControlledMazeEditor : EditorWindow {
	public ControlledMazeManager manager;

	[MenuItem ("Window/Controlled Maze")]
	static void Init() {
		ControlledMazeEditor window = (ControlledMazeEditor) EditorWindow.GetWindow(typeof(ControlledMazeEditor));
		window.name = "Controlled Maze Manager";
		window.Show();
	}

	void ClearMaze() {
		if(manager == null)
			return;
		for(int y = manager.rows.Length - 1; 0 <= y; y--) {
			for(int x = 0; x < manager.rows[y].column.Length; x++) {
				manager.rows[y].column[x] = null;
			}
		}
	}

    void OnGUI() {
		if(manager == null) {
			manager = GameObject.FindObjectOfType<ControlledMazeManager>();
		}
		GUILayout.Label("Controlled Maze Controller: " + (manager == null ? "N/A" : "Found (" + manager.getManager().mazeSize.x + "x" + manager.getManager().mazeSize.y + ")"), EditorStyles.boldLabel);

		//We couldn't find it, so we'll try again next update
		if(manager == null)
			return;

		EditorGUILayout.BeginHorizontal();
		GUILayout.Label("   W");
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.BeginHorizontal();
		GUILayout.Label("S + N");
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.BeginHorizontal();
		GUILayout.Label("   E");
		EditorGUILayout.EndHorizontal();

		for(int y = manager.rows.Length - 1; 0 <= y; y--) {
			EditorGUILayout.BeginHorizontal();
			for(int x = 0; x < manager.rows[y].column.Length; x++) {
				manager.rows[y].column[x] = (RoomObject) EditorGUILayout.ObjectField(manager.rows[y].column[x], typeof(RoomObject));
			}
			EditorGUILayout.EndHorizontal();
		}

		EditorGUILayout.BeginHorizontal();
		if(GUILayout.Button("Clear Maze")) {
			int option = EditorUtility.DisplayDialogComplex("Are you sure?",
				"What do you REALLY want to clear the maze?",
				"Yes",
				"No",
				"Cancel");

			switch(option) {
			// Yes
			case 0:
				ClearMaze();
				break;

				// No
			case 1:
				break;

				// Cancel
			case 2:
				break;

			default:
				Debug.LogError("Unrecognized option.");
				break;
			}
		}
		EditorGUILayout.EndHorizontal();
		EditorUtility.SetDirty(manager);
	}
}