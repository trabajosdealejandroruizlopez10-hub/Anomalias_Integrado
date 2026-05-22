using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    public bool correctDoor = true;

    private bool used;

    private void OnTriggerEnter(Collider other)
    {
        if (used)
            return;

        if (!other.CompareTag("Player"))
            return;

        used = true;

        LoopManager loopManager = FindObjectOfType<LoopManager>();

        if (correctDoor)
        {
            loopManager.PlayerChoseCorrectDoor();
        }
        else
        {
            loopManager.PlayerChoseWrongDoor();
        }
    }
}