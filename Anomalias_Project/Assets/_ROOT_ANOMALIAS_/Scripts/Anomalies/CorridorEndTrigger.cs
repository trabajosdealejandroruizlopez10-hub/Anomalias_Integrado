using UnityEngine;

public class CorridorTrigger : MonoBehaviour
{
    public bool isForward;

    private bool used;

    private void OnTriggerEnter(Collider other)
    {
        if (used)
            return;

        if (!other.CompareTag("Player"))
            return;

        used = true;

        LoopManager loopManager =
            FindObjectOfType<LoopManager>();

        loopManager.NextLoop(isForward);
    }
}