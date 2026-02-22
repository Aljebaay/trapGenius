using UnityEngine;

[System.Serializable]
public class PlayerMutation
{
    public enum StatType 
    { 
        MoveSpeed, 
        JumpHeight, 
        GravityMultiplier, 
        InvertControls,
        PlayerScale,
        CoyoteTime,
        DoubleJump
    }
    
    public StatType statToChange;
    
    [Tooltip("Used for Speed, Jump, Gravity, Scale, Coyote Time")]
    public float numberValue;
    
    [Tooltip("Used for Invert Controls and Double Jump")]
    public bool booleanValue;

    [Header("Duration Settings")]
    [Tooltip("If true, the stat will revert to its previous value after the duration.")]
    public bool isTemporaryChange = false;

    [Tooltip("Time in seconds before reverting (only if isTemporaryChange is true).")]
    public float duration = 5f;
}