using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class FloorManager : MonoBehaviour
{
    public static FloorManager Instance;

    [Header("Floor")]
    public int currentFloor = 0;

    public int winFloor = 10;

    [Header("Scenes")]
    public string victorySceneName;

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

        if (currentFloor >= winFloor)
        {
            WinGame();
        }
    }

    public void WrongChoice()
    {
        currentFloor = 0;

        Rigidbody rb =
            player.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.linearVelocity =
                Vector3.zero;

            rb.angularVelocity =
                Vector3.zero;
        }

        PlayerController controller =
            player.GetComponent<PlayerController>();

        if (controller != null)
        {
            controller.Teleport(
                spawnPoint.position,
                spawnPoint.rotation
            );
        }
        else
        {
            player.position =
                spawnPoint.position;

            player.rotation =
                spawnPoint.rotation;
        }

        UpdateUI();
    }

    void WinGame()
    {
        SceneManager.LoadScene(
            victorySceneName
        );
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