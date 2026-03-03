using UnityEngine;
using UnityEngine.Events;

public class AttemptTrigger : MonoBehaviour
{
    // Added "GreaterOrEqual" to the list
    public enum CheckType { Equal, Greater, Less, GreaterOrEqual, Modulo }

    [Header("Condition Logic")]
    [Tooltip("How to compare the current attempt count.")]
    [SerializeField] private CheckType condition = CheckType.Equal;
    
    [Tooltip("The number to compare against.")]
    [SerializeField] private int targetValue = 2;

    [Header("Outcomes")]
    [Tooltip("Fires if the condition is TRUE.")]
    public UnityEvent onConditionMet;

    [Tooltip("Fires if the condition is FALSE.")]
    public UnityEvent onConditionFailed;

    public void Check()
    {
        if (GameManager.Instance == null) return;

        int current = GameManager.Instance.Attempts;
        bool isMet = false;

        switch (condition)
        {
            case CheckType.Equal:
                // Triggers ONLY on attempt X (e.g., A surprise that happens once)
                isMet = (current == targetValue);
                break;

            case CheckType.Greater:
                // Triggers on X+1, X+2... (Skips the specific target number)
                isMet = (current > targetValue);
                break;

            case CheckType.Less:
                // Triggers on 1, 2... up to X-1 (Good for help that disappears later)
                isMet = (current < targetValue);
                break;
            
            case CheckType.GreaterOrEqual:
                // BEST FOR Devilbait: Triggers on X, X+1, X+2... (Permanent difficulty increase)
                isMet = (current >= targetValue);
                break;

            case CheckType.Modulo:
                // Triggers every X turns (e.g., 3, 6, 9...)
                if (targetValue == 0)
                {
                    Debug.LogError($"AttemptTrigger '{name}': targetValue is 0 for Modulo check. Defaulting to FAILED.");
                    isMet = false;
                }
                else
                {
                    isMet = (current % targetValue == 0);
                }
                break;
        }

        if (isMet)
        {
            Debug.Log($"Attempt Trigger: {condition} {targetValue} (Current: {current}) -> MET");
            onConditionMet.Invoke();
        }
        else
        {
            Debug.Log($"Attempt Trigger: {condition} {targetValue} (Current: {current}) -> FAILED");
            onConditionFailed.Invoke();
        }
    }
}