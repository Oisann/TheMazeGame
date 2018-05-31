using UnityEditor;
using UnityEngine;

public class OisannTools : EditorWindow {

	[MenuItem("Tools/Oisann Tools/Clear PlayerPrefs", false, 1)]
	static void ClearPlayerPrefs() {
		PlayerPrefs.DeleteAll();
	}
}