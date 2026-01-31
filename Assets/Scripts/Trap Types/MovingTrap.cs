using UnityEngine;

public class MovingTrap : MonoBehaviour
{
    [Header("Movement Config")]
    [Tooltip("If true, the trap starts moving immediately. If false, it waits for MoveOnTrigger()")]
    [SerializeField] private bool isAutoMove = true; 

    [Tooltip("The first target position relative to the trap's initial position.")]
    [SerializeField] private Vector3 localTargetOffset1 = new Vector3(-3, 0, 0); 
    [Tooltip("The second target position relative to the trap's initial position.")]
    [SerializeField] private Vector3 localTargetOffset2 = new Vector3(3, 0, 0); 
    [SerializeField] private float speed = 2f; 
    
    private Vector3 initialPosition; 
    private Vector3 globalTargetPosition1;
    private Vector3 globalTargetPosition2;

    // Internal state
    private bool isMoving = false;
    private float timeAccumulator = 0f; // Tracks time only while moving

    private void Start()
    {
        initialPosition = transform.position; 

        // Calculate the absolute world positions
        globalTargetPosition1 = initialPosition + localTargetOffset1;
        globalTargetPosition2 = initialPosition + localTargetOffset2;

        // If Auto Move is checked, start immediately
        if (isAutoMove)
        {
            isMoving = true;
        }
    }

    private void Update()
    {
        // If we are not allowed to move, do nothing
        if (!isMoving) return;

        // We use a custom accumulator instead of Time.time so the trap 
        // starts smoothly from 0 whenever it is triggered.
        timeAccumulator += Time.deltaTime;

        // Calculate t based on our local time accumulator
        float t = Mathf.PingPong(timeAccumulator * speed, 1f);
        
        transform.position = Vector3.Lerp(globalTargetPosition1, globalTargetPosition2, t);
    }

    // --- PUBLIC METHODS ---

    // Call this function from your trigger script/event
    public void MoveOnTrigger()
    {
        isMoving = true;
    }

    // Optional: Call this if you want to pause/stop the trap later
    public void StopMovement()
    {
        isMoving = false;
    }

    // --- VISUALIZATION ---

    private void OnDrawGizmos()
    {
        if (Application.isPlaying)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(globalTargetPosition1, globalTargetPosition2);
            Gizmos.DrawSphere(globalTargetPosition1, 0.2f);
            Gizmos.DrawSphere(globalTargetPosition2, 0.2f);
        }
        else 
        {
            Vector3 currentInitialPos = transform.position;
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(currentInitialPos + localTargetOffset1, currentInitialPos + localTargetOffset2);
            Gizmos.DrawSphere(currentInitialPos + localTargetOffset1, 0.2f);
            Gizmos.DrawSphere(currentInitialPos + localTargetOffset2, 0.2f);
        }
    }
}