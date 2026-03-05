using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class CrushingWall : TrapBase
{
    [Header("Movement Settings")]
    [Tooltip("Where the wall moves to (Relative to start position).")]
    [SerializeField] private Vector3 moveOffset = new Vector3(5, 0, 0);
    
    [SerializeField] private float speed = 2f;
    
    [Tooltip("If true, wall moves back and forth. If false, it stops at destination.")]
    [SerializeField] private bool pingPong = true;
    
    [Tooltip("Delay at each end of the movement.")]
    [SerializeField] private float waitTime = 1f;

    [Tooltip("If true, starts moving immediately. If false, waits for Activate().")]
    [SerializeField] private bool autoStart = true;

    [Header("Crush Logic")]
    [Tooltip("What layers cause the player to get crushed? (e.g., Ground, Walls, other Traps)")]
    [SerializeField] private LayerMask crushLayer;
    
    [Tooltip("How far to check behind the player for a wall. Slightly larger than player width.")]
    [SerializeField] private float squishCheckDistance = 0.6f;

    private Rigidbody2D rb;
    private Vector3 startPos;
    private Vector3 targetPos;
    private Vector3 currentTarget;
    private float waitTimer;
    private bool isWaiting = false;
    private bool isMoving = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        // Ensure Rigidbody is Kinematic so it pushes the player unstoppable
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.useFullKinematicContacts = true; 

        startPos = transform.position;
        targetPos = startPos + moveOffset;
        currentTarget = targetPos;
    }

    private void Start()
    {
        if (autoStart) Activate();
    }

    // --- REQUIRED OVERRIDE ---
    public override void Activate()
    {
        // 1. RNG Check
        if (!ShouldActivate()) return;

        // 2. Enable Movement
        isMoving = true;

        // 3. Apply Global Mutations (if configured in Inspector)
        if (changesPlayerData) ApplyMutationsToPlayer();
    }

    private void FixedUpdate()
    {
        if (!isMoving) return;

        HandleMovement();
    }

    private void HandleMovement()
    {
        if (isWaiting)
        {
            waitTimer -= Time.fixedDeltaTime;
            if (waitTimer <= 0)
            {
                isWaiting = false;
                // Swap targets if ping-ponging
                if (pingPong)
                {
                    currentTarget = (currentTarget == targetPos) ? startPos : targetPos;
                }
            }
            return;
        }

        // Move the Rigidbody
        Vector3 newPos = Vector3.MoveTowards(rb.position, currentTarget, speed * Time.fixedDeltaTime);
        rb.MovePosition(newPos);

        // Check if reached destination
        if (Vector3.Distance(rb.position, currentTarget) < 0.01f)
        {
            if (pingPong || currentTarget != targetPos) 
            {
                isWaiting = true;
                waitTimer = waitTime;
            }
            else
            {
                // If not ping pong and reached end, stop moving
                isMoving = false;
            }
        }
    }

    // This runs every frame the wall is touching the player
    protected void OnCollisionStay2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Player")) return;
        
        // Check if Player is Squished
        if (CheckSquish(collision))
        {
            KillPlayer(collision.gameObject);
        }
    }

    // NEW: Support for instant touch lethality without squish.
    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);

        if (collision.gameObject.CompareTag("Player") && CanKillFromCollision(collision))
        {
            KillPlayer(collision.gameObject);
        }
    }

    private void KillPlayer(GameObject player)
    {
        player.SetActive(false);
        if (GameManager.Instance != null) GameManager.Instance.GameOver();
    }

    private bool CheckSquish(Collision2D collision)
    {
        // Calculate the direction the wall is pushing
        Vector3 pushDirection = (currentTarget - startPos).normalized;

        // If we are moving back to start, flip direction
        if (Vector3.Distance(currentTarget, startPos) < 0.1f) 
            pushDirection = (startPos - targetPos).normalized;

        // If wall is currently stationary (waiting), it can't crush
        if (isWaiting) return false;

        Vector2 playerCenter = collision.transform.position;
        
        // Debug draw to see the ray in Scene view
        Debug.DrawRay(playerCenter, pushDirection * squishCheckDistance, Color.red);

        // Raycast against the Crush Layer (Ground/Walls)
        // We do NOT include the wall itself in this mask
        RaycastHit2D hit = Physics2D.Raycast(playerCenter, pushDirection, squishCheckDistance, crushLayer);

        if (hit.collider != null)
        {
            Debug.Log($"💀 SQUISHED! Pinned between {name} and {hit.collider.name}");
            return true;
        }

        return false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Vector3 start = Application.isPlaying ? startPos : transform.position;
        Vector3 end = Application.isPlaying ? targetPos : transform.position + moveOffset;
        
        Gizmos.DrawWireCube(start, transform.localScale);
        Gizmos.DrawLine(start, end);
        Gizmos.DrawWireCube(end, transform.localScale);
    }
}