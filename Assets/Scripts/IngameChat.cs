using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections.Generic;

public class IngameChat : NetworkBehaviour {
    public static bool showChat {
        get;
        private set;
    }
	[Range(1, 10000)]
	public int maxChatMessages = 200;
    private int oldIndex = -1;
    public List<string> oldMessages = new List<string>();

    private InputField IF;
    private Image bg;

    [SerializeField]
    private RectTransform content;
    [SerializeField]
    private GameObject messagePrefab;
	[SerializeField]
	private PlayerSync player;
	[SerializeField]
	private ScrollRect sr;
	[SerializeField]
	private static UnetChat unetChat;

    void Start() {
        IF = GetComponent<InputField>();
        bg = GetComponent<Image>();
		unetChat = GetComponentInParent<UnetChat>();
        showChat = false;
        oldMessages.Add("");
    }
	
	void Update() {
		if(player == null) {
			foreach(PlayerController p in GameObject.FindObjectsOfType<PlayerController>()) {
				if(p.isLocal()) {
					player = p.GetComponentInParent<PlayerSync>();
				}
			}
		}

        if(Input.GetKeyDown(KeyCode.T) && !IF.isFocused) {
            showChat = !showChat;
            updateChat();
			IF.ActivateInputField();
            IF.Select();
        }

        if(IF.isFocused) {
            int dir = 0;
            if(Input.GetKeyDown(KeyCode.UpArrow))
                dir = 1;
            if(Input.GetKeyDown(KeyCode.DownArrow))
                dir = -1;

            if(oldMessages.Count != 0 && dir != 0) {
                int index = Mathf.Clamp(oldIndex + dir, 0, oldMessages.Count);
                if(index == oldMessages.Count)
                    index = 0;
                IF.text = oldMessages[index];
                oldIndex = index;
				IF.selectionAnchorPosition = IF.text.Length;
				IF.selectionFocusPosition = IF.text.Length;
            }
        }

        if(bg.enabled != showChat) {
            updateChat();
        }

		if(content.transform.childCount > maxChatMessages) {
			Destroy(content.transform.GetChild(content.transform.childCount - 1).gameObject);
		}
	}

    private void updateChat() {
        bg.enabled = showChat;
        IF.enabled = showChat;
		sr.vertical = showChat;
		sr.GetComponent<Image>().enabled = showChat;
        oldIndex = -1;
    }

	public static void postLoginLogOut(bool login, string user, Color col) {
		string action = login ? "joined the game.                                                                                                                                 "
							  : "left the game.                                                                                                                                 ";
		unetChat.SendChat(user + " " + action, "", col);
	}

    public void postMessage(string s) {
		showChat = false;

		if(player == null || string.IsNullOrEmpty(s) || IF.wasCanceled) {
			IF.text = "";
			return;
		}

        string u = player.getUsername();
        Color c = player.getUserColor();

        oldMessages.Insert(0, s);

        //Commands
        if(s.StartsWith("/")) {
            string[] arguments = s.Split(" ".ToCharArray());

            if(s.StartsWith("/render ")){
                float dist = 150f;

                if(arguments[1] == null) {
                    IF.DeactivateInputField();
                    IF.text = "";
                    return;
                }

                float.TryParse(arguments[1], out dist);
                dist = Mathf.Clamp(dist, 0f, 1000f);
                GameObject.FindObjectOfType<Cameraman>().renderDistance = dist;
                AddMessage("Set your render distance to " + dist + ".                                                                                                                  ", "", c);
            } else if(s.StartsWith("/help")) {
				AddMessage("List of commands:                                                                                                                  ", "", c);
				AddMessage(" - /admin : Toggles Admin mode                                                                                                                  ", "", c);
				AddMessage(" - /render [distance]: Sets the render distance to [distance]                                                                                                                  ", "", c);
				AddMessage(" - /var [obj] [OPTIONAL script] [variable] [value]: Sets a variable.                                                                                                                ", "", c);
				AddMessage(" - /tutorial : Toggles the ability to skip the tutorial the next time.                                                                                                                  ", "", c);
			} else if(s.StartsWith("/admin")) {
                player.isAdmin = !player.isAdmin;
                AddMessage("Toggled admin mode.                                                                                                                  ", "", c);
			} else if(s.StartsWith("/tutorial")) {
				string ver = NewTutController.instance.getTutorialVersion() + "tutorialCompleted";
				if(PlayerPrefs.HasKey(ver)) {
					PlayerPrefs.DeleteKey(ver);
					PlayerPrefs.Save();
					AddMessage("You lose your experience gained from doing the tutorial.                                                                                                                  ", "", c);
				} else {
					PlayerPrefs.SetInt(ver, 1);
					PlayerPrefs.Save();
					AddMessage("You gain the experience from doing the tutorial, somehow.                                                                                                                  ", "", c);
				}
			} else if(s.StartsWith("/var ")) {
				string[] args = s.Split(" ".ToCharArray());
				//string obj = args[1];
				//string variable = args[2];
				//string value = args[3];
				//string value2 = args[4];

				if(!string.IsNullOrEmpty(args[1])) {
					if(args[1].ToLower() == "this.room") {
						RoomController rc = player.getController().currentRoomController;
						if(!string.IsNullOrEmpty(args[2])) {
							if(args[2].ToLower() == "puzzlecomplete") {
								if(!string.IsNullOrEmpty(args[3])) {
									if(args[3].ToLower() == "true" || args[3].ToLower() == "false" || args[3].ToLower() == "0" || args[3].ToLower() == "1") {
										bool val = args[3].ToLower() == "true" || args[3].ToLower() == "1";
										rc.puzzleComplete = val;
										AddMessage("Set the variable 'puzzleComplete' to '" + val.ToString().ToLower() + "'.                                                                                                                  ", "", c);
									} else {
										AddMessage("Error: puzzleComplete is a bool.                                                                                                                  ", "", c);
									}
								} else {
									AddMessage("Value not found.                                                                                                                  ", "", c);
								}
							} else {
								AddMessage("Variable not found.                                                                                                                  ", "", c);
							}
						} else {
							AddMessage("Variable not found.                                                                                                                  ", "", c);
						}
					} else {
						AddMessage("GameObject not found.                                                                                                                  ", "", c);
					}
				} else {
					AddMessage("No GameObject to look for.                                                                                                                  ", "", c);
				}

			}

            IF.DeactivateInputField();
            IF.text = "";
            return;
        }

		unetChat.SendChat(u, s, c);
		IF.DeactivateInputField();
        IF.text = "";
    }
    
    public void AddMessage(string u, string s, Color c) {
        GameObject message = Instantiate(messagePrefab, Vector3.zero, Quaternion.identity) as GameObject;
        message.transform.SetParent(content);
        message.transform.SetAsFirstSibling();
        message.transform.localScale = new Vector3(1f, -1f, 1f);
        Image dot = message.transform.Find("Dot").GetComponent<Image>();
        dot.color = c;
        Text name = message.transform.Find("Username").GetComponent<Text>();
        name.text = " " + u + ": ";
        Text cont = message.transform.Find("Message").GetComponent<Text>();
        cont.text = s;
		message.name = u + ": " + (s.Length > 10 ? s.Remove(10) : s);
    }

}
