using UnityEngine;

public class CameraLid : MonoBehaviour {
    [Range(450, 9000)]
    public int imageSize = 900;

    public FilterMode fm = FilterMode.Point;

    [HideInInspector]
	[SerializeField]
	private Camera _camera;
	//private int _downResFactor = 1;
    private int lastSize = 900;
    private FilterMode lastFm = FilterMode.Point;

    private string _globalTextureName = "_GlobalLidTexture";

	void Start() {
		_camera = GetComponent<Camera>();
        Transform pappa = transform.parent;
		//_camera.pixelRect = new Rect(Vector2.zero, new Vector2(1, 1));
        pappa.GetComponent<MeshRenderer>().sortingLayerName = "Black Lid";
        GenerateRT();
    }

    void Update() {
        if(imageSize != lastSize) {
            GenerateRT();
            lastSize = imageSize;
        }

        if(fm != lastFm) {
            GenerateRT();
            lastFm = fm;
        }
    }

	void GenerateRT() {
		if(_camera.targetTexture != null) {
			RenderTexture temp = _camera.targetTexture;
			_camera.targetTexture = null;
			DestroyImmediate(temp);
		}
        //_camera.targetTexture = new RenderTexture(_camera.pixelWidth >> _downResFactor, _camera.pixelHeight >> _downResFactor, 16);
        _camera.targetTexture = new RenderTexture(imageSize, imageSize, 16);
		_camera.targetTexture.filterMode = fm;

		Shader.SetGlobalTexture(_globalTextureName, _camera.targetTexture);
	}

}
