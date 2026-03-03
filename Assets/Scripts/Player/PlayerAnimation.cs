using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerAnimation : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private PlayerController controller;
    [SerializeField] private Rigidbody2D rb;

    [Header("Animator Parameter Names")]
    [SerializeField] private string isRunningParam = "IsRunning";
    [SerializeField] private string isGroundedParam = "IsGrounded";
    [SerializeField] private string jumpTriggerParam = "Jump";

    private Animator anim;
    private int isRunningHash;
    private int isGroundedHash;
    private int jumpTriggerHash;

    private void Awake()
    {
        anim = GetComponent<Animator>();

        // Convert string names to Hashes for better performance
        isRunningHash = Animator.StringToHash(isRunningParam);
        isGroundedHash = Animator.StringToHash(isGroundedParam);
        jumpTriggerHash = Animator.StringToHash(jumpTriggerParam);
    }

    private void Update()
    {
        if (controller == null || rb == null) return;

        // 1. Handle Running (Based on horizontal speed)
        // We use 0.1f to give it a little buffer so it doesn't flicker at 0.0001 speed
        bool isMoving = Mathf.Abs(rb.linearVelocity.x) > 0.1f;
        anim.SetBool(isRunningHash, isMoving);

        // 2. Handle Grounded State (For landing transitions)
        anim.SetBool(isGroundedHash, controller.IsGrounded);
    }

    // Call this specifically when the player presses the jump button
    public void TriggerJumpAnimation()
    {
        anim.SetTrigger(jumpTriggerHash);
    }
}