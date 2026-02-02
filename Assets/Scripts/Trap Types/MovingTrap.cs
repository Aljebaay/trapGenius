using UnityEngine;

public class MovingTrap : MonoBehaviour
{
    [Header("Movement Config")]
    [Tooltip("If true, starts immediately.")]
    [SerializeField] private bool isAutoMove = true; 

    [SerializeField] private Vector3 localTargetOffset1 = new Vector3(-3, 0, 0); 
    [SerializeField] private Vector3 localTargetOffset2 = new Vector3(3, 0, 0); 
    [SerializeField] private float speed = 2f; 
    
    private Vector3 initialPosition; 
    private Vector3 globalTargetPosition1;
    private Vector3 globalTargetPosition2;

    private bool isMoving = false;
    private float timeAccumulator = 0f;

    private void Start()
    {
        initialPosition = transform.position; 
        globalTargetPosition1 = initialPosition + localTargetOffset1;
        globalTargetPosition2 = initialPosition + localTargetOffset2;

        if (isAutoMove) Activate();
    }

    private void Update()
    {
        if (!isMoving) return;

        timeAccumulator += Time.deltaTime;
        float t = Mathf.PingPong(timeAccumulator * speed, 1f);
        transform.position = Vector3.Lerp(globalTargetPosition1, globalTargetPosition2, t);
    }

    // --- CHANGED: Renamed to Activate for consistency ---
    public void Activate()
    {
        isMoving = true;
    }

    public void StopMovement()
    {
        isMoving = false;
    }

    private void OnDrawGizmos()
    {
        if (!Application.isPlaying)
        {
            Gizmos.color = Color.blue;
            Vector3 center = transform.position;
            Gizmos.DrawLine(center + localTargetOffset1, center + localTargetOffset2);
            Gizmos.DrawSphere(center + localTargetOffset1, 0.2f);
            Gizmos.DrawSphere(center + localTargetOffset2, 0.2f);
        }
    }
}