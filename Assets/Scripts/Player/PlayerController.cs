using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private PlayerData data;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private CapsuleCollider2D bodyCollider;
    [SerializeField] private PlayerAudio playerAudio; 
    
    public bool IsGrounded => isGrounded; 

    private Rigidbody2D rb;
    private Vector2 moveInput;
    private bool isGrounded;
    
    // Timers
    private float coyoteTimeCounter;
    private float jumpBufferCounter;
    private bool canDoubleJump;
    private bool isFacingRight = true;

    // --- RUNTIME STATS ---
    private float currentMoveSpeed;
    private float currentJumpHeight;
    private float currentGravityMult;
    private bool currentInvertControls;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        ResetStats(); 
    }

    public void ResetStats()
    {
        currentMoveSpeed = data.moveSpeed;
        currentJumpHeight = data.jumpHeight;
        currentGravityMult = data.fallGravityMultiplier;
        currentInvertControls = data.invertControls;
    }

    // --- NEW: Apply List of Mutations ---
    public void ApplyMutations(List<PlayerMutation> mutations)
    {
        foreach (var m in mutations)
        {
            switch (m.statToChange)
            {
                case PlayerMutation.StatType.MoveSpeed:
                    currentMoveSpeed = m.numberValue;
                    break;
                case PlayerMutation.StatType.JumpHeight:
                    currentJumpHeight = m.numberValue;
                    break;
                case PlayerMutation.StatType.GravityMultiplier:
                    currentGravityMult = m.numberValue;
                    break;
                case PlayerMutation.StatType.InvertControls:
                    currentInvertControls = m.booleanValue;
                    break;
            }
        }
        // Visual debug to confirm it happened
        Debug.Log($"Applied {mutations.Count} mutations to player.");
    }
    // ------------------------------------

    private void Update()
    {
        if (isGrounded) coyoteTimeCounter = data.coyoteTime;
        else if (data.useCoyoteTime) coyoteTimeCounter -= Time.deltaTime;
        else coyoteTimeCounter = 0f;

        jumpBufferCounter -= Time.deltaTime;
    }

    private void FixedUpdate()
    {
        Run();
        ApplyCustomGravity();
        CornerCorrect();
        CheckGround();
    }

    public void SetInput(Vector2 input)
    {
        moveInput = input;
    }

    public void OnJumpPressed()
    {
        jumpBufferCounter = data.jumpBufferTime; 
        
        if (coyoteTimeCounter > 0f)
        {
            PerformJump(); 
            if(data.allowDoubleJump) canDoubleJump = true; 
        }
        else if (data.allowDoubleJump && canDoubleJump)
        {
            PerformJump();
            canDoubleJump = false; 
        }
    }

    public void OnJumpReleased()
    {
        if (rb.linearVelocity.y > 0f)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * 0.5f);
        }
    }

    private void Run()
    {
        // --- NEW: Invert Logic ---
        float directionMult = currentInvertControls ? -1f : 1f;
        float finalInputX = moveInput.x * directionMult;
        // -------------------------

        float targetSpeed = finalInputX * currentMoveSpeed;
        float accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? data.acceleration : data.decceleration;
        float velX = Mathf.MoveTowards(rb.linearVelocity.x, targetSpeed, accelRate * Time.fixedDeltaTime);

        rb.linearVelocity = new Vector2(velX, rb.linearVelocity.y);

        // Flip Logic (Also needs to respect inversion visually?)
        // Usually, if I press Right (and it's inverted), I move Left. The character should look Left.
        // finalInputX handles this automatically.
        if (finalInputX > 0 && !isFacingRight) Flip();
        else if (finalInputX < 0 && isFacingRight) Flip();
    }

    private void PerformJump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0); 
        float gravity = -(2 * currentJumpHeight) / (data.timeToJumpApex * data.timeToJumpApex);
        float jumpVel = Mathf.Abs(gravity) * data.timeToJumpApex;
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpVel);
    
        jumpBufferCounter = 0f;
        coyoteTimeCounter = 0f;

        if(playerAudio != null) playerAudio.PlayJump(); 
    }

    private void ApplyCustomGravity()
    {
        float gravity = -(2 * currentJumpHeight) / (data.timeToJumpApex * data.timeToJumpApex);
        if (rb.linearVelocity.y < 0) gravity *= currentGravityMult;
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y + (gravity * Time.fixedDeltaTime));
    }

    private void CornerCorrect()
    {
        if (rb.linearVelocity.y <= 0) return;
        Vector2 originLeft = new Vector2(transform.position.x - bodyCollider.bounds.extents.x, transform.position.y + bodyCollider.bounds.extents.y);
        Vector2 originRight = new Vector2(transform.position.x + bodyCollider.bounds.extents.x, transform.position.y + bodyCollider.bounds.extents.y);
        RaycastHit2D hitLeft = Physics2D.Raycast(originLeft, Vector2.up, data.topRaycastLength, groundLayer);
        RaycastHit2D hitRight = Physics2D.Raycast(originRight, Vector2.up, data.topRaycastLength, groundLayer);

        if (hitLeft && !hitRight) transform.position += Vector3.right * (data.cornerCorrectionDistance * Time.fixedDeltaTime);
        else if (!hitLeft && hitRight) transform.position += Vector3.left * (data.cornerCorrectionDistance * Time.fixedDeltaTime);
    }

    private void CheckGround()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
        if (isGrounded) canDoubleJump = true;
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