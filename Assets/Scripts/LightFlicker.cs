using UnityEngine;

public class LightFlicker : MonoBehaviour {
	public float range = 0.5f;
	public float speed = 0.5f;

	private Light light;
	private float startIntensity;
	private bool increasing = false;

	private float intensity = 0f;

	void Start() {
		light = GetComponent<Light>();
		startIntensity = light.intensity;
		InvokeRepeating("UpdateFlicker", 0f, speed);
	}

	void Update() {
        if(light == null)
            return;
        if(!light.enabled)
            return;
		light.intensity = Mathf.Lerp(light.intensity, intensity, speed * Time.deltaTime);
	}

	void UpdateFlicker() {
		intensity = startIntensity + Random.Range(-range, range);
	}
}
