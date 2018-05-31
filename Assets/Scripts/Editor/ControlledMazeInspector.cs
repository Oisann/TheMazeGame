using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ControlledMazeManager))]
public class ControlledMazeInspector : Editor {
    private ControlledMazeManager manager;
    private ControlledMazeEditor window;
    private string lastSeed = "";

    public void Start() {
        manager = (ControlledMazeManager) target;
    }

    public override void OnInspectorGUI() {
        ControlledMazeManager manager = (ControlledMazeManager) target;

        if(window != null) {
            DrawDefaultInspector();
        } else {
            if(GUILayout.Button("Show Editor")) {
				window = (ControlledMazeEditor) EditorWindow.GetWindow(typeof(ControlledMazeEditor));
                window.Show();
            }
        }
		EditorUtility.SetDirty(manager);
    }
}
