using UnityEngine;
using UnityEngine.UI;

public class PlayerIndicators : MonoBehaviour {

    public MapController controller;
    public PlayerController player;

    private Image image;
    private GameObject tooltip;

    void Start() {
        image = GetComponent<Image>();
		controller = transform.parent.GetComponentInParent<MapController>();
        tooltip = transform.GetChild(0).gameObject;
        tooltip.SetActive(false);
    }

    void Update() {
        if(controller != null) {
			if(player == null) {
				if(!controller.unusedP.Contains(gameObject)) {
                	controller.unusedP.Add(gameObject);
					gameObject.SetActive(false);
				}
            }
        }

        if(player != null) {
            image.color = player.GetComponentInParent<PlayerSync>().getUserColor();
            tooltip.GetComponent<Text>().text = player.GetComponentInParent<PlayerSync>().getUsername();
        }
    }

    public void MouseEnter() {
        tooltip.SetActive(true);
    }

    public void MouseExit() {
        tooltip.SetActive(false);
    }
}
