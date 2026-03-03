using System.Collections;
using UnityEngine;
using UnityEngine.UI; 
using TMPro; 

public class LevelUIManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject winScreenObject; 
    [SerializeField] private CanvasGroup winScreenCanvasGroup; 
    [SerializeField] private float fadeDuration = 1.0f;

    [Header("HUD")] 
    [SerializeField] private TMP_Text attemptText;
    [SerializeField] private TMP_Text coinText;
    [SerializeField] private TMP_Text keyText;

    private void Start()
    {
        InventoryManager.onCoinChanged += UpdateCoinText;
        if(InventoryManager.Instance != null) 
            UpdateCoinText(InventoryManager.Instance.GetCoinCount());
        
        InventoryManager.onKeysChanged += UpdateKeyText;
        if (InventoryManager.Instance != null)
            UpdateKeyText(InventoryManager.Instance.GetKeyCount());

        
        if(winScreenObject != null) 
            winScreenObject.SetActive(false);

        if(winScreenCanvasGroup != null) 
            winScreenCanvasGroup.alpha = 0f;

      
        if (GameManager.Instance != null)
        {
            GameManager.Instance.RegisterLevelUI(this);
        }
    }
    
    public void UpdateAttemptText(int count)
    {
        if (attemptText != null)
        {
            attemptText.text = count.ToString();
        }
    }
    
    public void UpdateKeyText(int count)
    {
        if (keyText != null)
        {
            // You might want to format this (e.g., "Keys: 1")
            // For now, it just shows the number like the others
            keyText.text = count.ToString();
        }
    }
        
    public void UpdateCoinText(int count)
    {
        if (coinText != null)
        {
            coinText.text = count.ToString();
        }
    }
    
    private void OnDestroy() 
    {
        // Unsubscribe from BOTH to prevent errors
        InventoryManager.onCoinChanged -= UpdateCoinText;
        InventoryManager.onKeysChanged -= UpdateKeyText;
    }
    
    
    
    //-------------- EFFECTS

    public void ShowWinEffects()
    {
        if (winScreenObject != null)
        {
            winScreenObject.SetActive(true);
            if (winScreenCanvasGroup != null) StartCoroutine(FadeInRoutine());
        }
    }
    
    

    private IEnumerator FadeInRoutine()
    {
        float timer = 0f;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            winScreenCanvasGroup.alpha = Mathf.Lerp(0f, 1f, timer / fadeDuration);
            yield return null;
        }
        winScreenCanvasGroup.alpha = 1f;
    }

}