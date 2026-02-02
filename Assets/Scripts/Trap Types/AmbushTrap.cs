using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class AmbushTrap : TrapBase
{
    [Header("⚠️ SETUP INSTRUCTIONS")]
    [TextArea(2, 3)] 
    [SerializeField] private string setupNote = "This trap is PASSIVE. It will not move until triggered.\n\n1. Create an empty GameObject.\n2. Add 'AreaTrigger' script.\n3. Link 'OnTriggerEnter' to this object's Activate() method.";

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
        else // PopUp
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

    // --- TRAP BASE OVERRIDE ---
    public override void Activate()
    {
        if (isTriggered) return;
        isTriggered = true;

        // Apply mutations remotely if triggered by a sequencer
        if (changesPlayerData) ApplyMutationsToPlayer();

        if (type == AmbushType.FallGravity)
        {
            rb.constraints = RigidbodyConstraints2D.None; 
            rb.constraints = RigidbodyConstraints2D.FreezeRotation; 
            rb.gravityScale = 3f; 
        }
    }

    // --- COLLISION LOGIC ---
    
    // 1. Solid Contact
    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision); // Logic for Mutations
        
        if (collision.gameObject.CompareTag("Player"))
        {
            KillPlayer(collision.gameObject);
        }
    }

    // 2. Trigger Contact
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision); // Logic for Mutations

        if (collision.CompareTag("Player"))
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