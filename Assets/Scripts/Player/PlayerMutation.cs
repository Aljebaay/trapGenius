using UnityEngine;

[System.Serializable]
public class PlayerMutation
{
    public enum StatType 
    { 
        MoveSpeed, 
        JumpHeight, 
        GravityMultiplier, 
        InvertControls 
    }
    
    public StatType statToChange;
    
    [Tooltip("Used for Speed, Jump, Gravity")]
    public float numberValue;
    
    [Tooltip("Used for Invert Controls")]
    public bool booleanValue;
}