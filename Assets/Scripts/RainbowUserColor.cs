using UnityEngine;

public class RainbowUserColor : MonoBehaviour {

    private TextMesh text;
    private Color currentColor;
    private Color[] colors = { Color.red, Color.yellow, Color.green, Color.blue, Color.cyan };

    void Start () {
        text = GetComponent<TextMesh>();
        InvokeRepeating("setRandomColor", 0f, 1.5f);
    }
	
	void Update() {
        text.color = Color.Lerp(text.color, currentColor, 1f * Time.deltaTime);
    }

    void setRandomColor() {
        currentColor = colors[Random.Range(0, colors.Length)];
    }
}
