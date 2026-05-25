using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class RoomManager : MonoBehaviour
{
    public static RoomManager Instance { get; private set; }

    [Header("Habitación de inicio (fija en escena)")]
    public GameObject startRoom;

    [Header("Prefabs de habitaciones (Room_01 a Room_09)")]
    public GameObject[] roomPrefabs;

    [Header("Jugador")]
    public Transform player;

    [Header("Fade")]
    public Image fadeImage;
    public float fadeDuration = 0.4f;

    [Header("Victoria / Derrota")]
    public GameObject winPanel;
    public GameObject failPanel;

    [Header("Ajustes")]
    public int roomsToWin = 10;

    private int consecutiveCorrect = 0;
    private GameObject currentRoom;
    private RoomData currentRoomData;
    private bool isTransitioning = false;
    private bool isInStartRoom = true;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        if (winPanel) winPanel.SetActive(false);
        if (failPanel) failPanel.SetActive(false);

        if (fadeImage)
        {
            fadeImage.color = new Color(0, 0, 0, 1);
            StartCoroutine(FadeOut());
        }

        // Al inicio el jugador está en la habitación de inicio
        currentRoom = null;
        isInStartRoom = true;
    }

    public void OnPlayerPassedPortal(bool isForward)
    {
        if (isTransitioning) return;
        StartCoroutine(HandlePortal(isForward));
    }

    private IEnumerator HandlePortal(bool isForward)
    {
        isTransitioning = true;

        SetPlayerMovement(false);

        yield return StartCoroutine(FadeIn());

        if (isInStartRoom)
        {
            // Desde la habitación de inicio solo se puede avanzar
            // Desactivar habitación de inicio
            if (startRoom) startRoom.SetActive(false);
            isInStartRoom = false;
            consecutiveCorrect = 0;
            LoadRoom(GetRandomRoomIndex());
        }
        else
        {
            bool correctChoice;
            if (currentRoomData.hasAnomaly)
                correctChoice = !isForward;
            else
                correctChoice = isForward;

            if (correctChoice)
            {
                consecutiveCorrect++;

                if (consecutiveCorrect >= roomsToWin)
                {
                    yield return StartCoroutine(FadeOut());
                    ShowWin();
                    isTransitioning = false;
                    yield break;
                }
                else
                {
                    LoadRoom(GetRandomRoomIndex());
                }
            }
            else
            {
                // Fallo — volver a habitación de inicio
                consecutiveCorrect = 0;

                if (currentRoom != null)
                    Destroy(currentRoom);

                if (startRoom) startRoom.SetActive(true);
                isInStartRoom = true;

                TeleportPlayerToSpawn(startRoom.GetComponent<RoomData>()?.spawnPoint);

                if (failPanel)
                {
                    failPanel.SetActive(true);
                    yield return new WaitForSeconds(1.5f);
                    failPanel.SetActive(false);
                }
            }
        }

        yield return StartCoroutine(FadeOut());
        SetPlayerMovement(true);
        isTransitioning = false;
    }

    private void LoadRoom(int prefabIndex)
    {
        if (currentRoom != null)
            Destroy(currentRoom);

        if (roomPrefabs == null || prefabIndex >= roomPrefabs.Length || roomPrefabs[prefabIndex] == null)
        {
            Debug.LogError($"[RoomManager] Prefab {prefabIndex} no asignado.");
            return;
        }

        currentRoom = Instantiate(roomPrefabs[prefabIndex], Vector3.zero, Quaternion.identity);
        currentRoomData = currentRoom.GetComponent<RoomData>();

        if (currentRoomData == null)
        {
            Debug.LogError($"[RoomManager] El prefab no tiene RoomData.");
            return;
        }

        currentRoomData.SetupAnomaly();
        TeleportPlayerToSpawn(currentRoomData.spawnPoint);
    }

    private int GetRandomRoomIndex()
    {
        if (roomPrefabs == null || roomPrefabs.Length == 0) return 0;
        return Random.Range(0, roomPrefabs.Length);
    }

    private void TeleportPlayerToSpawn(Transform spawnPoint)
    {
        if (spawnPoint == null || player == null) return;

        Rigidbody rb = player.GetComponent<Rigidbody>();
        if (rb)
        {
            rb.isKinematic = true;
            player.position = spawnPoint.position;
            player.rotation = spawnPoint.rotation;
            rb.isKinematic = false;
        }
        else
        {
            player.position = spawnPoint.position;
            player.rotation = spawnPoint.rotation;
        }
    }

    private void SetPlayerMovement(bool enabled)
    {
        if (player == null) return;
        PlayerController pc = player.GetComponent<PlayerController>();
        if (pc) pc.enabled = enabled;
    }

    private void ShowWin()
    {
        if (winPanel) winPanel.SetActive(true);
        SetPlayerMovement(false);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private IEnumerator FadeIn()
    {
        if (fadeImage == null) yield break;
        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            fadeImage.color = new Color(0, 0, 0, Mathf.Clamp01(t / fadeDuration));
            yield return null;
        }
        fadeImage.color = new Color(0, 0, 0, 1);
    }

    private IEnumerator FadeOut()
    {
        if (fadeImage == null) yield break;
        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            fadeImage.color = new Color(0, 0, 0, Mathf.Clamp01(1f - t / fadeDuration));
            yield return null;
        }
        fadeImage.color = new Color(0, 0, 0, 0);
    }

    public void RestartGame()
    {
        consecutiveCorrect = 0;
        if (winPanel) winPanel.SetActive(false);
        if (failPanel) failPanel.SetActive(false);

        if (currentRoom != null) Destroy(currentRoom);
        if (startRoom) startRoom.SetActive(true);
        isInStartRoom = true;

        TeleportPlayerToSpawn(startRoom.GetComponent<RoomData>()?.spawnPoint);
        SetPlayerMovement(true);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (fadeImage) fadeImage.color = new Color(0, 0, 0, 0);
    }
}