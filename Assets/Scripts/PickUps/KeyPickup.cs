using System;
using UnityEngine;

public class KeyPickup : MonoBehaviour
{
    [SerializeField] private KeyItem keyType; // Drag the SO here
    [SerializeField] private float bobSpeed = 2f;
    [SerializeField] private float bobHeight = 0.2f;

    private Vector3 startPos;

    private void OnEnable()
    {
        startPos = transform.position;
    }

    private void Update()
    {
        
        // 2. Bob Up and Down
        float newY = startPos.y + (Mathf.Sin(Time.time * bobSpeed) * bobHeight);
        transform.position = new Vector3(startPos.x, newY, startPos.z);
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            InventoryManager.Instance.AddKey(keyType);
            AudioManager.Instance.PlayKeyPickupSound();
            Destroy(gameObject);
        }
    }
}