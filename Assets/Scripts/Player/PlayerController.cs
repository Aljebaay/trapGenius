using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private PlayerData data;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;

    private Rigidbody2D rb;
    private Vector2 moveInput;
    private bool isGrounded;
    
    // Timers
    private float coyoteTimeCounter;
    private float jumpBufferCounter;

    // State
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
        ApplyGravityModifiers();
        CheckGround();
    }

    // --- Core Movement Logic ---

    public void SetInput(Vector2 input)
    {
        moveInput = input;
    }

    public void OnJumpPressed()
    {
        jumpBufferCounter = data.jumpBufferTime; // Queue the jump
        
        // Execute Jump if buffer and coyote time are valid
        if (jumpBufferCounter > 0f && coyoteTimeCounter > 0f)
        {
            PerformJump();
        }
    }

    public void OnJumpReleased()
    {
        // Variable Jump Height: If moving up, cut velocity to create short hop
        if (rb.linearVelocity.y > 0f)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * 0.5f);
        }
    }

    private void Run()
    {
        // Calculate target speed
        float targetSpeed = moveInput.x * data.moveSpeed;
        
        // Calculate speed difference
        float speedDif = targetSpeed - rb.linearVelocity.x;
        
        // Apply acceleration or decceleration based on input
        float accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? data.acceleration : data.decceleration;
        
        // Apply force
        float movement = speedDif * accelRate;
        
        rb.AddForce(movement * Vector2.right);

        // Visual Flip
        if (moveInput.x > 0 && !isFacingRight) Flip();
        else if (moveInput.x < 0 && isFacingRight) Flip();
    }

    private void PerformJump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0); // Reset Y velocity for consistent jump height
        rb.AddForce(Vector2.up * data.jumpForce, ForceMode2D.Impulse);
        
        jumpBufferCounter = 0f;
        coyoteTimeCounter = 0f;
    }

    // --- Physics Refinements ---

    private void ApplyGravityModifiers()
    {
        // Heavy falling (makes controls feel tighter)
        if (rb.linearVelocity.y < 0)
        {
            rb.gravityScale = data.gravityScale * data.fallGravityMultiplier;
        }
        else
        {
            rb.gravityScale = data.gravityScale;
        }
    }

    private void CheckGround()
    {
        // Simple circle check at the feet
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
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
        // 1. Kill all momentum immediately
        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;

        // 2. Turn off gravity and physics simulations so they don't slide or fall
        rb.isKinematic = true; 
        
        // 3. Disable this script so Update/FixedUpdate no longer run
        this.enabled = false;
    }
}