using UnityEngine;

public class CorridorEndTrigger : MonoBehaviour
{
    public bool isForwardTrigger;

    private bool triggered;

    private void OnTriggerEnter(Collider other)
    {
        if (triggered)
            return;

        if (!other.CompareTag("Player"))
            return;

        triggered = true;

        AnomalyManager anomalyManager = FindObjectOfType<AnomalyManager>();
        LoopManager loopManager = FindObjectOfType<LoopManager>();

        bool hasAnomaly = anomalyManager.hasAnomaly;

        if (isForwardTrigger)
        {
            if (hasAnomaly)
            {
                loopManager.PlayerChoseWrongDoor();
            }
            else
            {
                loopManager.PlayerChoseCorrectDoor();
            }
        }
        else
        {
            if (hasAnomaly)
            {
                loopManager.PlayerChoseCorrectDoor();
            }
            else
            {
                loopManager.PlayerChoseWrongDoor();
            }
        }
    }
}