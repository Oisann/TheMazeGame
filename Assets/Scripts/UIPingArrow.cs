using UnityEngine;
using UnityEngine.UI;

public class UIPingArrow : MonoBehaviour {

    public PingController ping;

    private Camera cam;
    private PlayerController player;
    private RawImage renderr;
	private Text distance;
	
    void Start() {
        renderr = GetComponentInChildren<RawImage>();
		distance = renderr.GetComponentInChildren<Text>();
    }

	void Update() {
        if(cam == null) {
            foreach(PlayerController p in GameObject.FindObjectsOfType<PlayerController>()) {
                if(p.isLocal()) {
                    player = p;
                    cam = p.getCameraman().cam;
                }
            }
        }

        if(ping != null) {
            Vector3 screenPos = cam.WorldToViewportPoint(ping.transform.position);
            if(renderr.color != ping.arrowColor)
                renderr.color = ping.arrowColor;

            if(screenPos.x >= 0 && screenPos.x <= 1 && screenPos.y >= 0 && screenPos.y <= 1) {
                if(renderr.enabled)
                    renderr.enabled = false;
				distance.text = "";
                return;
            }

            if(!renderr.enabled)
                renderr.enabled = true;

            if(screenPos.z < 0f) {
                screenPos *= -1f;
            }

            Vector2 onScreenPos = new Vector2(screenPos.x - 0.5f, screenPos.y - 0.5f) * 2;
            float max = Mathf.Max(Mathf.Abs(onScreenPos.x), Mathf.Abs(onScreenPos.y));
            onScreenPos = (onScreenPos / (max * 2)) + new Vector2(0.5f, 0.5f);

            Vector3 screenPoint = cam.ViewportToScreenPoint(onScreenPos);
            Vector3 screenCenter = new Vector3(Screen.width, Screen.height, 0f) / 2f;
            float angle = Mathf.Atan2(screenPoint.y - screenCenter.y, screenPoint.x - screenCenter.x);
            angle -= 90 * Mathf.Deg2Rad;

            Vector3 distanceToCenter = screenPoint - screenCenter;
            distanceToCenter *= 0.2f;
            screenPoint -= distanceToCenter;

            renderr.rectTransform.anchoredPosition = screenPoint;
            renderr.rectTransform.rotation = Quaternion.Euler(0f, 0f, angle * Mathf.Rad2Deg);

			if (onScreenPos.x >= .5f) {
				distance.rectTransform.localScale = new Vector3 (-1f, -1f, 1f);
				distance.alignment = TextAnchor.MiddleRight;
			} else {
				distance.rectTransform.localScale = new Vector3 (1f, 1f, 1f);
				distance.alignment = TextAnchor.MiddleLeft;
			}
			float dist = Vector3.Distance(ping.transform.position, player.transform.position/*cam.ScreenToWorldPoint(screenPoint)*/);

			distance.text = System.Math.Round(dist, 2) + "m";


            //SOME STUFF FROM YOUTUBE THAT DIDNT WORK
            /*
            Vector3 screenpos = cam.WorldToViewportPoint(pingLocation.position);

            if(screenpos.z > 0f && screenpos.x > 0f && screenpos.y > 0f && screenpos.x < Screen.width && screenpos.y < Screen.height) {
                //The arrow is on the screen, we don't have to draw a UI element.
                renderr.enabled = false;
                return;
            } else {
                if(!renderr.enabled)
                    renderr.enabled = true;

                if(screenpos.z < 0f) {
                    //Flip it when it is behind us
                    screenpos *= -1f;
                }

                Vector3 screenCenter = new Vector3(Screen.width, Screen.height, 0f) / 2f;

                //make 0,0 the center of the screen
                screenpos -= screenCenter;

                float angle = Mathf.Atan2(screenpos.y, screenpos.x);
                angle -= 90 * Mathf.Deg2Rad;

                float cos = Mathf.Cos(angle);
                float sin = -Mathf.Sin(angle);

                screenpos = screenCenter + new Vector3(sin * 150f, cos * 150f, 0f);

                float m = cos / sin;

                Vector3 screenBounds = screenCenter * 0.9f;

                if(cos > 0f) {
                    screenpos = new Vector3(screenBounds.y / m, screenBounds.y, 0f);
                } else {
                    screenpos = new Vector3(-screenBounds.y / m, -screenBounds.y, 0f);
                }

                if(screenpos.x > screenBounds.x) {
                    screenpos = new Vector3(screenBounds.x, screenBounds.x * m, 0f);
                } else if(screenpos.x < -screenBounds.x) {
                    screenpos = new Vector3(-screenBounds.x, -screenBounds.x * m, 0f);
                }

                screenpos += screenCenter;

                renderr.rectTransform.anchoredPosition = screenpos;
                renderr.rectTransform.localRotation = Quaternion.Euler(0f, 0f, angle * Mathf.Rad2Deg);
            } */
        } else {
            if(renderr.enabled)
                renderr.enabled = false;
			distance.text = "";
        }
	}
}
