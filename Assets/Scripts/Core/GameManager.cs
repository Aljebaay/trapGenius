using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Flow Settings")]
    [SerializeField] private float deathRestartDelay = 0.5f;
    [SerializeField] private float winNextLevelDelay = 2.0f; // Increased slightly for fade time

    private bool isGameActive = true;
    
    // --- NEW: Hold reference to the current scene's UI ---
    private LevelUIManager currentLevelUI;

    private void Awake()
    {
        // Singleton Setup
        if (Instance == null) 
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Make sure GameManager survives scene load
        }
        else 
        {
            Destroy(gameObject);
        }
    }

    // --- NEW: Called by the UI when the scene starts ---
    public void RegisterLevelUI(LevelUIManager ui)
    {
        currentLevelUI = ui;
        // Reset game state for the new level
        isGameActive = true; 
    }

    // --- LOSS LOGIC ---
    public void GameOver()
    {
        if (!isGameActive) return;
        isGameActive = false;

        Debug.Log("💀 Dead. Restarting...");
        Invoke(nameof(RestartLevel), deathRestartDelay);
    }

    private void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // --- WIN LOGIC ---
    public void LevelComplete()
    {
        if (!isGameActive) return;
        isGameActive = false;

        Debug.Log("⭐ Level Complete!");

        // --- NEW: Trigger the UI Fade ---
        if (currentLevelUI != null)
        {
            currentLevelUI.ShowWinEffects();
        }

        Invoke(nameof(LoadNextLevel), winNextLevelDelay);
    }

    private void LoadNextLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;

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