using UnityEngine;


public class MovingTrap : TrapBase
{
    [Header("Movement Config")]
    [SerializeField] private bool isAutoMove = false; 
    [SerializeField] private bool isStopOnReachingDestination = false;
    [SerializeField] private Vector3 localTargetOffset1 = new Vector3(0, 0, 0); 
    [SerializeField] private Vector3 localTargetOffset2 = new Vector3(-5, 0, 0); 
    [SerializeField] private float speed = 1f; 
    
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
        float t = isStopOnReachingDestination ? Mathf.Clamp01(timeAccumulator * speed) : Mathf.PingPong(timeAccumulator * speed, 1f);
        
        transform.position = Vector3.Lerp(globalTargetPosition1, globalTargetPosition2, t);
    }

    // OVERRIDE THE BASE ACTIVATE
    public override void Activate()
    {
        if (!isMoving)
        {
            isMoving = true;
            // Apply mutations (inherited from TrapBase)
            if (changesPlayerData) ApplyMutationsToPlayer();
        }
    }
    
    public void StopMovement() { isMoving = false; }
    
    // OnCollisionEnter2D is already handled in TrapBase for mutations!
    // But if you want custom kill logic, you can override it and call base.OnCollisionEnter2D(collision)

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