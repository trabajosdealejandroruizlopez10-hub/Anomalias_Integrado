using UnityEngine;

public class Portal : MonoBehaviour
{
    public Transform exitPortal;

    public Transform exitPoint;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        Rigidbody rb =
            other.GetComponent<Rigidbody>();

        if (rb == null)
            return;

        PlayerController player =
            other.GetComponent<PlayerController>();

        if (player == null)
            return;

        player.Teleport(
            exitPoint.position,
            exitPoint.rotation
        );
    }
}