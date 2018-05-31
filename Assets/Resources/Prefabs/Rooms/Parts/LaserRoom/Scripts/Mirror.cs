using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;

public class Mirror : NetworkBehaviour {
	public RaycastHit hit;
	public Vector3 frm;
	public bool server = false;


	private Laser laser;

	void Start() {
		//Debug.Log("Spawned mirror");
		hit = new RaycastHit();
		hit.point = new Vector3(-.4324234f, -232.42354f, 111.0032034f);

		if(transform.CompareTag("TutBlock")) {
			Invoke("setServer", 2f);

			if(isServer) {
				Transform spawnpoints = transform.parent.Find("Button Spawnpoints");
				System.Random rand = new System.Random((transform.parent.name + "-" + spawnpoints.childCount + "-" + transform.name).GetHashCode());
				int i = rand.Next(0, spawnpoints.childCount);
				transform.position = spawnpoints.GetChild(i).position;
			}

			return;
		}
		laser = transform.GetChild(0).GetComponent<Laser>();
		if(isServer) {
			Transform spawnpoints = transform.parent.Find("Mirror Spawnpoints");
			System.Random rand = new System.Random((transform.parent.name + "-" + spawnpoints.childCount + "-" + transform.name).GetHashCode());
			int indexChild = 0;
			string[] n = transform.name.Split(" ".ToCharArray());
			int.TryParse(n[n.Length-1], out indexChild);
			transform.position = spawnpoints.GetChild(indexChild).position;

			int rot = rand.Next(0, 16);
			transform.eulerAngles = new Vector3(0f, 22.5f * rot, 0f);
		}
	}

	private Renderer prism;

	void FixedUpdate() {
		if(transform.CompareTag("TutBlock")) {
			if(prism == null)
				prism = transform.GetComponentInChildren<Renderer>();
			
			if(hit.point != new Vector3(-.4324234f, -232.42354f, 111.0032034f)) {
				openRoom();
				prism.material.SetFloat("_HitByLaser", 1);
			} else {
				prism.material.SetFloat("_HitByLaser", 0);
			}
			return;
		}

		if(hit.point != new Vector3(-.4324234f, -232.42354f, 111.0032034f)) {
			Vector3 fromDirection = (hit.point - frm).normalized;
			float angle = Vector3.Angle(fromDirection, -transform.forward);
			Vector3 cross = Vector3.Cross(fromDirection, -transform.forward);

			if(cross.y < 0)
				angle *= -1;


			laser.transform.localPosition = transform.InverseTransformPoint(hit.point);
			laser.transform.rotation = Quaternion.LookRotation(transform.right);
			laser.transform.localEulerAngles += new Vector3(0f, angle, 0f);

			float dot = Vector3.Dot(fromDirection, transform.forward);
			if(dot <= -0.95f) {
				laser.gameObject.SetActive(false);
			} else {
				laser.gameObject.SetActive(true);
			}
		} else {
			laser.gameObject.SetActive(false);
		}
		hit = new RaycastHit();
		hit.point = new Vector3(-.4324234f, -232.42354f, 111.0032034f);
	}

	void setServer() {
		server = isServer;
	}

	public void openRoom() {
		if (!server)
			return;
		transform.parent.parent.GetComponent<RoomController>().puzzleComplete = true;
		server = false;
	}
}
