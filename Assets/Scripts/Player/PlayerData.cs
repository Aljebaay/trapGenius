using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "Game/PlayerData")]
public class PlayerData : ScriptableObject
{
    [Header("Movement")]
    public float moveSpeed = 12f;
    public float acceleration = 120f;
    public float decceleration = 120f;

    [Header("Math-Based Jump")]
    public float jumpHeight = 4f;
    public float timeToJumpApex = 0.4f;
    public float fallGravityMultiplier = 2f;

    [Header("Abilities")]
    public bool allowDoubleJump = true; 

    [Header("Assists")]
    public float coyoteTime = 0.1f;
    public float jumpBufferTime = 0.1f;
    
    [Header("Corner Correction")]
    public float cornerCorrectionDistance = 0.5f;
    public float topRaycastLength = 0.5f;

    // --- Calculated Properties ---
    public float GravityStrength => -(2 * jumpHeight) / (timeToJumpApex * timeToJumpApex);
    public float JumpVelocity => Mathf.Abs(GravityStrength) * timeToJumpApex;
}