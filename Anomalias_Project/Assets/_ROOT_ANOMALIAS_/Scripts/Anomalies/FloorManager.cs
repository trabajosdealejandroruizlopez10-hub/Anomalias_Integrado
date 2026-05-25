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

    public void ResetFloor()
    {
        currentFloor = 0;

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