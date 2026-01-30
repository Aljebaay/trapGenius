using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Flow Settings")]
    [SerializeField] private float deathRestartDelay = 0.5f; // Fast reset
    [SerializeField] private float winNextLevelDelay = 1.0f; // Time for animation

    private bool isGameActive = true;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    // --- LOSS LOGIC ---
    public void GameOver()
    {
        if (!isGameActive) return;
        isGameActive = false;

        Debug.Log("💀 Dead. Restarting...");
        
        // Logic to freeze/hide player is handled by the trap, 
        // but we ensure input is dead here if needed.
        
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

        Debug.Log("⭐ Level Complete! Waiting for animation...");

        // TODO: Trigger your Win Animation here in Phase 2
        // e.g. PlayerAnimator.SetTrigger("Dance");

        Invoke(nameof(LoadNextLevel), winNextLevelDelay);
    }

    private void LoadNextLevel()
    {
        // Calculate next scene index
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;
        //int nextSceneIndex = currentSceneIndex;

        // Check if next scene exists in Build Settings
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            Debug.Log("No more levels! Looping to start.");
            SceneManager.LoadScene(0); // Loop back to first level
        }
    }
}