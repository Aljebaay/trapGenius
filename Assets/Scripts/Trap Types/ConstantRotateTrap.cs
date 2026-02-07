using UnityEngine;

public class ConstantRotateTrap : MonoBehaviour
{
    public enum RotateDirection { Clockwise, CounterClockwise }

    [Header("Settings")]
    [SerializeField] private RotateDirection direction = RotateDirection.Clockwise;
    
    [Tooltip("Speed in degrees per second (Always positive)")]
    [SerializeField] private float rotationSpeed = 100f;

    private void Update()
    {
        float dirMultiplier = (direction == RotateDirection.CounterClockwise) ? 1f : -1f;
        
        float zRotation = Mathf.Abs(rotationSpeed) * dirMultiplier * Time.deltaTime;
        
        transform.Rotate(0, 0, zRotation);
    }
}