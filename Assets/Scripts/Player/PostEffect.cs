using UnityEngine;
using UnityEngine.PostProcessing;
using System.Collections;

public class PostEffect : MonoBehaviour {
    private Camera AttachedCamera;
    public Shader Post_Outline;
    public Shader DrawSimple;
	public Color outlineColor = Color.white;
    private Camera TempCam;
    private Material Post_Mat;

	public Renderer currentObj;
    private Color defaultOutlineColor;
    // public RenderTexture TempRT;

    public bool showOutline = true;
    public bool isMac = false;

    void Start() {
        //if(Application.platform == RuntimePlatform.OSXPlayer || Application.platform == RuntimePlatform.OSXEditor || Application.platform == RuntimePlatform.OSXDashboardPlayer) {
            //Destroy(this);
            //return;
		isMac = SystemInfo.graphicsDeviceVersion.StartsWith("Open");

		PostProcessingBehaviour ppb = GetComponent<PostProcessingBehaviour>();
		if(ppb != null)
			ppb.enabled = !isMac;

        AttachedCamera = GetComponent<Camera>();
        TempCam = new GameObject("Outline").AddComponent<Camera>();
        TempCam.transform.parent = transform;
        TempCam.enabled = false;
        Post_Mat = new Material(Post_Outline);
        defaultOutlineColor = outlineColor;
    }

	public void SetHover(Renderer obj) {
        outlineColor = defaultOutlineColor;

        if(currentObj != null)
			currentObj.gameObject.layer = LayerMask.NameToLayer("Interactable");
		currentObj = obj;

        try {
            CustomGlowColor gc = currentObj.GetComponent<CustomGlowColor>();
            outlineColor = gc.glowColor;
        } catch(System.Exception e) {
            DisposableVariables.Exceptions(e);
        }
        
        if(currentObj != null)
			currentObj.gameObject.layer = LayerMask.NameToLayer("InteractableHover");
	}

    void OnRenderImage(RenderTexture source, RenderTexture destination) {
        //set up a temporary camera
        TempCam.CopyFrom(AttachedCamera);
        TempCam.clearFlags = CameraClearFlags.Color;
        TempCam.backgroundColor = Color.black;

        //cull any layer that isn't the outline
		TempCam.cullingMask = 1 << LayerMask.NameToLayer("InteractableHover");

        //make the temporary rendertexture
		RenderTexture TempRT = new RenderTexture(source.width, source.height, 0, RenderTextureFormat.Default);

        //put it to video memory
        TempRT.Create();

        //set the camera's target texture when rendering
        TempCam.targetTexture = TempRT;

        //render all objects this camera can render, but with our custom shader.
        TempCam.RenderWithShader(DrawSimple, "");
        
        Post_Mat.SetTexture("_SceneTex", source);
		Post_Mat.SetColor("_OutlineColor", outlineColor);
		Post_Mat.SetFloat("_FlipImage", isMac ? 1f : 0f);

        //copy the temporary RT to the final image
        Graphics.Blit(TempRT, destination, Post_Mat);

        //release the temporary RT
        TempRT.Release();
    }
}