using UnityEngine;

public class PortalDecision : MonoBehaviour
{
    public bool isCorrectPortal;

    public bool roomHasAnomaly;

    private bool used;

    private void OnTriggerEnter(Collider other)
    {
        if (used)
            return;

        if (!other.CompareTag("Player"))
            return;

        used = true;

        bool success =
            (
                roomHasAnomaly &&
                !isCorrectPortal
            )
            ||
            (
                !roomHasAnomaly &&
                isCorrectPortal
            );

        if (success)
        {
            FloorManager.Instance
                .CorrectChoice();
        }
        else
        {
            FloorManager.Instance
                .WrongChoice();
        }
    }
}