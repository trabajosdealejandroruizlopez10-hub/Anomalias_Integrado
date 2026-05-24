using UnityEngine;

public class PortalCamera : MonoBehaviour
{
    public Transform playerCamera;

    public Transform portal;

    public Transform otherPortal;

    private Camera cam;

    void Start()
    {
        cam = GetComponent<Camera>();
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

        relativeRotation =
            Quaternion.Euler(
                0f,
                180f,
                0f
            ) * relativeRotation;

        transform.rotation =
            portal.rotation *
            relativeRotation;

        cam.fieldOfView =
            Camera.main.fieldOfView;
    }
}