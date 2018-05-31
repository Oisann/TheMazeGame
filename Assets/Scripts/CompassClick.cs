using UnityEngine;
using UnityEngine.UI;

public class CompassClick : MonoBehaviour {
	private Button button;
	private Cameraman cameraman;
	private Compass compass;

	void Start() {
		button = GetComponent<Button>();
		button.onClick.AddListener(() => {
			ResetRotation();
		});
		compass = transform.parent.GetComponentInChildren<Compass>();
		cameraman = compass.getCameraman();
	}

	void Update() {
		if(compass != null) {
			if(cameraman == null) {
				cameraman = compass.getCameraman ();
			}
		}
	}

	public void ResetRotation() {
		if(cameraman == null)
			return;
		cameraman.RotationsTransform.eulerAngles = new Vector3(cameraman.RotationsTransform.eulerAngles.x, 90f, cameraman.RotationsTransform.eulerAngles.z);
	}
}
