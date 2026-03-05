using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DisappearingPlatform : TrapBase
{
    [Header("Disappear Settings")]
    [Tooltip("Time in seconds before the platform disappears. Set to 0 for instant.")]
    [SerializeField] private float delay = 1.5f;

    [Tooltip("If true, the platform shakes during the delay to warn the player.")]
    [SerializeField] private bool shakeBeforeVanish = true;

    [SerializeField] private float shakeIntensity = 0.04f;

    [Tooltip("If true, the platform reappears after a set time.")]
    [SerializeField] private bool respawn = false;

    [SerializeField] private float respawnDelay = 3f;

    private Tilemap tilemap;
    private TilemapRenderer tilemapRenderer;
    private Collider2D col;

    private Vector3 initialPosition;
    private bool isTriggered = false;

    private void Awake()
    {
        tilemap = GetComponent<Tilemap>();
        tilemapRenderer = GetComponent<TilemapRenderer>();
        col = GetComponent<Collider2D>();
        initialPosition = transform.position;
    }

    public override void Activate()
    {
        if (isTriggered) return;

        // RNG CHECK
        if (!ShouldActivate()) return;

        isTriggered = true;
        if (changesPlayerData) ApplyMutationsToPlayer();

        StartCoroutine(DisappearRoutine());
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Activate();

            if (CanKillFromCollision(collision))
            {
                KillPlayer(collision.gameObject);
            }
        }
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Activate();

            var trapCol = GetComponent<Collider2D>();
            if (CanKillFromTrigger(collision, trapCol))
            {
                KillPlayer(collision.gameObject);
            }
        }
    }

    private void KillPlayer(GameObject player)
    {
        player.SetActive(false);
        if (GameManager.Instance != null) GameManager.Instance.GameOver();
    }

    private IEnumerator DisappearRoutine()
    {
        // --- DELAY PHASE (with optional shake) ---
        if (delay > 0f)
        {
            float timer = 0f;
            while (timer < delay)
            {
                if (shakeBeforeVanish)
                {
                    float x = Random.Range(-1f, 1f) * shakeIntensity;
                    float y = Random.Range(-1f, 1f) * shakeIntensity;
                    transform.position = initialPosition + new Vector3(x, y, 0);
                }
                timer += Time.deltaTime;
                yield return null;
            }
            transform.position = initialPosition;
        }

        // --- VANISH ---
        SetVisible(false);

        // --- RESPAWN (optional) ---
        if (respawn)
        {
            yield return new WaitForSeconds(respawnDelay);
            SetVisible(true);
            isTriggered = false; // Allow re-triggering
        }
    }

    private void SetVisible(bool visible)
    {
        if (col != null) col.enabled = visible;
        if (tilemapRenderer != null) tilemapRenderer.enabled = visible;

        // Also handle plain SpriteRenderer for non-tilemap setups
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null) sr.enabled = visible;
    }
}