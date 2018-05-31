using UnityEngine;
using UnityEngine.Networking;

public class RockIndentifier : NetworkBehaviour {

	[SyncVar]
	public float weight;

	private PickupableObject po;
	private Rigidbody rb;
    private bool registered = false;

    [SyncVar]
    private Vector3 size = Vector3.zero;

	void Start() {
		po = GetComponent<PickupableObject>();
		rb = GetComponent<Rigidbody>();
		if(isServer) {
			System.Random rand = new System.Random((transform.name + "" + transform.parent.name).GetHashCode());

			weight = rand.Next(6, 16);

            size = transform.localScale * (weight / 10f);

            //random position
            /* OLD WAY, THE NEW WAY IS IN THE WeightController
			int x = rand.Next(1, 3) == 1 ? -1 : 1;
			int y = rand.Next(1, 3) == 1 ? -1 : 1;
			transform.localPosition = new Vector3((rand.Next(1000, 2001) / 100f) * x, 1.5f * (weight / 10f), (rand.Next(1000, 2001) / 100f) * y);
            */

			po.setReachDistance(weight / 2f);
			Vector3 startOffset = po.getPositionOffset();
			startOffset.z = (weight + (weight / 2f)) / 10f;
			po.setPositionOffset(startOffset);
        }
	}

    void Update() {
        if(transform.localScale != size)
            transform.localScale = size;
    }

    void LateUpdate() {
        if(isServer && !registered)
            register();
    }

    void register() {
        transform.parent.GetComponentInChildren<WeightController>().AddRock(this);
        Vector3 temp = transform.localPosition;
        temp.y = 1.5f * (weight / 10f);
        transform.localPosition = temp;
        po.checkStartPosition();
        registered = true;
    }
}
