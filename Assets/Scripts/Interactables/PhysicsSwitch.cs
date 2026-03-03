using System.Collections;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider2D))]
public class PhysicsSwitch : TrapBase // <--- NOW INHERITS TRAPBASE
{
    [Header("Interaction Settings")]
    [SerializeField] private LayerMask contactLayer;
    [SerializeField] private float toggleCooldown = 0.5f;
    [SerializeField] private bool startOn = false;

    [Header("Visuals & Audio")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite offSprite;
    [SerializeField] private Sprite onSprite;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip toggleClip;

    [Header("Events")]
    public UnityEvent onTurnOn;
    public UnityEvent onTurnOff;

    private bool isOn = false;
    private bool canToggle = true;

    private void Start()
    {
        isOn = startOn;
        UpdateVisuals();
        
        // Optionally fire event on start if needed, but usually we don't apply mutations on start
        // to avoid weird behavior on level load.
    }

    // Override Base Activate to link to Toggle
    public override void Activate()
    {
        if(canToggle) Toggle();
    }

    // Override Base Trigger to use Switch Logic
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

        if (!canToggle) return;

        if (IsInLayerMask(collision.gameObject, contactLayer))
        {
            Toggle();
        }
    }

    public void Toggle()
    {
        isOn = !isOn;
        
        if (audioSource != null && toggleClip != null) 
            audioSource.PlayOneShot(toggleClip);

        if (isOn)
        {
            Debug.Log($"[Switch] {name} turned ON");
            
            // APPLY MUTATION
            if (changesPlayerData) ApplyMutationsToPlayer();
            
            onTurnOn.Invoke();
        }
        else
        {
            Debug.Log($"[Switch] {name} turned OFF");

            // REVERT MUTATION <--- NEW
            if (changesPlayerData) RevertMutationsFromPlayer();

            onTurnOff.Invoke();
        }

        UpdateVisuals();
        StartCoroutine(CooldownRoutine());
    }

    public void SetOn() { if(!isOn) Toggle(); }
    public void SetOff() { if(isOn) Toggle(); }

    private void UpdateVisuals()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.sprite = isOn ? onSprite : offSprite;
        }
    }

    private IEnumerator CooldownRoutine()
    {
        canToggle = false;
        yield return new WaitForSeconds(toggleCooldown);
        canToggle = true;
    }

    private bool IsInLayerMask(GameObject obj, LayerMask mask)
    {
        return (mask.value & (1 << obj.layer)) > 0;
    }

    // Check for Lethality on Collision, disable normal logic
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