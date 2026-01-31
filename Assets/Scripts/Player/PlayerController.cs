using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private PlayerData data;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private CircleCollider2D bodyCollider;
    
    [SerializeField] private PlayerAudio playerAudio; 
    public bool IsGrounded => isGrounded; 

    private Rigidbody2D rb;
    private Vector2 moveInput;
    private bool isGrounded;
    
    // Timers
    private float coyoteTimeCounter;
    private float jumpBufferCounter;

    // Jump State
    private bool canDoubleJump; // NEW: Tracks if we have a charge left
    private bool isFacingRight = true;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        // Update Timers
        if (isGrounded)
        {
            coyoteTimeCounter = data.coyoteTime;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }

        jumpBufferCounter -= Time.deltaTime;
    }

    private void FixedUpdate()
    {
        Run();
        ApplyCustomGravity();
        CornerCorrect();
        CheckGround();
    }
    

    // --- Core Movement Logic ---

    public void SetInput(Vector2 input)
    {
        moveInput = input;
    }

    public void OnJumpPressed()
    {
        jumpBufferCounter = data.jumpBufferTime; // Always buffer input first
        
        // PRIORITY 1: Normal Jump (Grounded or Coyote Time)
        if (coyoteTimeCounter > 0f)
        {
            PerformJump(); // This checks "isGrounded" logic effectively via CoyoteTime
            
            // If we jump from ground, we are allowed to double jump next (if enabled)
            if(data.allowDoubleJump) canDoubleJump = true; 
        }
        // PRIORITY 2: Double Jump (Mid-air)
        else if (data.allowDoubleJump && canDoubleJump)
        {
            PerformJump();
            canDoubleJump = false; // Consume the charge
        }
    }

    public void OnJumpReleased()
    {
        // Variable Jump Height
        if (rb.linearVelocity.y > 0f)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * 0.5f);
        }
    }

    private void Run()
    {
        float targetSpeed = moveInput.x * data.moveSpeed;
        float accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? data.acceleration : data.decceleration;
        float velX = Mathf.MoveTowards(rb.linearVelocity.x, targetSpeed, accelRate * Time.fixedDeltaTime);

        rb.linearVelocity = new Vector2(velX, rb.linearVelocity.y);

        if (moveInput.x > 0 && !isFacingRight) Flip();
        else if (moveInput.x < 0 && isFacingRight) Flip();
    }

    private void PerformJump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0); 
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, data.JumpVelocity);
    
        jumpBufferCounter = 0f;
        coyoteTimeCounter = 0f;

        // 3. TRIGGER AUDIO HERE
        if(playerAudio != null) playerAudio.PlayJump(); 
    }

    // --- Physics Refinements ---

    private void ApplyCustomGravity()
    {
        float gravity = data.GravityStrength;

        // Fall Faster
        if (rb.linearVelocity.y < 0)
        {
            gravity *= data.fallGravityMultiplier;
        }

        rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y + (gravity * Time.fixedDeltaTime));
    }

    private void CornerCorrect()
    {
        if (rb.linearVelocity.y <= 0) return;

        Vector2 originLeft = new Vector2(transform.position.x - bodyCollider.bounds.extents.x, transform.position.y + bodyCollider.bounds.extents.y);
        Vector2 originRight = new Vector2(transform.position.x + bodyCollider.bounds.extents.x, transform.position.y + bodyCollider.bounds.extents.y);

        RaycastHit2D hitLeft = Physics2D.Raycast(originLeft, Vector2.up, data.topRaycastLength, groundLayer);
        RaycastHit2D hitRight = Physics2D.Raycast(originRight, Vector2.up, data.topRaycastLength, groundLayer);

        if (hitLeft && !hitRight)
        {
            transform.position += Vector3.right * (data.cornerCorrectionDistance * Time.fixedDeltaTime);
        }
        else if (!hitLeft && hitRight)
        {
            transform.position += Vector3.left * (data.cornerCorrectionDistance * Time.fixedDeltaTime);
        }
    }

    private void CheckGround()
    {
        bool wasGrounded = isGrounded;
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);

        // Reset Double Jump when we touch the ground
        if (isGrounded) 
        {
            canDoubleJump = true;
        }
        // Edge Case: If we walk off a ledge, we still have "CoyoteTime", 
        // but if we wait too long, we shouldn't lose our Double Jump ability.
        // The logic in OnJumpPressed handles this: 
        // if CoyoteTime expires, it falls through to the Double Jump check.
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 scaler = transform.localScale;
        scaler.x *= -1;
        transform.localScale = scaler;
    }
    
    public void StopMovement()
    {
        rb.linearVelocity = Vector2.zero;
        rb.isKinematic = true; 
        this.enabled = false;
    }
}