using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(NetworkInstanceID))]
public class NetworkInteriorSpawner : NetworkBehaviour {
	public GameObject[] spawnPrefabs;
	public string name = "";
    public bool addIndexToName = false;

	private NetworkInstanceID nID;
    private int index;

	void Start() {
		transform.name = name + " @ " + transform.parent.name;

		nID = GetComponent<NetworkInstanceID>();

		NetworkInstanceID parentID = nID;
		parentID.objectName = transform.name;
		parentID.parentNetId = transform.parent.GetComponent<NetworkIdentity>().netId;

		if(!NetworkServer.active)
			return;

		if (spawnPrefabs.Length > 0) {
			foreach (GameObject spawn in spawnPrefabs) {
				GameObject s = Instantiate(spawn) as GameObject;
				if(s.transform.name.EndsWith("(Clone)")) {
					int i = s.transform.name.IndexOf("(Clone)");
					s.transform.name = s.transform.name.Remove(i);
				}

                if(addIndexToName)
                    s.transform.name += " " + index;

				Vector3 local = s.transform.position;
				s.transform.SetParent(transform);
				s.transform.localPosition = new Vector3(0f, .1f, 0f);
				s.transform.localPosition += local;

				s.transform.rotation = transform.rotation;

				NetworkInstanceID ID = s.transform.GetComponent<NetworkInstanceID>();
				ID.objectName = s.transform.name;
				ID.parentNetId = nID.netId;

                index++;
				NetworkServer.Spawn(s);
			}
		}
	}
}
