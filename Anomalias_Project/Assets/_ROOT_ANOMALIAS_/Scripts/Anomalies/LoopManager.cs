using UnityEngine;

public class LoopManager : MonoBehaviour
{
    public GameObject corridorPrefab;

    public Transform player;

    private GameObject currentCorridor;

    private Transform currentEndPoint;

    private AnomalyManager anomalyManager;

    public int roomNumber;

    void Start()
    {
        anomalyManager =
            FindObjectOfType<AnomalyManager>();

        GenerateFirstCorridor();
    }

    void GenerateFirstCorridor()
    {
        currentCorridor =
            Instantiate(
                corridorPrefab,
                Vector3.zero,
                Quaternion.identity
            );

        CorridorReferences refs =
            currentCorridor.GetComponent<CorridorReferences>();

        currentEndPoint = refs.endPoint;

        anomalyManager.GenerateAnomaly();

        SpawnPlayer(refs.spawnPoint);
    }

    public void NextLoop(bool wentForward)
    {
        bool hasAnomaly =
            anomalyManager.hasAnomaly;

        bool correctChoice;

        if (hasAnomaly)
        {
            correctChoice = !wentForward;
        }
        else
        {
            correctChoice = wentForward;
        }

        if (correctChoice)
        {
            roomNumber++;
        }
        else
        {
            roomNumber = 0;
        }

        GenerateNextCorridor();
    }

    void GenerateNextCorridor()
    {
        GameObject newCorridor =
            Instantiate(corridorPrefab);

        CorridorReferences newRefs =
            newCorridor.GetComponent<CorridorReferences>();

        Vector3 offset =
            newRefs.startPoint.position -
            newCorridor.transform.position;

        newCorridor.transform.position =
            currentEndPoint.position - offset;

        Quaternion rotationDifference =
            Quaternion.FromToRotation(
                newRefs.startPoint.forward,
                currentEndPoint.forward
            );

        newCorridor.transform.rotation =
            rotationDifference *
            newCorridor.transform.rotation;

        newRefs =
            newCorridor.GetComponent<CorridorReferences>();

        currentEndPoint =
            newRefs.endPoint;

        Destroy(currentCorridor);

        currentCorridor = newCorridor;

        anomalyManager.GenerateAnomaly();

        SpawnPlayer(newRefs.spawnPoint);
    }

    void SpawnPlayer(Transform spawnPoint)
    {
        CharacterController controller =
            player.GetComponent<CharacterController>();

        controller.enabled = false;

        player.position =
            spawnPoint.position;

        player.rotation =
            spawnPoint.rotation;

        controller.enabled = true;
    }
}