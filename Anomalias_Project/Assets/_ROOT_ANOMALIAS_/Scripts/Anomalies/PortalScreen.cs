using UnityEngine;

public class PortalScreen : MonoBehaviour
{
    public MeshRenderer screen;

    public Camera portalCamera;

    void Start()
    {
        RenderTexture texture =
            new RenderTexture(
                Screen.width,
                Screen.height,
                24
            );

        portalCamera.targetTexture =
            texture;

        screen.material.mainTexture =
            texture;
    }
}