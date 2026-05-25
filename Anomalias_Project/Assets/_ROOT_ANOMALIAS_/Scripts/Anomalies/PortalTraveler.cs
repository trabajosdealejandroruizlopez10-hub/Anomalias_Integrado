using UnityEngine;
using System.Collections;

public class PortalTraveller : MonoBehaviour
{
    [Header("Portal")]
    public Transform destination;

    [Header("Points")]
    public bool isCorrectPortal;

    private bool canTeleport = true;

    private bool used;

    private void OnTriggerEnter(Collider other)
    {
        if (used)
            return;

        if (!canTeleport)
            return;

        if (!other.CompareTag("Player"))
            return;

        used = true;

        if (isCorrectPortal)
        {
            FloorManager.Instance
                .CorrectChoice();
        }
        else
        {
            FloorManager.Instance
                .ResetFloor();
        }

        Teleport(
            other.transform
        );
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        used = false;
    }

    void Teleport(Transform player)
    {
        StartCoroutine(
            TeleportCooldown()
        );

        Rigidbody rb =
            player.GetComponent<Rigidbody>();

        Vector3 relativePosition =
            transform.InverseTransformPoint(
                player.position
            );

        relativePosition =
            Quaternion.Euler(
                0f,
                180f,
                0f
            ) * relativePosition;

        Quaternion relativeRotation =
            Quaternion.Inverse(
                transform.rotation
            ) *
            player.rotation;

        relativeRotation =
            Quaternion.Euler(
                0f,
                180f,
                0f
            ) * relativeRotation;

        Vector3 newPosition =
            destination.TransformPoint(
                relativePosition
            );

        Quaternion newRotation =
            destination.rotation *
            relativeRotation *
            Quaternion.Euler(
                0f,
                180f,
                0f
            );

        PlayerController controller =
            player.GetComponent<PlayerController>();

        controller.Teleport(
            newPosition,
            newRotation
        );

        Vector3 relativeVelocity =
            transform.InverseTransformDirection(
                rb.linearVelocity
            );

        relativeVelocity =
            Quaternion.Euler(
                0f,
                180f,
                0f
            ) * relativeVelocity;

        rb.linearVelocity =
            destination.TransformDirection(
                relativeVelocity
            );

        player.position +=
            destination.forward * 0.5f;
    }

    IEnumerator TeleportCooldown()
    {
        canTeleport = false;

        yield return new WaitForSeconds(0.2f);

        canTeleport = true;
    }
}