using UnityEngine;
using System.Collections;

public class PortalTraveller : MonoBehaviour
{
    [Header("Portal")]
    public Transform destination;

    [Header("Game Logic")]
    public bool roomHasAnomaly;

    public bool isForwardPortal;

    [Header("Wrong Portal")]
    public bool sendsToStart;

    private bool canTeleport = true;

    private void OnTriggerEnter(Collider other)
    {
        if (!canTeleport)
            return;

        if (!other.CompareTag("Player"))
            return;

        bool success =
            (
                !roomHasAnomaly &&
                isForwardPortal
            )
            ||
            (
                roomHasAnomaly &&
                !isForwardPortal
            );

        if (success)
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
            other.transform,
            !success
        );
    }

    void Teleport(
        Transform player,
        bool resetPortal
    )
    {
        StartCoroutine(
            TeleportCooldown()
        );

        Rigidbody rb =
            player.GetComponent<Rigidbody>();

        PlayerController controller =
            player.GetComponent<PlayerController>();

        if (resetPortal)
        {
            controller.Teleport(
                destination.position,
                destination.rotation
            );

            rb.linearVelocity =
                Vector3.zero;

            rb.angularVelocity =
                Vector3.zero;

            return;
        }

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