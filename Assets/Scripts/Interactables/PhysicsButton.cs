using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider2D))] 
public class PhysicsButton : TrapBase 
{
    [Header("Interaction Settings")]
    [Tooltip("What layers can press this button?")]
    [SerializeField] private LayerMask contactLayer;

    [Header("Visuals & Audio")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite releasedSprite;
    [SerializeField] private Sprite pressedSprite;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip pressClip;
    [SerializeField] private AudioClip releaseClip;

    [Header("Events")]
    public UnityEvent onPressed;
    public UnityEvent onReleased;

    private int objectsOnButton = 0;
    private bool isPressed = false;

    private void Start()
    {
        if (spriteRenderer != null && releasedSprite != null) 
            spriteRenderer.sprite = releasedSprite;
    }

    // We override Activate to link it to the Button Press
    public override void Activate()
    {
        if (!isPressed) Press();
    }

    // We override the Base trigger to use Button Logic instead of Generic Trap Logic
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            var trapCol = GetComponent<Collider2D>();
            if (CanKillFromTrigger(collision, trapCol))
            {
                KillPlayer(collision.gameObject);
                return;
            }
        }

        if (IsInLayerMask(collision.gameObject, contactLayer))
        {
            objectsOnButton++;

            if (objectsOnButton == 1 && !isPressed)
            {
                Press();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (IsInLayerMask(collision.gameObject, contactLayer))
        {
            objectsOnButton--;
            if (objectsOnButton < 0) objectsOnButton = 0;

            if (objectsOnButton == 0 && isPressed)
            {
                Release();
            }
        }
    }

    private void Press()
    {
        isPressed = true;
        
        if (spriteRenderer != null && pressedSprite != null) 
            spriteRenderer.sprite = pressedSprite;
        
        if (audioSource != null && pressClip != null) 
            audioSource.PlayOneShot(pressClip);

        // APPLY MUTATION
        if (changesPlayerData) ApplyMutationsToPlayer();

        Debug.Log($"[Button] Pressed by {name}");
        onPressed.Invoke();
    }

    private void Release()
    {
        isPressed = false;

        if (spriteRenderer != null && releasedSprite != null) 
            spriteRenderer.sprite = releasedSprite;

        if (audioSource != null && releaseClip != null) 
            audioSource.PlayOneShot(releaseClip);
        
        // REVERT MUTATION <--- NEW
        if (changesPlayerData) RevertMutationsFromPlayer();

        Debug.Log($"[Button] Released by {name}");
        onReleased.Invoke();
    }

    private bool IsInLayerMask(GameObject obj, LayerMask mask)
    {
        return (mask.value & (1 << obj.layer)) > 0;
    }
    
    // Check for Lethality on Collision, ignore logic to prevent double firing
    protected override void OnCollisionEnter2D(Collision2D collision) 
    { 
        if (collision.gameObject.CompareTag("Player") && CanKillFromCollision(collision))
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