using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public class LayoutManager : EditorWindow {

	[MenuItem("Layout/Load Remote/Automatically #%l", false, 1)]
	static void Auto() {
		if(System.Environment.UserName == "151346"
			|| System.Environment.UserName == "oisan"
			|| System.Environment.UserName == "Jonas") {
			Jonas();
			Debug.Log("Welcome back, Jonas!");
		} else if(System.Environment.UserName == "DISABLED_SCHOOL_NUMBER"
			|| System.Environment.UserName == "AgustDISABLED") {
			Agust();
			Debug.Log("Welcome back, Agust!");
		}  else if(System.Environment.UserName == "151354") {
			Jonas();
			Debug.Log("Welcome back, Adrian!");
		} else {
			Debug.LogError("No Layout found for you. Try selecting one manually: Layout -> Load Remote -> LAYOUT_NAME");
		}
	}

	[MenuItem("Layout/Load Remote/Jonas", false, 100)]
	static void Jonas() {
		DownloadLayout("http://colhatteraldamage.web44.net/TheMazeGame/Jonas.wlt", "Jonas");
	}

	[MenuItem("Layout/Load Remote/Agust", false, 101)]
	static void Agust() {
		DownloadLayout("http://colhatteraldamage.web44.net/TheMazeGame/Jonas.wlt", "Agust");
	}

	static void DownloadLayout(string url, string name) {
		string path = Application.temporaryCachePath + "\\" + name + ".wlt";
		using(System.Net.WebClient client = new System.Net.WebClient()) {
			client.DownloadFile(new System.Uri(url), path);
		}
		LayoutUtility.LoadLayoutFromAsset(path);
	}
}
