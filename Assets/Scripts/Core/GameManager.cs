using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Flow Settings")]
    [SerializeField] private float deathRestartDelay = 0.5f;
    [SerializeField] private float winNextLevelDelay = 2.0f; 

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

    public void RegisterLevelUI(LevelUIManager ui)
    {
        currentLevelUI = ui;
        isGameActive = true; 
    }

    // --- LOSS LOGIC ---
    public void GameOver()
    {
        if (!isGameActive) return;
        isGameActive = false;

        Debug.Log("💀 Dead. Restarting...");

        // 1. Play Death SFX
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

    // --- WIN LOGIC ---
    public void LevelComplete()
    {
        if (!isGameActive) return;
        isGameActive = false;

        Debug.Log("⭐ Level Complete!");

        // 1. Play Win SFX
        if (AudioManager.Instance != null) 
        {
            AudioManager.Instance.PlayWin();
        }

        // 2. Show UI
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