using UnityEngine;

public class DisableOnPlay : MonoBehaviour {

	void Awake() {
		DestroyImmediate(this.gameObject);
	}
}
