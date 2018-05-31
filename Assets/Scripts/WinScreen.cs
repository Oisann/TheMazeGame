using UnityEngine;
using UnityEngine.UI;

public class WinScreen : MonoBehaviour {
	public int timeInSeconds = 0;
	public int roomsOpened = 0;
	public int roomCount = 0;
	public int puzzlesCompleted = 0;
	public int puzzleCount = 0;

	public int sizeX = 0, sizeY = 0;
	public string mazeSeed = "";

	[Header("Settings")]
	public string timeString, roomsString, puzzlesString, sizeString, seedString, scoreString;

	private Image background;
	private Text time, rooms, puzzles, size, seed, score;
	private Color startColor = new Color(0f, 0f, 0f, 0f);
	private Color endColor = new Color(0f, 0f, 0f, .95f);

	void Start() {
		background = transform.GetChild(0).GetComponent<Image>();
		time = transform.GetChild(2).GetComponent<Text>();
		rooms = transform.GetChild(3).GetComponent<Text>();
		puzzles = transform.GetChild(4).GetComponent<Text>();
		size = transform.GetChild(5).GetComponent<Text>();
		seed = transform.GetChild(6).GetComponent<Text>();
		score = transform.GetChild(7).GetComponent<Text>();

		//WTF??
		string roomPercent = roomCount == 0 ?  "0" : System.Math.Round(((double) roomsOpened / (double) roomCount) * 100d, 2) + "";
		string puzzlePercent = puzzleCount == 0 ? "0" : System.Math.Round(((double) puzzlesCompleted / (double) puzzleCount) * 100d, 2) + "";

		//Didnt work, so it's disabled for now.
		int scoreCount = 0;

		time.text = timeString + ": " + string.Format("{0:00}:{1:00}:{2:00}", timeInSeconds / 3600, (timeInSeconds / 60) % 60, timeInSeconds % 60);
		rooms.text = roomsString + ": " + roomsOpened + "/" + roomCount + " (" + roomPercent + "%)";
		puzzles.text = puzzlesString + ": " + puzzlesCompleted + "/" + puzzleCount + " (" + puzzlePercent + "%)";
		size.text = sizeString + ": " + sizeX + " x " + sizeY;
		seed.text = seedString + ": " + mazeSeed;
		score.text = "";
		//score.text = scoreString + ": " + scoreCount;

		//AnalyticsController.DoAction("mazeCompleted", "Seconds: " + timeInSeconds + ", Rooms: " + roomsOpened + "/" + roomCount + ", Puzzles: " + puzzlesCompleted + "/" + puzzleCount + ", Size: " + sizeX + " x " + sizeY + ", Seed: " + mazeSeed);

		background.color = startColor;
	}

	void FixedUpdate() {
		background.color = Color.Lerp(background.color, endColor, Time.fixedDeltaTime);
	}
}
