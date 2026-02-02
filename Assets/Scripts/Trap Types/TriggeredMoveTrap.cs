using UnityEngine;

public class TriggeredMoveTrap : TrapBase
{
    [Header("Move Settings")]
    [SerializeField] private Vector3 moveOffset = new Vector3(0, -3, 0); 
    [SerializeField] private float speed = 5f;
    [SerializeField] private bool triggerOnContact = true; 

    private Vector3 initialPos;
    private Vector3 targetPos;
    private bool isTriggered = false;

    private void Awake()
    {
        initialPos = transform.position;
        targetPos = initialPos + moveOffset;
    }

    private void Update()
    {
        if (isTriggered)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
        }
    }

    public override void Activate()
    {
        if (!isTriggered)
        {
            isTriggered = true;
            if (changesPlayerData) ApplyMutationsToPlayer();
        }
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision); // Mutations

        if (triggerOnContact && collision.gameObject.CompareTag("Player"))
        {
            Activate();
        }
    }
}