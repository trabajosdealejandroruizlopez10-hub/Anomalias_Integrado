using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;

    private void Awake()
    {
        // Evita duplicados
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        // No destruir entre escenas
        DontDestroyOnLoad(gameObject);
    }
}