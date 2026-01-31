using UnityEngine;
using System.Collections;
using UnityEngine.UI; // Required for UI manipulation

public class LevelUIManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject winScreenObject; // The object you want to set active
    [SerializeField] private CanvasGroup winScreenCanvasGroup; // Used for fading (Optional but recommended)
    [SerializeField] private float fadeDuration = 1.0f;

    private void Start()
    {
        // 1. Ensure the Win Screen is hidden at start
        if(winScreenObject != null) 
            winScreenObject.SetActive(false);

        // 2. Set Alpha to 0 if using CanvasGroup
        if(winScreenCanvasGroup != null) 
            winScreenCanvasGroup.alpha = 0f;

        // 3. Register SELF to the GameManager
        // This solves your "lost reference" issue.
        if (GameManager.Instance != null)
        {
            GameManager.Instance.RegisterLevelUI(this);
        }
    }

    public void ShowWinEffects()
    {
        if (winScreenObject != null)
        {
            winScreenObject.SetActive(true);
            
            // Start the fade coroutine if we have a canvas group
            if (winScreenCanvasGroup != null)
            {
                StartCoroutine(FadeInRoutine());
            }
        }
    }

    private IEnumerator FadeInRoutine()
    {
        float timer = 0f;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            // Interpolate alpha from 0 to 1
            winScreenCanvasGroup.alpha = Mathf.Lerp(0f, 1f, timer / fadeDuration);
            yield return null;
        }
        winScreenCanvasGroup.alpha = 1f;
    }
}