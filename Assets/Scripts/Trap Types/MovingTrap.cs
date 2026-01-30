using UnityEngine;

public class MovingTrap : MonoBehaviour
{
    [Header("Movement Config")]
    [Tooltip("The first target position relative to the trap's initial position.")]
    [SerializeField] private Vector3 localTargetOffset1 = new Vector3(-3, 0, 0); 
    [Tooltip("The second target position relative to the trap's initial position.")]
    [SerializeField] private Vector3 localTargetOffset2 = new Vector3(3, 0, 0); 
    [SerializeField] private float speed = 2f; // How fast the trap moves between points
    
    private Vector3 initialPosition; // The trap's position at the start
    private Vector3 globalTargetPosition1;
    private Vector3 globalTargetPosition2;

    private void Start()
    {
        initialPosition = transform.position; // Store the initial world position

        // Calculate the absolute world positions for the targets
        globalTargetPosition1 = initialPosition + localTargetOffset1;
        globalTargetPosition2 = initialPosition + localTargetOffset2;
    }

    private void Update()
    {
        // PingPong creates a value that bounces between 0 and 1 over time
        // The speed now dictates how quickly it completes one full cycle (to 1 and back to 0)
        float t = Mathf.PingPong(Time.time * speed, 1f);
        
        // Lerp between globalTargetPosition1 and globalTargetPosition2
        transform.position = Vector3.Lerp(globalTargetPosition1, globalTargetPosition2, t);
    }

    // Optional: Draw gizmos in the editor to visualize the path
    private void OnDrawGizmos()
    {
        if (Application.isPlaying)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(globalTargetPosition1, globalTargetPosition2);
            Gizmos.DrawSphere(globalTargetPosition1, 0.2f);
            Gizmos.DrawSphere(globalTargetPosition2, 0.2f);
        }
        else // Show gizmos in editor when not playing
        {
            Vector3 currentInitialPos = transform.position;
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(currentInitialPos + localTargetOffset1, currentInitialPos + localTargetOffset2);
            Gizmos.DrawSphere(currentInitialPos + localTargetOffset1, 0.2f);
            Gizmos.DrawSphere(currentInitialPos + localTargetOffset2, 0.2f);
        }
    }
}