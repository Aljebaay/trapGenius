using UnityEngine;

public class LockedDoor : MonoBehaviour
{
    [SerializeField] private KeyItem requiredKey;
    [SerializeField] private bool consumeKey = false; // Does the key break after use?

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            CheckDoor();
        }
    }

    public void CheckDoor()
    {
        if (InventoryManager.Instance.HasKey(requiredKey))
        {
            Unlock();
            if (consumeKey) InventoryManager.Instance.RemoveKey(requiredKey);
        }
        else
        {
            Debug.Log("🔒 Locked! You need the " + requiredKey.keyName);
            // Optional: Play "Locked" sound or shake door
        }
    }

    private void Unlock()
    {
        Debug.Log("🔓 Door Opened!");
        // Animation, Disable Collider, or Destroy
        gameObject.SetActive(false); 
    }
}