using UnityEngine;

public class CoinPickup : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private int coinValue = 1;
    [SerializeField] private float rotateSpeed = 100f;
    [SerializeField] private float bobSpeed = 2f;
    [SerializeField] private float bobHeight = 0.2f;

    private Vector3 startPos;
    private bool isCollected = false;

    private void Start()
    {
        startPos = transform.position;
    }

    private void Update()
    {
        // 1. Rotate
        transform.Rotate(0, 0, rotateSpeed * Time.deltaTime);

        // 2. Bob Up and Down
        float newY = startPos.y + (Mathf.Sin(Time.time * bobSpeed) * bobHeight);
        transform.position = new Vector3(startPos.x, newY, startPos.z);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isCollected) return;

        if (collision.CompareTag("Player"))
        {
            isCollected = true;
            Collect();
        }
    }

    private void Collect()
    {
        // 1. Add to Game Manager
        if (GameManager.Instance != null)
        {
            InventoryManager.Instance.AddCoins(coinValue);
        }

        // 2. Play Sound
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayCoin();
        }

        // 3. Visuals (Disable sprite, maybe spawn particle later)
        // We destroy the object so it can't be picked up again
        Destroy(gameObject);
    }
}