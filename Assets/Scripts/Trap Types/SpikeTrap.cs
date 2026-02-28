using UnityEngine;

public class SpikeTrap : TrapBase
{
    public override void Activate()
    {
        // 1. Check RNG first
        if (!ShouldActivate()) return;

        if (changesPlayerData) ApplyMutationsToPlayer();
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        // 1. Check RNG first. If false, DO NOTHING (Safe spike)
        if (!ShouldActivate()) return;

        // 2. Run Base (Mutations)
        base.OnCollisionEnter2D(collision);

        // 3. Run Specific Logic (Kill)
        if (collision.gameObject.CompareTag("Player") && CanKillFromCollision(collision))
        {
            KillPlayer(collision.gameObject);
        }
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (!ShouldActivate()) return;
        if (changesPlayerData) ApplyMutationsToPlayer(collision.gameObject);
        
        var trapCol = GetComponentInChildren<Collider2D>();
        if (collision.CompareTag("Player") && CanKillFromTrigger(collision, trapCol))
        {
            KillPlayer(collision.gameObject);
        }
    }

    private void KillPlayer(GameObject player)
    {
        player.SetActive(false); 
        if (GameManager.Instance != null) GameManager.Instance.GameOver();
    }
}