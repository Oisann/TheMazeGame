using UnityEngine;

public class CapeController : MonoBehaviour {

    private Vector3 lastPos;
    //private Vector3 lastEuler;

    private float randomX = 0f;
    private System.Random rand;

    void Start() {
        rand = new System.Random(transform.parent.parent.parent.name.GetHashCode());
        lastPos = transform.position;
        //lastEuler = transform.parent.parent.eulerAngles;
        InvokeRepeating("newRandomX", 0f, .2f);
    }
	
	void FixedUpdate() {
        float dist = Vector3.Distance(transform.position, lastPos);
        dist = Mathf.Clamp(dist, 0f, 3f);
        float speed = dist / Time.fixedDeltaTime;

        float random = 10f + randomX;

        Vector3 newRot = new Vector3(speed * 5f + random, 0f, 0f);
        transform.localEulerAngles = Vector3.Lerp(transform.localEulerAngles, newRot, .02f);

        lastPos = transform.position;
        //lastEuler = transform.parent.parent.eulerAngles;
    }

    private void newRandomX() {
        randomX = rand.Next(-30, 30) / 10f;
    }
}
