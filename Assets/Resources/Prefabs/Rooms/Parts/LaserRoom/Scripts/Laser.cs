using UnityEngine;

public class Laser : MonoBehaviour {
	public LineRenderer line;
	public Light LaserLight;
	private Transform endParticle;

	private Vector3 lastHit = Vector3.zero;
	private Transform lightsParent;

	void Start() {
		line = GetComponent<LineRenderer>();
		endParticle = transform.GetChild(0);
		lightsParent = transform.GetChild(1);
	}

	void FixedUpdate() {
		RaycastHit hit;
		if(Physics.Raycast(transform.position, -transform.right, out hit, 100)) {
			Vector3[] points = new Vector3[2];
			points [0] = transform.position;
			points [1] = hit.point;
			Mirror m = hit.transform.GetComponent<Mirror>();

			float distFromLast = Vector3.Distance(lastHit, hit.point);
			if(distFromLast >= .1f)
				UpdateLights(transform.position, hit.point);

			if (m == null) {
				endParticle.gameObject.SetActive(true);
				endParticle.position = hit.point;
				endParticle.rotation = Quaternion.LookRotation((hit.point - transform.position).normalized * -1f);
			} else {
				if (m.server)
					m.openRoom();
				endParticle.gameObject.SetActive(false);
				m.hit = hit;
				m.frm = transform.position;
			}
			line.SetPositions(points);
			lastHit = hit.point;
		}
	}

	private void UpdateLights(Vector3 start, Vector3 end) {
		if (LaserLight == null)
			return;
		//Delete old lights
		for(int i = 0; i < lightsParent.childCount; i++) {
			Destroy(lightsParent.GetChild(i).gameObject);
		}

		float intensity = LaserLight.range / 2f;
		float distance = Vector3.Distance(start, end);
		int count = Mathf.FloorToInt(distance / intensity);
		Vector3 direction = (end - start).normalized;

		for(int i = 0; i < count; i++) {
			GameObject light = Instantiate(LaserLight.gameObject, Vector3.zero, Quaternion.identity) as GameObject;
			light.transform.SetParent(lightsParent);
			light.transform.position = lightsParent.position;
			light.transform.localScale = Vector3.one;
			light.transform.position = start + ((direction * (distance / count)) * i);
		}
	}
}
