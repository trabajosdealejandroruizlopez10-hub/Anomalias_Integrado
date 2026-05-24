using UnityEngine;
using System.Collections;

public class PortalTraveller : MonoBehaviour
{
    public Transform destination;

    private bool canTeleport = true;

    private void OnTriggerEnter(Collider other)
    {
        if (!canTeleport)
            return;

        if (!other.CompareTag("Player"))
            return;

        Rigidbody rb =
            other.GetComponent<Rigidbody>();

        if (rb == null)
            return;

        StartCoroutine(
            TeleportCooldown()
        );

        Transform player =
            other.transform;

        Vector3 localPosition =
            transform.InverseTransformPoint(
                player.position
            );

        localPosition =
            Quaternion.Euler(
                0f,
                180f,
                0f
            ) * localPosition;

        player.position =
            destination.TransformPoint(
                localPosition
            );

        Quaternion difference =
            destination.rotation *
            Quaternion.Inverse(
                transform.rotation
            );

        player.rotation =
            difference *
            player.rotation;

        Vector3 velocity =
            transform.InverseTransformDirection(
                rb.linearVelocity
            );

        velocity =
            Quaternion.Euler(
                0f,
                180f,
                0f
            ) * velocity;

        rb.linearVelocity =
            destination.TransformDirection(
                velocity
            );

        player.position +=
            destination.forward * 1f;
    }

    IEnumerator TeleportCooldown()
    {
        canTeleport = false;

        yield return new WaitForSeconds(0.2f);

        canTeleport = true;
    }
}