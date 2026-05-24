using System.Collections;
using UnityEditor.EditorTools;
using UnityEngine;

public class PortalTraveller : MonoBehaviour
{
    public Transform destination;
    public bool isForward;

    private Transform player;
    private bool playerOverlapping;
    private bool canTeleport = true;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        player = other.transform;
        playerOverlapping = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        playerOverlapping = false;
    }

    void Update()
    {
        if (!playerOverlapping) return;
        if (!canTeleport) return;

        Vector3 portalToPlayer = player.position - transform.position;
        float dot = Vector3.Dot(transform.forward, portalToPlayer);

        if (dot < 0f)
        {
            if (RoomManager.Instance != null)
                RoomManager.Instance.OnPlayerPassedPortal(isForward);
            else
                Teleport();
        }
    }

    public void Teleport()
    {
        StartCoroutine(TeleportCooldown());

        Rigidbody rb = player.GetComponent<Rigidbody>();

        Vector3 relativePosition = transform.InverseTransformPoint(player.position);
        relativePosition = Quaternion.Euler(0f, 180f, 0f) * relativePosition;
        player.position = destination.TransformPoint(relativePosition);

        Quaternion relativeRotation = Quaternion.Inverse(transform.rotation) * player.rotation;
        relativeRotation = Quaternion.Euler(0f, 180f, 0f) * relativeRotation;
        player.rotation = destination.rotation * relativeRotation * Quaternion.Euler(0f, 180f, 0f);

        Vector3 relativeVelocity = transform.InverseTransformDirection(rb.linearVelocity);
        relativeVelocity = Quaternion.Euler(0f, 180f, 0f) * relativeVelocity;
        rb.linearVelocity = destination.TransformDirection(relativeVelocity);

        player.position += destination.forward * 0.5f;
        playerOverlapping = false;
    }

    IEnumerator TeleportCooldown()
    {
        canTeleport = false;
        yield return new WaitForSeconds(0.2f);
        canTeleport = true;
    }
}