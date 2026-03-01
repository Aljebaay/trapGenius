using System.Collections;
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
    [SerializeField] private PlayerAnimation playerAnim;
    
    public bool IsGrounded => isGrounded;
    public float VelocityX => rb.linearVelocity.x;

    private Rigidbody2D rb;
    private Vector2 moveInput;
    private bool isGrounded;
    
    // Timers
    private float coyoteTimeCounter;
    private float jumpBufferCounter;
    private bool canDoubleJump; // Internal logic state
    private bool isFacingRight = true;

    // --- RUNTIME STATS ---
    private float currentMoveSpeed;
    private float currentDeceleration;
    private float currentJumpHeight;
    private float currentGravityMult;
    private float currentCoyoteTime; 
    private bool currentInvertControls;
    private bool currentAllowDoubleJump;
    
    // Scale Logic
    private Vector3 originalScale;
    private Coroutine activeScaleCoroutine;
    private bool isScaling = false; 

    // --- LOCKING SYSTEM ---
    private HashSet<PlayerMutation.StatType> lockedStats = new HashSet<PlayerMutation.StatType>();

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        originalScale = new Vector3(Mathf.Abs(transform.localScale.x), Mathf.Abs(transform.localScale.y), transform.localScale.z);
        ResetStats(); 
    }

    public void ResetStats()
    {
        currentMoveSpeed = data.moveSpeed;
        currentDeceleration = data.decceleration;
        currentJumpHeight = data.jumpHeight;
        currentGravityMult = data.fallGravityMultiplier;
        currentCoyoteTime = data.coyoteTime;
        currentInvertControls = data.invertControls;
        currentAllowDoubleJump = data.allowDoubleJump; // <--- NEW (Load Default)
        
        lockedStats.Clear();

        if(activeScaleCoroutine != null) StopCoroutine(activeScaleCoroutine);
        isScaling = false;

        float direction = isFacingRight ? 1f : -1f;
        transform.localScale = new Vector3(originalScale.x * direction, originalScale.y, originalScale.z);
    }

    // --- MUTATION SYSTEM ---
    public void ApplyMutations(List<PlayerMutation> mutations)
    {
        foreach (var m in mutations)
        {
            if (lockedStats.Contains(m.statToChange)) continue;

            lockedStats.Add(m.statToChange);
            StartCoroutine(ProcessMutation(m));
        }
    }

    private IEnumerator ProcessMutation(PlayerMutation m)
    {
        float oldFloat = 0;
        bool oldBool = false;
        float currentRatio = Mathf.Abs(transform.localScale.x) / originalScale.x;

        switch (m.statToChange)
        {
            case PlayerMutation.StatType.MoveSpeed:
                oldFloat = currentMoveSpeed;
                currentMoveSpeed = m.numberValue;
                break;
            case PlayerMutation.StatType.JumpHeight:
                oldFloat = currentJumpHeight;
                currentJumpHeight = m.numberValue;
                break;
            case PlayerMutation.StatType.GravityMultiplier:
                oldFloat = currentGravityMult;
                currentGravityMult = m.numberValue;
                break;
            case PlayerMutation.StatType.CoyoteTime:
                oldFloat = currentCoyoteTime;
                currentCoyoteTime = m.numberValue;
                break;
            case PlayerMutation.StatType.InvertControls:
                oldBool = currentInvertControls;
                currentInvertControls = m.booleanValue;
                break;
            case PlayerMutation.StatType.DoubleJump: // <--- NEW
                oldBool = currentAllowDoubleJump;
                currentAllowDoubleJump = m.booleanValue;
                break;
            case PlayerMutation.StatType.PlayerScale:
                if (activeScaleCoroutine != null) StopCoroutine(activeScaleCoroutine);
                activeScaleCoroutine = StartCoroutine(SmoothScaleRoutine(m.numberValue));
                break;
        }

        if (m.isTemporaryChange)
        {
            yield return new WaitForSeconds(m.duration);

            RevertMutation(m, oldFloat, oldBool, currentRatio);
            
            if (lockedStats.Contains(m.statToChange))
            {
                lockedStats.Remove(m.statToChange);
            }
        }
    }

    private void RevertMutation(PlayerMutation m, float oldFloat, bool oldBool, float oldRatio)
    {
        switch (m.statToChange)
        {
            case PlayerMutation.StatType.MoveSpeed: currentMoveSpeed = oldFloat; break;
            case PlayerMutation.StatType.JumpHeight: currentJumpHeight = oldFloat; break;
            case PlayerMutation.StatType.GravityMultiplier: currentGravityMult = oldFloat; break;
            case PlayerMutation.StatType.CoyoteTime: currentCoyoteTime = oldFloat; break;
            case PlayerMutation.StatType.InvertControls: currentInvertControls = oldBool; break;
            case PlayerMutation.StatType.DoubleJump: currentAllowDoubleJump = oldBool; break; // <--- NEW
            case PlayerMutation.StatType.PlayerScale:
                if (activeScaleCoroutine != null) StopCoroutine(activeScaleCoroutine);
                activeScaleCoroutine = StartCoroutine(SmoothScaleRoutine(oldRatio));
                break;
        }
    }

    // --- MANUAL REVERT (For Switches) ---
    public void RevertMutations(List<PlayerMutation> mutations)
    {
        foreach (var m in mutations)
        {
            if (lockedStats.Contains(m.statToChange)) lockedStats.Remove(m.statToChange);

            switch (m.statToChange)
            {
                case PlayerMutation.StatType.MoveSpeed: currentMoveSpeed = data.moveSpeed; break;
                case PlayerMutation.StatType.JumpHeight: currentJumpHeight = data.jumpHeight; break;
                case PlayerMutation.StatType.GravityMultiplier: currentGravityMult = data.fallGravityMultiplier; break;
                case PlayerMutation.StatType.CoyoteTime: currentCoyoteTime = data.coyoteTime; break;
                case PlayerMutation.StatType.InvertControls: currentInvertControls = data.invertControls; break;
                case PlayerMutation.StatType.DoubleJump: currentAllowDoubleJump = data.allowDoubleJump; break; // <--- NEW
                case PlayerMutation.StatType.PlayerScale:
                    if (activeScaleCoroutine != null) StopCoroutine(activeScaleCoroutine);
                    activeScaleCoroutine = StartCoroutine(SmoothScaleRoutine(1f));
                    break;
            }
        }
    }

    private IEnumerator SmoothScaleRoutine(float targetMultiplier)
    {
        isScaling = true;
        rb.linearVelocity = new Vector2(0, rb.linearVelocity.y); 

        Vector3 startScale = transform.localScale;
        float direction = isFacingRight ? 1f : -1f;
        Vector3 targetScale = new Vector3(originalScale.x * targetMultiplier * direction, originalScale.y * targetMultiplier, originalScale.z);

        float elapsedTime = 0f;
        float duration = 0.5f; 

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            transform.localScale = Vector3.Lerp(startScale, targetScale, t);
            yield return null;
        }

        transform.localScale = targetScale;
        isScaling = false;
        activeScaleCoroutine = null;
    }

    private void Update()
    {
        if (isGrounded) coyoteTimeCounter = currentCoyoteTime; 
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
        if (isScaling) return;

        jumpBufferCounter = data.jumpBufferTime; 
        
        if (coyoteTimeCounter > 0f)
        {
            PerformJump(); 
            // USE RUNTIME VARIABLE
            if(currentAllowDoubleJump) canDoubleJump = true; 
        }
        else if (currentAllowDoubleJump && canDoubleJump) // USE RUNTIME VARIABLE
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
        if (isScaling)
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
            return; 
        }

        float directionMult = currentInvertControls ? -1f : 1f;
        float finalInputX = moveInput.x * directionMult;

        float targetSpeed = finalInputX * currentMoveSpeed;
        float accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? data.acceleration : currentDeceleration;
        float velX = Mathf.MoveTowards(rb.linearVelocity.x, targetSpeed, accelRate * Time.fixedDeltaTime);

        rb.linearVelocity = new Vector2(velX, rb.linearVelocity.y);

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

        if (playerAudio != null) playerAudio.PlayJump();
        if (playerAnim != null) playerAnim.TriggerJumpAnimation();

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
        
        float scaleX = Mathf.Abs(transform.localScale.x);
        
        Vector2 originLeft = new Vector2(transform.position.x - (bodyCollider.bounds.extents.x), transform.position.y + bodyCollider.bounds.extents.y);
        Vector2 originRight = new Vector2(transform.position.x + (bodyCollider.bounds.extents.x), transform.position.y + bodyCollider.bounds.extents.y);
        RaycastHit2D hitLeft = Physics2D.Raycast(originLeft, Vector2.up, data.topRaycastLength * scaleX, groundLayer);
        RaycastHit2D hitRight = Physics2D.Raycast(originRight, Vector2.up, data.topRaycastLength * scaleX, groundLayer);

        if (hitLeft && !hitRight) transform.position += Vector3.right * (data.cornerCorrectionDistance * scaleX * Time.fixedDeltaTime);
        else if (!hitLeft && hitRight) transform.position += Vector3.left * (data.cornerCorrectionDistance * scaleX * Time.fixedDeltaTime);
    }

    private void CheckGround()
    {
        float scaleY = Mathf.Abs(transform.localScale.y);
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.2f * scaleY, groundLayer);
        // Only reset canDoubleJump if the feature is actually enabled
        if (isGrounded && currentAllowDoubleJump) canDoubleJump = true;
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 scaler = transform.localScale;
        scaler.x *= -1;
        transform.localScale = scaler;
    }
    
    // --- DECELERATION ACCESS (Used by SlipperyFloor) ---
    public float GetDeceleration() => currentDeceleration;
    public void SetDeceleration(float value) => currentDeceleration = value;

    // --- GRAVITY ACCESS (Used by GravityFloor) ---
    public float GetGravityMultiplier() => currentGravityMult;
    public void SetGravityMultiplier(float value) => currentGravityMult = value;

    public void StopMovement()
    {
        rb.linearVelocity = Vector2.zero;
        rb.isKinematic = true; 
        this.enabled = false;
    }
    
    public void ResumeMovement()
    {
        rb.isKinematic = false;
        this.enabled = true;
    }

}