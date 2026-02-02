using UnityEngine;

public class SpikeTrap : TrapBase
{
    // 1. REQUIRED: The method triggers/sequencers call
    public override void Activate()
    {
        // For a static spike, Activate might just check for mutations.
        // Or, if you animate the spikes popping up later, you trigger the animation here.
        if (changesPlayerData) ApplyMutationsToPlayer();
    }

    // 2. Logic for Solid Spikes (if BoxCollider is NOT Trigger)
    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        // Important: Call base so Mutation logic runs
        base.OnCollisionEnter2D(collision);

        if (collision.gameObject.CompareTag("Player"))
        {
            KillPlayer(collision.gameObject);
        }
    }

    // 3. Logic for Trigger Spikes (if BoxCollider IS Trigger)
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        // Important: Call base so Mutation logic runs
        base.OnTriggerEnter2D(collision);

        if (collision.CompareTag("Player"))
        {
            KillPlayer(collision.gameObject);
        }
    }

    private void KillPlayer(GameObject player)
    {
        // 1. Disable player movement/visuals instantly
        player.SetActive(false); 
        
        // 2. Tell Game Manager to reset
        if (GameManager.Instance != null)
        {
            GameManager.Instance.GameOver();
        }
    }
}