using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Settings")]
    [SerializeField] private float restartDelay = 0.3f; // Instant retry feel

    private bool isGameOver = false;

    private void Awake()
    {
        // Simple Singleton pattern
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void GameOver()
    {
        if (isGameOver) return; // Prevent double death
        isGameOver = true;

        Debug.Log("Player Died!");
        
        // TODO: Add Particle Effects / Sound here later
        
        // Restart level logic
        Invoke(nameof(RestartLevel), restartDelay);
    }

    private void RestartLevel()
    {
        // Reloads the current active scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}