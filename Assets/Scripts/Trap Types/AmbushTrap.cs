using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class AmbushTrap : TrapBase
{
    public enum AmbushType { FallGravity, PopUp }

    [Header("Ambush Settings")]
    [SerializeField] private AmbushType type = AmbushType.FallGravity;
    [SerializeField] private Vector3 popOffset = new Vector3(0, 1, 0); 
    [SerializeField] private float popSpeed = 15f; 

    private Rigidbody2D rb;
    private Vector3 targetPos;
    private bool isTriggered = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        if (type == AmbushType.FallGravity)
        {
            rb.gravityScale = 0; 
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
        }
        else 
        {
            rb.isKinematic = true; 
            targetPos = transform.position + popOffset;
        }
    }

    private void Update()
    {
        if (isTriggered && type == AmbushType.PopUp)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, popSpeed * Time.deltaTime);
        }
    }

    public override void Activate()
    {
        if (isTriggered) return;
        
        // RNG CHECK
        if (!ShouldActivate()) return;

        isTriggered = true;
        if (changesPlayerData) ApplyMutationsToPlayer();

        if (type == AmbushType.FallGravity)
        {
            rb.constraints = RigidbodyConstraints2D.None; 
            rb.constraints = RigidbodyConstraints2D.FreezeRotation; 
            rb.gravityScale = 3f; 
        }
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        // RNG CHECK
        if (!ShouldActivate()) return;

        if (changesPlayerData) ApplyMutationsToPlayer(collision.gameObject);

        var trapCol = GetComponent<Collider2D>();
        if (collision.CompareTag("Player") && CanKillFromTrigger(collision, trapCol))
        {
            KillPlayer(collision.gameObject);
        }
    }

    private void KillPlayer(GameObject player)
    {
        player.SetActive(false);
        if(GameManager.Instance != null) GameManager.Instance.GameOver();
    }
}