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

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        UpdateUI();
    }

    // LLAMAR CUANDO EL PLAYER ACIERTA
    public void CorrectChoice()
    {
        currentFloor++;

        UpdateUI();

        Debug.Log("Correct! Floor: " + currentFloor);
    }

    // LLAMAR CUANDO EL PLAYER FALLA
    public void WrongChoice()
    {
        // Reinicia el contador
        currentFloor = 0;

        // Teletransporta al spawn
        player.position = spawnPoint.position;
        player.rotation = spawnPoint.rotation;

        // Resetea físicas
        Rigidbody rb = player.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        UpdateUI();

        Debug.Log("Wrong! Reset to Floor 0");
    }

    // ACTUALIZA EL TEXTO
    void UpdateUI()
    {
        if (floorText != null)
        {
            floorText.text = "Floor " + currentFloor;
        }
    }
}