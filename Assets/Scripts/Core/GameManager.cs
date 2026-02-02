using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Flow Settings")]
    [SerializeField] private float deathRestartDelay = 0.5f;
    [SerializeField] private float winNextLevelDelay = 2.0f;

    // --- NEW: Attempt Tracking ---
    public int Attempts { get; private set; } = 1;
    private int currentSceneIndex = -1;
    // -----------------------------

    private bool isGameActive = true;
    private LevelUIManager currentLevelUI;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // Called automatically whenever a level loads (start or restart)
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        isGameActive = true;

        if (scene.buildIndex != currentSceneIndex)
        {
            // New Level: Reset attempts
            currentSceneIndex = scene.buildIndex;
            Attempts = 1;
        }
        else
        {
            // Same Level (Restart): Increment attempts
            Attempts++;
        }

        Debug.Log($"Level Loaded. Attempt #{Attempts}");
    }

    public void RegisterLevelUI(LevelUIManager ui)
    {
        currentLevelUI = ui;
    }

    public void GameOver()
    {
        if (!isGameActive) return;
        isGameActive = false;

        Debug.Log("💀 Dead. Restarting...");

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayDeath();
        }

        Invoke(nameof(RestartLevel), deathRestartDelay);
    }

    private void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LevelComplete()
    {
        if (!isGameActive) return;
        isGameActive = false;

        Debug.Log("⭐ Level Complete!");

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayWin();
        }

        if (currentLevelUI != null)
        {
            currentLevelUI.ShowWinEffects();
        }

        Invoke(nameof(LoadNextLevel), winNextLevelDelay);
    }

    private void LoadNextLevel()
    {
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;

        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            Debug.Log("No more levels! Looping to start.");
            SceneManager.LoadScene(0);
        }
    }
}