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
    
    // ------------------------
    // Kill Direction Settings
    // ------------------------
    public enum KillHitDirection
    {
        AnySide,
        OnlyTop,
        OnlyBelow,
        OnlyLeft,
        OnlyRight
    }

    // =========================
    // Kill Settings
    // =========================
    [Header("Kill Settings")]
    [Tooltip("If disabled, this trap will never kill the player.")]
    public bool isLethal = false;

    [Tooltip("Controls from which side the trap is lethal.")]
    public KillHitDirection killDirection = KillHitDirection.AnySide;

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

    
    // --- Kill Direction Helpers ---
    protected bool CanKillFromCollision(Collision2D collision)
    {
        if (!isLethal) return false;
        if (killDirection == KillHitDirection.AnySide) return true;
        if (collision == null || collision.contactCount == 0) return true; // fail-open

        // Contact normal points from the OTHER collider towards THIS collider.
        // For determining where the player hit the trap FROM, we invert it.
        Vector2 fromPlayerToTrap = -collision.GetContact(0).normal;

        // Decide dominant axis
        if (Mathf.Abs(fromPlayerToTrap.x) > Mathf.Abs(fromPlayerToTrap.y))
        {
            if (killDirection == KillHitDirection.OnlyLeft)  return fromPlayerToTrap.x < 0f;
            if (killDirection == KillHitDirection.OnlyRight) return fromPlayerToTrap.x > 0f;
            return false;
        }
        else
        {
            if (killDirection == KillHitDirection.OnlyTop)   return fromPlayerToTrap.y > 0f;
            if (killDirection == KillHitDirection.OnlyBelow) return fromPlayerToTrap.y < 0f;
            return false;
        }
    }
    protected bool CanKillFromTrigger(Collider2D playerCollider, Collider2D trapCollider)
    {
        if (!isLethal) return false;
        if (killDirection == KillHitDirection.AnySide) return true;
        if (playerCollider == null || trapCollider == null) return true; // fail-open

        Vector2 p = playerCollider.bounds.center;
        Vector2 t = trapCollider.bounds.center;
        Vector2 d = p - t;

        if (Mathf.Abs(d.x) > Mathf.Abs(d.y))
        {
            if (killDirection == KillHitDirection.OnlyLeft)  return d.x < 0f;
            if (killDirection == KillHitDirection.OnlyRight) return d.x > 0f;
            return false;
        }
        else
        {
            if (killDirection == KillHitDirection.OnlyTop)   return d.y > 0f;
            if (killDirection == KillHitDirection.OnlyBelow) return d.y < 0f;
            return false;
        }
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