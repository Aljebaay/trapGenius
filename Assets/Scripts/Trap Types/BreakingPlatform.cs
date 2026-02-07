using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Rigidbody2D))]
public class BreakingPlatform : MonoBehaviour
{
    [Header("Break Settings")]
    [Tooltip("How long the platform shakes before breaking")]
    [SerializeField] private float timeBeforeBreak = 1.0f;
    
    [Tooltip("How violent the shaking is")]
    [SerializeField] private float shakeIntensity = 0.05f;

    [Header("Fall Settings")]
    [Tooltip("Gravity scale when the platform breaks (Higher = falls faster)")]
    [SerializeField] private float fallGravity = 3.0f;
    
    [Tooltip("How long after breaking before the object is deleted from the game")]
    [SerializeField] private float destroyAfterSeconds = 5.0f;

    // Components
    private Rigidbody2D rb;
    private Vector3 initialPosition;
    private bool isTriggered = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        
        // Setup Rigidbody to stay still initially
        rb.bodyType = RigidbodyType2D.Kinematic; 
        rb.constraints = RigidbodyConstraints2D.FreezeAll;

        initialPosition = transform.position;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if player touches it from above (or any side)
        if (collision.gameObject.CompareTag("Player") && !isTriggered)
        {
            StartCoroutine(BreakRoutine());
        }
    }

    private IEnumerator BreakRoutine()
    {
        isTriggered = true;
        float timer = 0f;

        // --- PHASE 1: VIBRATE ---
        while (timer < timeBeforeBreak)
        {
            // Generate a random offset for the shake effect
            float x = Random.Range(-1f, 1f) * shakeIntensity;
            float y = Random.Range(-1f, 1f) * shakeIntensity;

            // Apply offset to the initial position
            transform.position = initialPosition + new Vector3(x, y, 0);

            timer += Time.deltaTime;
            yield return null; // Wait for next frame
        }

        // Reset position specifically to ensure it doesn't fall from a weird offset
        transform.position = initialPosition;

        // --- PHASE 2: BREAK (FALL) ---
        // Unlock the Rigidbody so physics takes over
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.constraints = RigidbodyConstraints2D.None; // Unfreeze position/rotation
        rb.constraints = RigidbodyConstraints2D.FreezeRotation; // Optional: Keep false if you want it to rotate while falling
        rb.gravityScale = fallGravity;

        // --- PHASE 3: CLEANUP ---
        yield return new WaitForSeconds(destroyAfterSeconds);
        Destroy(gameObject);
    }
}