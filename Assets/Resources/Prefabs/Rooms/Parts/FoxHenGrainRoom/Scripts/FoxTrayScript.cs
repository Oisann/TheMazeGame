using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class FoxTrayScript : MonoBehaviour {

	public GameObject foxTarPos;
	public GameObject grainTarPos;
	public GameObject henTarPos;

	public GameObject foxStartPos;
	public GameObject grainStartPos;
	public GameObject henStartPos;

	public GameObject foxObj;
	public GameObject grainObj;
	public GameObject henObj;

	public GameObject checker;

	Vector3 foxPos;
	Vector3 henPos;
	Vector3 grainPos;

	public List<GameObject> inside = new List<GameObject>();


	void Start() {
		print ("Adrian har en fitte"); 
	}

	void Update() {

		/*
		foreach(GameObject i in inside) {
			Vector3 pos = Vector3.zero;
			if (gameObject.tag == "Fish") {
				if (i.tag == "Fox")
					pos = foxTarPos.transform.position;
				if (i.tag == "Hen")
					pos = henTarPos.transform.position;
				if (i.tag == "Grain")
					pos = grainTarPos.transform.position;
			} else {
				if (i.tag == "Fox")
					pos = foxStartPos.transform.position;
				if (i.tag == "Hen")
					pos = henStartPos.transform.position;
				if (i.tag == "Grain")
					pos = grainStartPos.transform.position;
			}
			i.transform.position = pos;
			i.transform.localRotation = Quaternion.identity;
		}
		*/
	}

	void OnTriggerEnter(Collider other) {
		PickupableObject po = other.GetComponent<PickupableObject>();
		if(po.pickupObject != null) {
			PickupController pc = po.pickupObject.parent.parent.GetComponent<PickupController> ();
			if (pc != null)
				pc.CmdDrop (other.gameObject);
		}

		if (gameObject.tag == "Fish") {
			if (other.name == "Fox") {
				other.tag = "InterObj";
				//other.transform.parent = transform;
				other.transform.position = foxTarPos.transform.position;
				other.transform.localRotation = Quaternion.identity;
				if(!inside.Contains(other.gameObject))
					inside.Add(other.gameObject);
				//foxPlaced = true;
			}
			if (other.name == "Hen") {
				other.tag = "InterObj";
				//other.transform.parent = transform;
				other.transform.position = henTarPos.transform.position;
				other.transform.localRotation = Quaternion.identity;
				if(!inside.Contains(other.gameObject))
					inside.Add(other.gameObject);
			}
			if (other.name == "Grain") {
				other.tag = "InterObj";
				//other.transform.parent = transform;
				other.transform.position = grainTarPos.transform.position;
				other.transform.localRotation = Quaternion.identity;
				if(!inside.Contains(other.gameObject))
					inside.Add(other.gameObject);
			}

			if (transform.childCount == 3) {
				print ("Door open");
				Destroy (checker);
                //winImage.SetActive(true);



				foxObj.GetComponent<SphereCollider> ().enabled = false;
				henObj.GetComponent<SphereCollider> ().enabled = false;
				grainObj.GetComponent<SphereCollider> ().enabled = false;
			}
		} 
		else 
		{
			if (other.name == "Fox") {
				other.tag = "InterObj";
				//other.transform.parent = transform;
				other.transform.position = foxStartPos.transform.position;
				other.transform.rotation = Quaternion.Euler (0f, 0f, 0f);
				if(!inside.Contains(other.gameObject))
					inside.Add(other.gameObject);
			}

			if (other.name == "Hen") {
				other.tag = "InterObj";
				//other.transform.parent = transform;
				other.transform.position = henStartPos.transform.position;
				other.transform.rotation = Quaternion.Euler (0f, 0f, 0f);
				if(!inside.Contains(other.gameObject))
					inside.Add(other.gameObject);
			}

			if (other.name == "Grain") {
				other.tag = "InterObj";
				//other.transform.parent = transform;
				other.transform.position = grainStartPos.transform.position;
				other.transform.rotation = Quaternion.Euler (0f, 0f, 0f);
				if(!inside.Contains(other.gameObject))
					inside.Add(other.gameObject);
			}

		}

	}

	void OnTriggerExit(Collider other) {
		if (inside.Contains (other.gameObject))
			inside.Remove (other.gameObject);
	}
}
