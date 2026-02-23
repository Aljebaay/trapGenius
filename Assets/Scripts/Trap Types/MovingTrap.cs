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
        float t = isStopOnReachingDestination ? Mathf.Clamp01(timeAccumulator * speed) : Mathf.PingPong(timeAccumulator * speed, 1f);
        transform.position = Vector3.Lerp(globalTargetPosition1, globalTargetPosition2, t);
    }
}