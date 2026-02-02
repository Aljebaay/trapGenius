using UnityEngine;
using UnityEngine.Events;

public class AttemptTrigger : MonoBehaviour
{
    public enum CheckType { Equal, Greater, Less, Modulo }

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
                isMet = (current == targetValue);
                break;
            case CheckType.Greater:
                isMet = (current > targetValue);
                break;
            case CheckType.Less:
                isMet = (current < targetValue);
                break;
            case CheckType.Modulo:
                // Useful for "Every 3rd attempt" (Value = 3)
                isMet = (current % targetValue == 0);
                break;
        }

        if (isMet)
        {
            onConditionMet.Invoke();
        }
        else
        {
            onConditionFailed.Invoke();
        }
    }
}