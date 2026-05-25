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
            SceneManager.LoadScene(
                victorySceneName
            );
        }
    }

    public void ResetFloor()
    {
        currentFloor = 0;

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