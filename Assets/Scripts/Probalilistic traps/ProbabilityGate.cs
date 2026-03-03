using UnityEngine;
using UnityEngine.Events;

public class ProbabilityGate : MonoBehaviour
{
    [Header("RNG Settings")]
    [Range(0f, 100f)]
    [Tooltip("The percentage chance that the 'Success' event will fire.")]
    [SerializeField] private float successChance = 50f;

    [Header("Outcomes")]
    public UnityEvent onSuccess; 
    public UnityEvent onFailure;
    
    public void Activate()
    {
        float roll = Random.Range(0f, 100f);

        if (roll <= successChance)
        {
            Debug.Log($"🎲 RNG Gate [{name}]: SUCCESS ({roll:F1} <= {successChance})");
            onSuccess.Invoke();
        }
        else
        {
            Debug.Log($"🎲 RNG Gate [{name}]: FAIL ({roll:F1} > {successChance})");
            onFailure.Invoke();
        }
    }

    // Call this to randomize the chance itself (e.g., make it harder dynamically)
    public void SetChance(float newChance)
    {
        successChance = Mathf.Clamp(newChance, 0f, 100f);
    }
}