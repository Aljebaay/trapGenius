using UnityEngine;

public class LockedDoor : MonoBehaviour
{
    [Header("Key Settings")]
    [SerializeField] private KeyItem requiredKey;
    [SerializeField] private bool consumeKey = false; 

    [Header("Door State")]
    [SerializeField] private GameObject doorVisuals; // The sprite/obstacle part
    [SerializeField] private Collider2D doorCollider; // The physical part

    private bool isOpen = false;

    private void Start()
    {
        // Auto-assign if empty
        if (doorVisuals == null) doorVisuals = gameObject;
        if (doorCollider == null) doorCollider = GetComponent<Collider2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isOpen) return;

        if (collision.gameObject.CompareTag("Player"))
        {
            CheckKey();
        }
    }

    // --- KEY LOGIC ---
    public void CheckKey()
    {
        // Only check key if one is actually assigned
        if (requiredKey != null)
        {
            if (InventoryManager.Instance.HasKey(requiredKey))
            {
                OpenDoor();
                if (consumeKey) InventoryManager.Instance.RemoveKey(requiredKey);
            }
            else
            {
                Debug.Log("🔒 Locked! You need the " + requiredKey.keyName);
            }
        }
    }

    // --- REMOTE CONTROL LOGIC (For Buttons/Switches) ---

    public void OpenDoor()
    {
        if (isOpen) return;
        isOpen = true;
        
        Debug.Log("🔓 Door Opened!");
        
        if (doorVisuals != null) doorVisuals.SetActive(false);
        if (doorCollider != null) doorCollider.enabled = false;
    }

    public void CloseDoor()
    {
        if (!isOpen) return;
        isOpen = false;

        Debug.Log("🔒 Door Closed!");

        if (doorVisuals != null) doorVisuals.SetActive(true);
        if (doorCollider != null) doorCollider.enabled = true;
    }

    public void ToggleDoor()
    {
        if (isOpen) CloseDoor();
        else OpenDoor();
    }
}