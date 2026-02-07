using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class AmbushTrap : MonoBehaviour
{
    [Header("⚠️ SETUP INSTRUCTIONS")]
    [TextArea(2, 3)] // Creates a text box in the inspector
    [SerializeField] private string setupNote = "This trap is PASSIVE. It will not move until triggered.\n\n1. Create an empty GameObject.\n2. Add 'AreaTrigger' script.\n3. Link 'OnTriggerEnter' to this object's Activate() method.";

    public enum AmbushType { FallGravity, PopUp }

    [Header("Settings")]
    [SerializeField] private AmbushType type = AmbushType.FallGravity;
    [SerializeField] private Vector3 popOffset = new Vector3(0, 1, 0); // Only for PopUp
    [SerializeField] private float popSpeed = 15f; 

    private Rigidbody2D rb;
    private Vector3 targetPos;
    private bool isTriggered = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        
        // Setup initial state based on type
        if (type == AmbushType.FallGravity)
        {
            rb.gravityScale = 0; 
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
        }
        else // PopUp
        {
            rb.isKinematic = true; // No physics, just manual movement
            targetPos = transform.position + popOffset;
        }
    }

    private void Update()
    {
        // Handle PopUp movement
        if (isTriggered && type == AmbushType.PopUp)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, popSpeed * Time.deltaTime);
        }
    }

    public void Activate()
    {
        if (isTriggered) return;
        isTriggered = true;

        if (type == AmbushType.FallGravity)
        {
            // Unlock physics and let gravity take over
            rb.constraints = RigidbodyConstraints2D.None; 
            rb.constraints = RigidbodyConstraints2D.FreezeRotation; // Keep rotation frozen (optional)
            rb.gravityScale = 3f; // Heavy fall
        }
    }

    // --- LETHALITY LOGIC ---

    // 1. For Solid objects (Falling Rocks)
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            KillPlayer(collision.gameObject);
        }
    }

    // 2. For Trigger objects (Spikes)
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            KillPlayer(collision.gameObject);
        }
    }

    private void KillPlayer(GameObject player)
    {
        // Disable player to prevent multiple deaths/movement
        player.SetActive(false);
        
        // Call the central Game Manager
        if(GameManager.Instance != null)
        {
            GameManager.Instance.GameOver();
        }
        else
        {
            Debug.LogError("GameManager not found in scene!");
        }
    }
}