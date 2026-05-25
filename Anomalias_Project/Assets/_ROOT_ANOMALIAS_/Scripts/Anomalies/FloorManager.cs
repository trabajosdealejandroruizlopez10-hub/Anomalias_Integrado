using UnityEngine;
using TMPro;

public class FloorManager : MonoBehaviour
{
    public static FloorManager Instance;

    [Header("Floor")]
    public int currentFloor = 0;

    [Header("Player")]
    public Transform player;

    public Transform spawnPoint;

    [Header("UI")]
    public TextMeshProUGUI floorText;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        UpdateUI();
    }

    public void CorrectChoice()
    {
        currentFloor++;

        UpdateUI();
    }

    public void WrongChoice()
    {
        currentFloor = 0;

        player.position =
            spawnPoint.position;

        player.rotation =
            spawnPoint.rotation;

        Rigidbody rb =
            player.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.linearVelocity =
                Vector3.zero;

            rb.angularVelocity =
                Vector3.zero;
        }

        UpdateUI();
    }

    void UpdateUI()
    {
        if (floorText != null)
        {
            floorText.text =
                "Floor " +
                currentFloor;
        }
    }
}