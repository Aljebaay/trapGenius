using UnityEngine;
using System.Collections.Generic;

public abstract class TrapBase : MonoBehaviour
{
    [Header("Mutation Settings")]
    public bool changesPlayerData = false;
    public List<PlayerMutation> mutations = new List<PlayerMutation>();

    [Header("RNG Settings")]
    [Range(0, 100)] 
    [Tooltip("Chance this trap works (0 = Broken, 100 = Always Works)")]
    public float activationChance = 100f;

    [HideInInspector] public PlayerData referenceData;

    protected virtual void OnValidate()
    {
        if (referenceData == null) referenceData = Resources.Load<PlayerData>("PlayerData");
    }

    // --- SHARED RNG CHECKER ---
    public bool ShouldActivate()
    {
        // 1. If 100%, always true (Optimization)
        if (activationChance >= 100f) return true;
        
        // 2. If 0%, always false
        if (activationChance <= 0f) return false;

        // 3. Roll the dice
        float roll = Random.Range(0f, 100f);
        bool success = (roll <= activationChance);

        if (!success) Debug.Log($"🎲 Trap '{name}' Failed RNG Roll ({roll:F1} > {activationChance})");
        
        return success;
    }

    // --- MUTATION HELPERS ---
    protected void ApplyMutationsToPlayer(GameObject playerObj = null)
    {
        if (!changesPlayerData || mutations.Count == 0) return;
        
        // ... (Same find player logic as before) ...
        PlayerController player = (playerObj != null) ? playerObj.GetComponent<PlayerController>() : FindAnyObjectByType<PlayerController>();
        if (player != null) player.ApplyMutations(mutations);
    }

    protected void RevertMutationsFromPlayer(GameObject playerObj = null)
    {
        if (!changesPlayerData || mutations.Count == 0) return;

        PlayerController player = (playerObj != null) ? playerObj.GetComponent<PlayerController>() : FindAnyObjectByType<PlayerController>();
        if (player != null) player.RevertMutations(mutations);
    }

    // --- ABSTRACT METHODS ---
    public abstract void Activate();

    // Default implementations that can be overridden
    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        // Only run base logic if RNG passes
        if (ShouldActivate() && collision.gameObject.CompareTag("Player"))
        {
            if (changesPlayerData) ApplyMutationsToPlayer(collision.gameObject);
        }
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (ShouldActivate() && collision.CompareTag("Player"))
        {
            if (changesPlayerData) ApplyMutationsToPlayer(collision.gameObject);
        }
    }
}