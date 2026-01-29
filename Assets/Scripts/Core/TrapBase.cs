using UnityEngine;

public abstract class TrapBase : MonoBehaviour
{
    // Common logic: Detect Player
    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            OnPlayerContact(collision.gameObject);
        }
    }

    // Every trap decides what happens on contact
    protected abstract void OnPlayerContact(GameObject player);
}