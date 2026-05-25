using UnityEngine;

public class PortalCamera : MonoBehaviour
{
    public Transform playerCamera;

    public Transform portal;

    public Transform otherPortal;

    private Camera cam;

    private Camera mainCam;

    void Start()
    {
        cam = GetComponent<Camera>();

        mainCam = Camera.main;
    }

    void LateUpdate()
    {
        Matrix4x4 m =
            portal.localToWorldMatrix *
            Matrix4x4.Rotate(
                Quaternion.Euler(
                    0f,
                    180f,
                    0f
                )
            ) *
            otherPortal.worldToLocalMatrix *
            playerCamera.localToWorldMatrix;

        transform.SetPositionAndRotation(
            m.GetColumn(3),
            m.rotation
        );

        cam.projectionMatrix =
            mainCam.projectionMatrix;

        cam.fieldOfView =
            mainCam.fieldOfView;

        cam.aspect =
            mainCam.aspect;

        cam.nearClipPlane =
            mainCam.nearClipPlane;

        cam.farClipPlane =
            mainCam.farClipPlane;
    }
}