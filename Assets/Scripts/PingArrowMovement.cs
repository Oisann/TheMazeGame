using UnityEngine;

public class PingArrowMovement : MonoBehaviour {

    public Vector3 highestPoint = Vector3.zero;
    public Vector3 lowestPoint = Vector3.zero;
    public float speed = 5.0f;

    private bool goingUp = true;

    void Start() {
        InvokeRepeating("toggle", .5f, .5f);
    }
	
	void Update() {
        float totalDistance = Vector3.Distance(lowestPoint, highestPoint);
        Vector3 target = goingUp ? highestPoint : lowestPoint;
        transform.localPosition = Vector3.Lerp(transform.localPosition, target, speed * Time.deltaTime);
        /*if(Vector3.Distance(transform.localPosition, target) <= totalDistance / 100f) {
            goingUp = !goingUp;
        }*/
	}

    private void toggle() {
        goingUp = !goingUp;
    }
}
