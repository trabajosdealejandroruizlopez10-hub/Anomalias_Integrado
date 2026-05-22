using UnityEngine;

public class AnomalyManager : MonoBehaviour
{
    public GameObject[] anomalies;

    private int currentAnomaly = -1;

    void Start()
    {
        DisableAllAnomalies();
        ActivateRandomAnomaly();
    }

    void DisableAllAnomalies()
    {
        for (int i = 0; i < anomalies.Length; i++)
        {
            anomalies[i].SetActive(false);
        }
    }

    public void ActivateRandomAnomaly()
    {
        DisableAllAnomalies();

        if (anomalies.Length == 0)
            return;

        currentAnomaly = Random.Range(0, anomalies.Length);

        anomalies[currentAnomaly].SetActive(true);
    }

    public bool HasActiveAnomaly()
    {
        return currentAnomaly != -1;
    }

    public void ClearAnomaly()
    {
        DisableAllAnomalies();

        currentAnomaly = -1;
    }
}