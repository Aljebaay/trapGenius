using UnityEngine;
using UnityEngine.Tilemaps;

public class SlipperyFloor : TrapBase
{
    [Header("Slippery Settings")]
    [Tooltip("Deceleration while on ice. Lower = slides longer. Default player decel is ~120.")]
    [SerializeField] private float slipperyDeceleration = 3f;

    [Tooltip("Speed the player is forced to maintain while on the floor. Cannot stand still.")]
    [SerializeField] private float minimumSpeed = 3f;

    [Tooltip("How fast the floor accelerates the player back up to minimumSpeed.")]
    [SerializeField] private float nudgeAcceleration = 30f;

    [Tooltip("Optional visual tint (default: icy blue).")]
    [SerializeField] private Color floorTint = new Color(0.6f, 0.88f, 1f, 1f);
    [SerializeField] private bool applyTint = true;

    private Tilemap tilemap;
    private Rigidbody2D playerRb;
    private Transform playerTransform;
    private PlayerController currentPlayer;

    private bool playerIsOnFloor = false;
    private float originalDeceleration;

    private void Awake()
    {
        tilemap = GetComponent<Tilemap>();
        if (applyTint && tilemap != null)
            tilemap.color = floorTint;
    }

    private void FixedUpdate()
    {
        if (!playerIsOnFloor || playerRb == null || playerTransform == null) return;

        // Read facing direction directly from the sprite scale sign.
        // This matches PlayerController.Flip() which does scaler.x *= -1.
        float facingDir = Mathf.Sign(playerTransform.localScale.x);

        float velX = playerRb.linearVelocity.x;

        // If the player has slowed below the minimum in their facing direction,
        // nudge them forward. This fires on landing, standing still, or reversing.
        float targetVel = facingDir * minimumSpeed;
        if (Mathf.Abs(velX) < minimumSpeed || Mathf.Sign(velX) != facingDir)
        {
            playerRb.linearVelocity = new Vector2(
                Mathf.MoveTowards(velX, targetVel, nudgeAcceleration * Time.fixedDeltaTime),
                playerRb.linearVelocity.y
            );
        }
    }

    public override void Activate()
    {
        if (!ShouldActivate()) return;
        if (currentPlayer != null && !playerIsOnFloor)
            ApplySlipToPlayer(currentPlayer);
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Player")) return;

        // Lethality check before slip
        if (CanKillFromCollision(collision))
        {
            KillPlayer(collision.gameObject);
            return;
        }

        if (!ShouldActivate()) return;

        PlayerController player = collision.gameObject.GetComponent<PlayerController>();
        if (player == null) return;

        currentPlayer = player;
        playerRb = collision.gameObject.GetComponent<Rigidbody2D>();
        playerTransform = collision.gameObject.transform;

        if (!playerIsOnFloor)
            ApplySlipToPlayer(player);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Player")) return;

        PlayerController player = collision.gameObject.GetComponent<PlayerController>();
        if (player == null) return;

        RemoveSlipFromPlayer(player);
        currentPlayer = null;
        playerRb = null;
        playerTransform = null;
    }

    private void ApplySlipToPlayer(PlayerController player)
    {
        playerIsOnFloor = true;
        originalDeceleration = player.GetDeceleration();
        player.SetDeceleration(slipperyDeceleration);

        if (changesPlayerData) ApplyMutationsToPlayer(player.gameObject);
    }

    private void RemoveSlipFromPlayer(PlayerController player)
    {
        if (!playerIsOnFloor) return;

        playerIsOnFloor = false;
        player.SetDeceleration(originalDeceleration);

        if (changesPlayerData) RevertMutationsFromPlayer(player.gameObject);
    }

    private void KillPlayer(GameObject player)
    {
        player.SetActive(false);
        if (GameManager.Instance != null) GameManager.Instance.GameOver();
    }
}