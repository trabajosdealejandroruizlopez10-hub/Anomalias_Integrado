using UnityEngine;

public class RoomData : MonoBehaviour
{
    [Header("Puntos de referencia")]
    public Transform spawnPoint;
    public Transform frontExit;
    public Transform backExit;

    [Header("Triggers de portal")]
    public CorridorTrigger frontTrigger;
    public CorridorTrigger backTrigger;

    [Header("Anomalías")]
    public GameObject[] anomalyObjects;

    [HideInInspector]
    public bool hasAnomaly = false;

    private void Awake()
    {
        foreach (var obj in anomalyObjects)
            if (obj != null) obj.SetActive(false);
    }

    public void SetupAnomaly()
    {
        if (anomalyObjects == null || anomalyObjects.Length == 0)
        {
            hasAnomaly = false;
            return;
        }

        hasAnomaly = Random.value > 0.5f;

        foreach (var obj in anomalyObjects)
            if (obj != null) obj.SetActive(hasAnomaly);
    }
}