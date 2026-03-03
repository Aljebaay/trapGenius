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
            // RNG CHECK
            if (!ShouldActivate()) return;
            
            isTriggered = true;
            if (changesPlayerData) ApplyMutationsToPlayer();
        }
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (triggerOnContact) Activate();

            if (CanKillFromCollision(collision))
            {
                KillPlayer(collision.gameObject);
            }
        }
    }

    private void KillPlayer(GameObject player)
    {
        player.SetActive(false);
        if (GameManager.Instance != null) GameManager.Instance.GameOver();
    }
}