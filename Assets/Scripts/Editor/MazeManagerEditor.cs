using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MazeManager))]
public class MazeManagerEditor : Editor {
	private MazeManager manager;
	private string lastSeed = "";

	public void Start() {
		manager = (MazeManager) target;
	}

	public override void OnInspectorGUI() {
		MazeManager manager = (MazeManager) target;

		if(GUILayout.Button ("Use last seed")) {
			if(PlayerPrefs.HasKey("lastSeed"))
				lastSeed = PlayerPrefs.GetString("lastSeed");
			manager.mazeSeed = lastSeed;
		}

		if(GUILayout.Button ("Reset seed")) {
			manager.mazeSeed = "";
		}

		DrawDefaultInspector();
	}
}
