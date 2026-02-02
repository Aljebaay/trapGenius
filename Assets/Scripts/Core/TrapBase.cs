using UnityEngine;
using System.Collections.Generic;

public abstract class TrapBase : MonoBehaviour
{
    [Header("Mutation Settings")]
    [Tooltip("Does this trap change player stats when activated?")]
    public bool changesPlayerData = false;

    // This list holds the changes we want to apply
    public List<PlayerMutation> mutations = new List<PlayerMutation>();

    // Hidden reference, auto-loaded
    [HideInInspector] public PlayerData referenceData;

    protected virtual void OnValidate()
    {
        // AUTO-LOAD LOGIC:
        // Finds "PlayerData" inside any "Resources" folder.
        if (referenceData == null)
        {
            referenceData = Resources.Load<PlayerData>("PlayerData");
        }
    }

    // --- SHARED LOGIC ---

    // All traps can call this to apply stats
    protected void ApplyMutationsToPlayer(GameObject playerObj = null)
    {
        if (!changesPlayerData || mutations.Count == 0) return;

        // If specific object passed, try that first
        PlayerController player = null;
        if (playerObj != null) player = playerObj.GetComponent<PlayerController>();
        
        // Fallback: Find globally
        if (player == null) player = FindAnyObjectByType<PlayerController>();

        if (player != null)
        {
            player.ApplyMutations(mutations);
        }
    }

    // --- STANDARD ACTIVATION ---
    
    // Abstract: Every trap MUST implement this
    // This allows AreaTrigger to call trap.Activate() on ANYTHING.
    public abstract void Activate();

    // Standard Collision Logic (Optional override)
    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (changesPlayerData) ApplyMutationsToPlayer(collision.gameObject);
        }
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (changesPlayerData) ApplyMutationsToPlayer(collision.gameObject);
        }
    }
}