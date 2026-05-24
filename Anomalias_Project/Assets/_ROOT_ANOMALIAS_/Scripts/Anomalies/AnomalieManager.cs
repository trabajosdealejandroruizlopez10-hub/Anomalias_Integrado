using UnityEngine;

public class AnomalyManager : MonoBehaviour
{
    public GameObject[] anomalies;

    public bool hasAnomaly;

    public void GenerateAnomaly()
    {
        DisableAll();

        hasAnomaly = Random.value > 0.5f;

        if (!hasAnomaly)
            return;

        int randomIndex =
            Random.Range(0, anomalies.Length);

        anomalies[randomIndex].SetActive(true);
    }

    void DisableAll()
    {
        for (int i = 0; i < anomalies.Length; i++)
        {
            anomalies[i].SetActive(false);
        }
    }
}