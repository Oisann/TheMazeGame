using UnityEngine;
using System.Collections;

public class FPSDisplay : MonoBehaviour {
    public Color textColor = Color.yellow;

    private bool show = true;
    private float deltaTime = 0.0f;

    void Start() {
        /*if(!PlayerPrefs.HasKey("FPSDisplay"))
            toggleFPSCounter();*/
        show = true; //PlayerPrefs.GetInt("FPSDisplay") == 1;
    }

    void Update() {
        if(Time.timeScale != 0f)
            deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
    }

    void OnGUI() {
        if(!show)
            return;
        int w = Screen.width, h = Screen.height;

        GUIStyle style = new GUIStyle();

        Rect rect = new Rect(0, 0, w, h * 2 / 100);
        style.alignment = TextAnchor.UpperLeft;
        style.fontSize = h * 2 / 100;
        style.normal.textColor = textColor;
        float msec = deltaTime * 1000.0f;
        float fps = 1.0f / deltaTime;
        string text = string.Format(" {0:0.0} ms ({1:0.} fps)", msec, fps);
        GUI.Label(rect, text, style);
    }

    public void toggleFPSCounter() {
        show = !show;
        //PlayerPrefs.SetInt("FPSDisplay", show ? 1 : 0);
    }
}