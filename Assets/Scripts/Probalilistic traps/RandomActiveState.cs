using UnityEngine;

public class RandomActiveState : MonoBehaviour
{
    [Header("Settings")]
    [Range(0f, 100f)]
    [Tooltip("Chance that this object is active/visible.")]
    [SerializeField] private float existChance = 50f;

    [Tooltip("If true, it rolls the dice immediately when the level loads.")]
    [SerializeField] private bool randomizeOnStart = true;

    private void Start()
    {
        if (randomizeOnStart)
        {
            RollDice();
        }
    }

    // Can be called manually by a Trigger/Button to "Reroll" reality
    public void RollDice()
    {
        float roll = Random.Range(0f, 100f);
        bool shouldExist = (roll <= existChance);

        gameObject.SetActive(shouldExist);
        
        Debug.Log($"❓ Random State [{name}]: {(shouldExist ? "EXISTS" : "GONE")}");
    }
}