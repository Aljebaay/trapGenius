using System.Collections;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))] // Ensure trigger exists
public class Teleporter : TrapBase
{
    [Header("🔗 Connection")]
    [Tooltip("Target destination transform.")]
    [SerializeField] private Transform destination;
    
    [Tooltip("If true, puts the destination teleporter on cooldown to prevent instant return.")]
    [SerializeField] private bool preventInstantReturn = true;

    [Header("⚙️ Settings")]
    [Tooltip("Time it takes to fade out/move (visual delay).")]
    [SerializeField] private float teleportDelay = 0.2f;

    [Tooltip("Time before this teleporter can be used again.")]
    [SerializeField] private float cooldownDuration = 1.0f;

    [Header("🎵 Audio & Visuals")]
    [SerializeField] private ParticleSystem teleportParticles;
    [SerializeField] private AudioClip teleportSound;

    private bool isOnCooldown = false;

    // --- TRAP BASE OVERRIDES ---

    public override void Activate()
    {
        // Allow external triggers (like a button) to force a teleport 
        // We try to find the player since we don't have a collision reference here
        if (!isOnCooldown && ShouldActivate())
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null) StartCoroutine(TeleportRoutine(player));
        }
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        // Check Lethality first!
        var trapCol = GetComponent<Collider2D>();
        if (CanKillFromTrigger(collision, trapCol))
        {
            KillPlayer(collision.gameObject);
            return;
        }

        // 1. Basic Checks
        if (isOnCooldown) return;

        // 2. RNG Check (Inherited from TrapBase)
        if (!ShouldActivate()) return;

        // 3. Trigger Teleport
        StartCoroutine(TeleportRoutine(collision.gameObject));
    }

    // --- LOGIC ---

    private void KillPlayer(GameObject player)
    {
        player.SetActive(false);
        if (GameManager.Instance != null) GameManager.Instance.GameOver();
    }

    private IEnumerator TeleportRoutine(GameObject player)
    {
        isOnCooldown = true;
        
        // 1. Visuals/Audio Start
        if (teleportSound != null) AudioSource.PlayClipAtPoint(teleportSound, transform.position);
        if (teleportParticles != null) teleportParticles.Play();

        // 2. Freeze Player (Disable controls & stop physics)
        PlayerController controller = player.GetComponent<PlayerController>();
        Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
        
        if (controller != null) controller.enabled = false; // Stop Input processing
        if (rb != null) rb.linearVelocity = Vector2.zero;   // Kill momentum

        // 3. Wait for delay (Visual effect time)
        yield return new WaitForSeconds(teleportDelay);

        // 4. Move Player
        if (destination != null)
        {
            player.transform.position = destination.position;

            // 5. Apply Remote Cooldown (Prevent ping-pong)
            if (preventInstantReturn)
            {
                Teleporter destTeleporter = destination.GetComponent<Teleporter>();
                if (destTeleporter != null)
                {
                    destTeleporter.StartCooldown(cooldownDuration);
                }
            }
            
            // Play sound at destination too
            if (teleportSound != null) AudioSource.PlayClipAtPoint(teleportSound, destination.position);
        }

        // 6. Apply Mutations (Scale, Gravity, etc. from TrapBase)
        if (changesPlayerData) ApplyMutationsToPlayer(player);

        // 7. Unfreeze Player
        if (controller != null) controller.enabled = true;
        
        // 8. Local Cooldown
        yield return new WaitForSeconds(cooldownDuration);
        isOnCooldown = false;
    }

    // Public method to allow the destination to disable this teleporter temporarily
    public void StartCooldown(float duration)
    {
        StartCoroutine(CooldownRoutine(duration));
    }

    private IEnumerator CooldownRoutine(float duration)
    {
        isOnCooldown = true;
        yield return new WaitForSeconds(duration);
        isOnCooldown = false;
    }

    // --- EDITOR GIZMOS ---
    private void OnDrawGizmos()
    {
        // Draw connection line
        if (destination != null)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(transform.position, destination.position);
            Gizmos.DrawWireSphere(destination.position, 0.5f);
        }
        
        // Draw Trigger Box
        Gizmos.color = new Color(0, 1, 1, 0.3f);
        Gizmos.DrawCube(transform.position, transform.localScale);
    }
}