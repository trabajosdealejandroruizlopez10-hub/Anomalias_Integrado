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
        Matrix4x4 m =
            portal.localToWorldMatrix *
            otherPortal.worldToLocalMatrix *
            playerCamera.localToWorldMatrix;

        transform.SetPositionAndRotation(
            m.GetColumn(3),
            m.rotation
        );

        transform.Rotate(
            Vector3.up,
            180f
        );

        cam.projectionMatrix =
            Camera.main.projectionMatrix;
    }
}