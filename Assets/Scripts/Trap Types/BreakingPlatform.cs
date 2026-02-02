using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BreakingPlatform : MonoBehaviour
{
    [Header("Break Settings")]
    [SerializeField] private float timeBeforeBreak = 1.0f;
    [SerializeField] private float shakeIntensity = 0.05f;

    [Header("Fall Settings")]
    [SerializeField] private float fallGravity = 3.0f;
    [SerializeField] private float destroyAfterSeconds = 5.0f;

    private Rigidbody2D rb;
    private Vector3 initialPosition;
    private bool isTriggered = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic; 
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        initialPosition = transform.position;
    }

    // --- NEW PUBLIC METHOD FOR TRIGGERS ---
    public void Activate()
    {
        if (isTriggered) return;
        StartCoroutine(BreakRoutine());
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Activate();
        }
    }

    private IEnumerator BreakRoutine()
    {
        isTriggered = true;
        float timer = 0f;

        // Phase 1: Vibrate
        while (timer < timeBeforeBreak)
        {
            float x = Random.Range(-1f, 1f) * shakeIntensity;
            float y = Random.Range(-1f, 1f) * shakeIntensity;
            transform.position = initialPosition + new Vector3(x, y, 0);
            timer += Time.deltaTime;
            yield return null;
        }

        transform.position = initialPosition;

        // Phase 2: Fall
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.constraints = RigidbodyConstraints2D.None; 
        rb.constraints = RigidbodyConstraints2D.FreezeRotation; 
        rb.gravityScale = fallGravity;

        // Phase 3: Cleanup
        yield return new WaitForSeconds(destroyAfterSeconds);
        Destroy(gameObject);
    }
}