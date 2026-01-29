using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "Game/PlayerData")]
public class PlayerData : ScriptableObject
{
    [Header("Movement")]
    public float moveSpeed = 10f;
    public float acceleration = 50f; // How fast we reach max speed
    public float decceleration = 50f; // How fast we stop (Critical for tight controls)

    [Header("Jump")]
    public float jumpForce = 16f;
    public float gravityScale = 3f; // Default gravity
    public float fallGravityMultiplier = 1.5f; // Falls faster than rising
    public float jumpCutMultiplier = 2f; // Short hop if button released

    [Header("Assists (Game Feel)")]
    public float coyoteTime = 0.1f; // Time to jump after leaving ledge
    public float jumpBufferTime = 0.1f; // Time to queue jump before hitting ground
}