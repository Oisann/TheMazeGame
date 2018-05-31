using System;
using System.Text;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;

public class AnalyticsController : MonoBehaviour {
    public static string uniqueIdentifier { get; private set; }
	public static AnalyticsController instance;

	private static string URL = "https://skatebrus.oisann.net/log.php";
	private const string secret = "5C7F4D50F1F482B8513FB13ED90137680562811EB396DED1BA59D185818C9A65C9F1460E68E1355A87E5544B528B9CF89FD6F6117C7459145F701F675453E662";

    void Awake() {
		instance = this;

		if(PlayerPrefs.HasKey("isFirstTime")) {
			PlayerPrefs.DeleteAll();
			PlayerPrefs.SetInt ("isFirstTime", 1);
		}

		if(PlayerPrefs.HasKey("uniqueIdentifier")) {
			uniqueIdentifier = PlayerPrefs.GetString("uniqueIdentifier");
        } else {
            uniqueIdentifier = RegisterNewUniqueID();
			PlayerPrefs.SetString("uniqueIdentifier", uniqueIdentifier);
        }
		PlayerPrefs.Save();
    }

    public static string RegisterNewUniqueID() {
        WWWForm form = new WWWForm();
        Dictionary<string, string> data = new Dictionary<string, string>();
        data.Add("Category", "Users");
        data.Add("Unity", Application.isEditor ? Application.unityVersion : "-1");
        data.Add("Version", Application.version);
        data.Add("Platform", Application.platform.ToString());
        data.Add("Language", Application.systemLanguage.ToString());
        data.Add("TimeZone", System.TimeZone.CurrentTimeZone.StandardName);
        data.Add("MachineName", System.Environment.MachineName);
        data.Add("UserName", System.Environment.UserName);
        data.Add("Processor", SystemInfo.processorType);
        data.Add("ProcessorFrequency", SystemInfo.processorFrequency.ToString());
        data.Add("ProcessorCount", System.Environment.ProcessorCount.ToString());
        data.Add("GPU", SystemInfo.graphicsDeviceName);
        data.Add("OS", SystemInfo.operatingSystem);
        data.Add("OSFamily", SystemInfo.operatingSystemFamily.ToString());
		data.Add("HostName", System.Environment.UserDomainName);
        data.Add("Timestamp", getCurrentTimestamp().ToString());
        data.Add("RandomNumber", new System.Random().Next(int.MinValue, int.MaxValue).ToString());

        string verified = VerifiedHash(data, ref form);
        form.AddField("verify", verified);
		IEnumerator post = PostForm(URL, form);
		instance.StartCoroutine(post);
		return verified;
    }

    public static string GenerateMazeId(string seed, Vector2 size) {
        WWWForm form = new WWWForm();
        Dictionary<string, string> data = new Dictionary<string, string>();
        data.Add("Category", "Mazes");
        data.Add("Seed", seed);
        data.Add("SizeX", Mathf.RoundToInt(size.x).ToString());
        data.Add("SizeY", Mathf.RoundToInt(size.y).ToString());
        data.Add("Timestamp", getCurrentTimestamp().ToString());
        data.Add("HostID", uniqueIdentifier);
        data.Add("RandomNumber", new System.Random().Next(int.MinValue, int.MaxValue).ToString());
        data.Add("Unity", Application.isEditor ? Application.unityVersion : "-1");
        data.Add("HostPcInfo", GenerateComputerInfoJson());

        string verified = VerifiedHash(data, ref form);
        form.AddField("verify", verified);
        IEnumerator post = PostForm(URL, form);
        instance.StartCoroutine(post);
        return verified;
    }

    public static void JoinMaze(string username, Color color, string mazeID) {
        WWWForm form = new WWWForm();
        Dictionary<string, string> data = new Dictionary<string, string>();
        data.Add("Category", "MazeJoins");
        data.Add("Username", username);
        data.Add("Color", color.ToString());
        data.Add("Timestamp", getCurrentTimestamp().ToString());
        data.Add("User", uniqueIdentifier);
        data.Add("MazeID", mazeID);
        data.Add("Unity", Application.isEditor ? Application.unityVersion : "-1");
        data.Add("HostPcInfo", GenerateComputerInfoJson());

        string verified = VerifiedHash(data, ref form);
        form.AddField("verify", verified);
        form.AddField("no_id", "1");
        IEnumerator post = PostForm(URL, form);
        instance.StartCoroutine(post);
    }

    public static string GenerateComputerInfoJson() {
        string res = "{ ";
        res += "\"Platform\": \"" + Application.platform.ToString() + "\", ";
        res += "\"Language\": \"" + Application.systemLanguage.ToString() + "\", ";
        res += "\"TimeZone\": \"" + System.TimeZone.CurrentTimeZone.StandardName + "\", ";
        res += "\"MachineName\": \"" + System.Environment.MachineName + "\", ";
        res += "\"UserName\": \"" + System.Environment.UserName + "\", ";
        res += "\"Processor\": \"" + SystemInfo.processorType + "\", ";
        res += "\"ProcessorFrequency\": \"" + SystemInfo.processorFrequency.ToString() + "\", ";
        res += "\"ProcessorCount\": \"" + System.Environment.ProcessorCount.ToString() + "\", ";
        res += "\"GPU\": \"" + SystemInfo.graphicsDeviceName + "\", ";
        res += "\"OS\": \"" + SystemInfo.operatingSystem + "\", ";
        res += "\"OSFamily\": \"" + SystemInfo.operatingSystemFamily.ToString() + "\", ";
        res += "\"HostName\": \"" + System.Environment.EmbeddingHostName + "\" }";
        return res;
    }

	private static IEnumerator PostForm(string url, WWWForm form) {
		WWW www = new WWW(url + "?z=" + getCurrentTimestamp(), form.data);
		yield return www;
		Debug.Log(www.text);
	}

    private static int getCurrentTimestamp() {
        return (int) (System.DateTime.UtcNow.Subtract(new System.DateTime(1970, 1, 1))).TotalSeconds;
    }
    public static string SHA512(string unhashedPassword, string salt = null) {
        string toHash = unhashedPassword;
        if(salt != null) {
            toHash += salt;
            toHash = salt + toHash;
        }
        return BitConverter.ToString(new SHA512Managed().ComputeHash(Encoding.Default.GetBytes(toHash))).Replace("-", String.Empty).ToUpper();
    }
    private static string VerifiedHash(Dictionary<string, string> data) {
        WWWForm del = new WWWForm();
        string v = VerifiedHash(data, ref del);
        return v;
    }
    private static string VerifiedHash(Dictionary<string, string> data, ref WWWForm form) {
        string verify = "";
        foreach(KeyValuePair<string, string> entry in data) {
            verify += entry.Key + "=" + entry.Value + ", ";
            form.AddField(entry.Key, entry.Value);
        }
        return SHA512(verify, secret);
    }
}
