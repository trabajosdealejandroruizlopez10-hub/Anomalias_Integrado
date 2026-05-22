using UnityEngine;

public class LoopManager : MonoBehaviour
{
    public Transform player;

    public GameObject[] corridorPrefabs;

    public Transform spawnPoint;

    public int currentRoom = 0;

    private GameObject currentCorridor;

    void Start()
    {
        GenerateNextCorridor();
    }

    public void PlayerChoseCorrectDoor()
    {
        currentRoom++;

        GenerateNextCorridor();
    }

    public void PlayerChoseWrongDoor()
    {
        currentRoom = 0;

        GenerateNextCorridor();
    }

    void GenerateNextCorridor()
    {
        if (currentCorridor != null)
        {
            Destroy(currentCorridor);
        }

        int randomIndex = Random.Range(0, corridorPrefabs.Length);

        currentCorridor = Instantiate(
            corridorPrefabs[randomIndex],
            spawnPoint.position,
            spawnPoint.rotation
        );

        player.position = spawnPoint.position;
    }
}