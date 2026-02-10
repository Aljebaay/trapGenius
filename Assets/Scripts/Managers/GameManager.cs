using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Flow Settings")]
    [SerializeField] private float deathRestartDelay = 0.5f;
    [SerializeField] private float winNextLevelDelay = 2.0f;

    
    public int Attempts { get; private set; } = 0;
    public int Coins { get; private set; } = 0;
    
    private int currentSceneIndex = -1; 


    private bool isGameActive = true;
    private LevelUIManager currentLevelUI;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            transform.SetParent(null); 
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        // Don't set currentSceneIndex here. Let OnSceneLoaded handle logic.
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        isGameActive = true;
        Coins = 0; 
        
        if (scene.buildIndex != currentSceneIndex)
        {
            // DIFFERENT SCENE (or First Load) -> Reset Counter
            currentSceneIndex = scene.buildIndex;
            Attempts = 1;
        }
        else
        {
            Attempts++;
        }
        
        Debug.Log($"Level Loaded: {scene.name} (Index: {currentSceneIndex}) | Attempt #{Attempts}");
    }
    
    public void AddCoin(int amount)
    {
        Coins += amount;
        
        // Update UI immediately
        if (currentLevelUI != null) 
        {
            currentLevelUI.UpdateCoinText(Coins);
        }
    }
    
    public void RegisterLevelUI(LevelUIManager ui)
    {
        currentLevelUI = ui;
        currentLevelUI.UpdateAttemptText(Attempts);
        currentLevelUI.UpdateCoinText(Coins);
    }

    public void GameOver()
    {
        if (!isGameActive) return;
        isGameActive = false;

        Debug.Log("💀 Dead. Restarting...");

         // Note: The actual 'Attempts' data increments in OnSceneLoaded, 
        // so this is just a visual trick for the split second before reload.
        if (currentLevelUI != null) 
        {
            currentLevelUI.UpdateAttemptText(Attempts + 1);
        }

        if (AudioManager.Instance != null) AudioManager.Instance.PlayDeath();
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
        if (AudioManager.Instance != null) AudioManager.Instance.PlayWin();
        if (currentLevelUI != null) currentLevelUI.ShowWinEffects();
        Invoke(nameof(LoadNextLevel), winNextLevelDelay);
    }

    private void LoadNextLevel()
    {
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
            SceneManager.LoadScene(nextSceneIndex);
        else
            SceneManager.LoadScene(0);
    }
    
    
    
}