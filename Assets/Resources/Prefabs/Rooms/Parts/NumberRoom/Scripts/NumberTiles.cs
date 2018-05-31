using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;

public class NumberTiles : NetworkBehaviour {

	[SyncVar]
	[Range(0, 27)]
	public int number = 0;

	public Mesh[] models;

	private MeshFilter model;
	private int currentMeshIndex = -1;

	void Start() {
		model = transform.GetChild(0).GetComponent<MeshFilter>();
	}

	void Update() {
		if(currentMeshIndex != number) {
			currentMeshIndex = number;
			model.mesh = ResetMeshPosition(models[currentMeshIndex], number);
			BoxCollider hasBox = model.gameObject.GetComponent<BoxCollider>();
			if(hasBox != null)
				DestroyImmediate(hasBox);
			model.gameObject.AddComponent<BoxCollider>();
		}
	}

	private Mesh ResetMeshPosition(Mesh original, int num) {
		Vector3 correct = new Vector3(0.4f, 3.6f, 1.6f);
		Vector3 offset = correct - original.vertices[0];

		Mesh newMesh = original;
		newMesh.name = num + " mesh";
		List<Vector3> verts = new List<Vector3>();
		Vector3 average = Vector3.zero;
		for(int i = 0; i < original.vertices.Length; i++) {
			Vector3 vert = original.vertices[i] + offset;
			average += vert / original.vertices.Length;
			verts.Add(vert);
		}
		for(int i = 0; i < verts.Count; i++) {
			verts[i] -= average;
		}
		newMesh.SetVertices(verts);
		newMesh.RecalculateBounds();
		newMesh.RecalculateNormals();
		return newMesh;
	}
}
