using UnityEngine;

public class MovingTrap : TrapBase
{
    [Header("Movement Config")]
    [SerializeField] private bool isAutoMove = false; 
    [SerializeField] private bool isStopOnReachingDestination = false;
    [SerializeField] private Vector3 localTargetOffset1 = new Vector3(0, 0, 0); 
    [SerializeField] private Vector3 localTargetOffset2 = new Vector3(-5, 0, 0); 
    [SerializeField] private float speed = 1f; 

    [Header("Rotation Config")]
    [Tooltip("Total degrees to rotate over the journey.")]
    [SerializeField] private float rotationAmount = 360f; 
    [Tooltip("If false: Rotates Clockwise towards target. If true: Rotates Counter-Clockwise towards target.")]
    [SerializeField] private bool revertRotationDirection = false;
    
    private Vector3 initialPosition; 
    private Quaternion initialRotation; // Store starting rotation
    private Vector3 globalTargetPosition1; 
    private Vector3 globalTargetPosition2;
    private bool isMoving = false;
    private float timeAccumulator = 0f;

    private void Start()
    {
        initialPosition = transform.position; 
        initialRotation = transform.rotation; // Capture rotation

        globalTargetPosition1 = initialPosition + localTargetOffset1;
        globalTargetPosition2 = initialPosition + localTargetOffset2;

        if (isAutoMove) Activate();
    }

    public override void Activate()
    {
        if (isMoving) return;
        if (!ShouldActivate()) return;

        isMoving = true;
        if (changesPlayerData) ApplyMutationsToPlayer();
    }

    public void StopMovement()
    {
        isMoving = false;
    }
    
    private void Update()
    {
        if (!isMoving) return;

        timeAccumulator += Time.deltaTime;
        
        // Calculate the interpolation factor 't' (0 to 1)
        float t = isStopOnReachingDestination 
            ? Mathf.Clamp01(timeAccumulator * speed) 
            : Mathf.PingPong(timeAccumulator * speed, 1f);

        // 1. Handle Position
        transform.position = Vector3.Lerp(globalTargetPosition1, globalTargetPosition2, t);

        // 2. Handle Rotation
        // In Unity 2D, Negative Z is Clockwise, Positive Z is Counter-Clockwise.
        // Default (revert=false): -1 * amount * t (Clockwise)
        float directionMult = revertRotationDirection ? 1f : -1f;
        float currentAngle = directionMult * rotationAmount * t;

        // Apply rotation relative to the initial rotation
        transform.rotation = initialRotation * Quaternion.Euler(0, 0, currentAngle);
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);

        if (collision.gameObject.CompareTag("Player") && CanKillFromCollision(collision))
        {
            KillPlayer(collision.gameObject);
        }
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);

        var trapCol = GetComponent<Collider2D>();
        if (collision.CompareTag("Player") && CanKillFromTrigger(collision, trapCol))
        {
            KillPlayer(collision.gameObject);
        }
    }

    private void KillPlayer(GameObject player)
    {
        player.SetActive(false);
        if (GameManager.Instance != null) GameManager.Instance.GameOver();
    }
}