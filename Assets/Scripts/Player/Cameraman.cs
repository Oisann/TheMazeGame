using UnityEngine;

[ExecuteInEditMode]
public class Cameraman : MonoBehaviour {

    [Range(0f, 3f)]
    public float RotationSpeed = 1.5f;
    [Range(0f, 3f)]
    public float ZoomSensitivity = .5f;
    [Range(0.00f, 1000.00f)]
    public float renderDistance = 150.00f;
    //[Range(0f, 360f)]
    //public float fieldOfView = 60f;

    public GameObject Player;
    [Range(0.1f, 10f)]
    public float followSpeed = 5f;

    [HideInInspector]
    public Transform RotationsTransform;
    [HideInInspector]
    public Transform UpDownTransform;
    [HideInInspector]
    public Transform DistanceTransform;

    public Camera cam;

    private NetworkVariables nvs;

    void Start() {
        if(DistanceTransform == null)
            DistanceTransform = transform.GetChild(0).GetChild(0).GetChild(0);
        if(UpDownTransform == null)
            UpDownTransform = transform.GetChild(0).GetChild(0);
        if(RotationsTransform == null)
            RotationsTransform = transform.GetChild(0);
        if(Player == null)
            Player = transform.parent.Find("Player").gameObject;
        if(cam == null)
            cam = DistanceTransform.GetChild(0).GetComponent<Camera>();
        nvs = GameObject.FindObjectOfType<NetworkVariables>();
    }

    void FixedUpdate() {
        if(IngameChat.showChat || nvs.finishTime != -1)
            return;

        if(renderDistance != cam.farClipPlane)
            cam.farClipPlane = renderDistance;

        //Rotate Up and Down
        if(Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
			UpDownTransform.localEulerAngles += new Vector3(1f, 0f, 0f) * RotationSpeed;
        if(Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
			UpDownTransform.localEulerAngles -= new Vector3(1f, 0f, 0f) * RotationSpeed;

        //Zoom
        if(Input.mouseScrollDelta != Vector2.zero)
            DistanceTransform.localPosition += new Vector3(0f, 0f, Input.mouseScrollDelta.y * ZoomSensitivity);

        if(Input.GetKey(KeyCode.LeftShift))
            return;

        //Rotate Left and Right
        if(Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
            RotationsTransform.eulerAngles += new Vector3(0f, 1f, 0f) * RotationSpeed;
        if(Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
            RotationsTransform.eulerAngles -= new Vector3(0f, 1f, 0f) * RotationSpeed;

        //Scrollwheel rotate
        if(Input.GetMouseButton(2)) {
            RotationsTransform.eulerAngles += new Vector3(0f, Input.GetAxis("Mouse X") * RotationSpeed, 0f) * 5f;
            UpDownTransform.eulerAngles -= new Vector3(Input.GetAxis("Mouse Y") * RotationSpeed, 0f, 0f) * 5f;
        }
    }

    void Update() {
        /*
        if(UpDownTransform.eulerAngles.x > 70f)
            UpDownTransform.eulerAngles -= new Vector3(UpDownTransform.eulerAngles.x - 70f, 0f, 0f);
        if(UpDownTransform.eulerAngles.x < 20f)
            UpDownTransform.eulerAngles -= new Vector3(UpDownTransform.eulerAngles.x - 20f, 0f, 0f);
        */

		UpDownTransform.localEulerAngles = new Vector3(Mathf.Clamp(UpDownTransform.localEulerAngles.x, 20f, 70f), 0f, 0f);
		DistanceTransform.localPosition = new Vector3(0f, 0f, Mathf.Clamp(DistanceTransform.localPosition.z, -25f, -10f));

		/*
        if(DistanceTransform.localPosition.z > -10f)
            DistanceTransform.localPosition -= new Vector3(0f, 0f, DistanceTransform.localPosition.z + 10f);
        if(DistanceTransform.localPosition.z < -25f)
            DistanceTransform.localPosition -= new Vector3(0f, 0f, DistanceTransform.localPosition.z + 25f);
		*/

        if(Player != null) {
            float dist = Vector3.Distance(Player.transform.position, transform.position);

            if(dist >= .1f) {
                float step = followSpeed * Time.deltaTime;
                transform.position = Vector3.Lerp(transform.position, Player.transform.position, step);
            }
        }
    }

    void OnDrawGizmos() {
        Gizmos.color = Color.white;
        if(Player != null) {
            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(cam.transform.position, transform.position);
            Gizmos.color = Color.white;
            if(Vector3.Distance(Player.transform.position, transform.position) >= .1f) {
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(Player.transform.position, .5f);
                Gizmos.color = Color.white;
                Gizmos.DrawLine(Player.transform.position, transform.position);
                Gizmos.color = Color.green;
            }
        }
        Gizmos.DrawWireSphere(transform.position, .5f);
    }

    public void ResetRotation() {
        RotationsTransform.eulerAngles = Vector3.zero;
    }
}