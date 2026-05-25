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
        Vector3 relativePosition =
            otherPortal.InverseTransformPoint(
                playerCamera.position
            );

        relativePosition =
            Quaternion.Euler(
                0f,
                180f,
                0f
            ) * relativePosition;

        transform.position =
            portal.TransformPoint(
                relativePosition
            );

        Quaternion relativeRotation =
            Quaternion.Inverse(
                otherPortal.rotation
            ) *
            playerCamera.rotation;

        Vector3 euler =
            relativeRotation.eulerAngles;

        transform.rotation =
            portal.rotation *
            Quaternion.Euler(
                euler.x,
                euler.y + 180f,
                0f
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