using UnityEngine;

[ExecuteInEditMode]
public class SetSortingLayer : MonoBehaviour {
    public string sortingLayer = "Transparent";

    void Start() {
        GetComponent<MeshRenderer>().sortingLayerName = sortingLayer;
    }
}
