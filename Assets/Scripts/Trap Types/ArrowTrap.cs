using System.Collections;
using UnityEngine;

public class ArrowTrap : TrapBase
{
    [Header("🏹 Projectile Settings")]
    [SerializeField] private ArrowProjectile arrowPrefab;
    [SerializeField] private Transform spawnPoint; 
    [SerializeField] private float arrowSpeed = 10f;
    [SerializeField] private bool killPlayer = true;

    [Header("🔄 Burst & Cycle Settings")]
    [SerializeField] private bool autoStart = false;
    [SerializeField] private bool loop = false;
    [SerializeField] private int arrowsPerBurst = 1;
    [SerializeField] private float fireRate = 0.2f;
    [SerializeField] private float cycleInterval = 2.0f;

    private Coroutine fireCoroutine;

    private void Start()
    {
        if (autoStart) Activate();
    }

    public override void Activate()
    {
        // RNG Check
        if (!ShouldActivate()) return;

        if (fireCoroutine != null) StopCoroutine(fireCoroutine);
        fireCoroutine = StartCoroutine(ShootCycle());
    }

    private IEnumerator ShootCycle()
    {
        do
        {
            for (int i = 0; i < arrowsPerBurst; i++)
            {
                FireOneArrow();
                if (i < arrowsPerBurst - 1) yield return new WaitForSeconds(fireRate);
            }

            if (loop) yield return new WaitForSeconds(cycleInterval);

        } while (loop);
    }

    private void FireOneArrow()
    {
        if (arrowPrefab == null) return;

        Vector3 spawnPos = (spawnPoint != null) ? spawnPoint.position : transform.position;
        
        // 1. Instantiate (Rotation will be fixed by Initialize immediately after)
        ArrowProjectile arrow = Instantiate(arrowPrefab, spawnPos, Quaternion.identity);

        // 2. Get Direction: The Red Axis (Right) of the trap
        Vector3 shootDir = transform.right; 
        
        // 3. Launch
        arrow.Initialize(shootDir, arrowSpeed, killPlayer, changesPlayerData ? mutations : null);
    }

    // NEW: Allow the trap block itself to be lethal to touch
    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);

        if (collision.gameObject.CompareTag("Player") && CanKillFromCollision(collision))
        {
            KillPlayer(collision.gameObject);
        }
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);

        var trapCol = GetComponent<Collider2D>();
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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector3 start = (spawnPoint != null) ? spawnPoint.position : transform.position;
        // Visualize the shoot direction
        Gizmos.DrawRay(start, transform.right * 3f);
    }
}