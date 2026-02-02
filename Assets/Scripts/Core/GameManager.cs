using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Flow Settings")]
    [SerializeField] private float deathRestartDelay = 0.5f;
    [SerializeField] private float winNextLevelDelay = 2.0f;
    //[SerializeField] private TMP_Text deathCounters;

    // Attempt Tracking 
    public int Attempts { get; private set; } = 0;
    
    // Initialize to -1 so the first load is ALWAYS detected as a "New Level"
    private int currentSceneIndex = -1; 
    // -----------------------------

    private bool isGameActive = true;
    private LevelUIManager currentLevelUI;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            // IMPORTANT: This object must be at the ROOT of the hierarchy 
            // for DontDestroyOnLoad to work. It cannot be a child of another object.
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

        // Check against the stored index
        if (scene.buildIndex != currentSceneIndex)
        {
            // DIFFERENT SCENE (or First Load) -> Reset Counter
            currentSceneIndex = scene.buildIndex;
            Attempts = 1;
        }
        else
        {
            // SAME SCENE -> Increment Counter
            Attempts++;
        }

        // if(deathCounters != null) deathCounters.text = Attempts.ToString();
        Debug.Log($"Level Loaded: {scene.name} (Index: {currentSceneIndex}) | Attempt #{Attempts}");
    }

    // ... Rest of your code (GameOver, LevelComplete, etc) remains the same ...
    
    public void RegisterLevelUI(LevelUIManager ui)
    {
        currentLevelUI = ui;
    }

    public void GameOver()
    {
        if (!isGameActive) return;
        isGameActive = false;

        Debug.Log("💀 Dead. Restarting...");
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