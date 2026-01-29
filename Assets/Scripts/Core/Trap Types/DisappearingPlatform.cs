using System.Collections;
using UnityEngine;

public class DisappearingPlatform : MonoBehaviour // Inherits MonoBehaviour (Physics-based)
{
    [Header("Settings")]
    [SerializeField] private float timeBeforeDisappear = 0.5f;
    [SerializeField] private float disappearDuration = 2.0f; // How long it stays gone
    [SerializeField] private bool reappear = true;

    [Header("Visual Feedback")]
    [SerializeField] private Color warningColor = Color.red;

    private SpriteRenderer sr;
    private BoxCollider2D col;
    private Color originalColor;
    private bool isTriggered = false;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        col = GetComponent<BoxCollider2D>();
        originalColor = sr.color;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if player lands ON TOP (checking normal)
        // or just check collision if you want it to trigger from any touch
        if (collision.gameObject.CompareTag("Player") && !isTriggered)
        {
            StartCoroutine(DisappearRoutine());
        }
    }

    private IEnumerator DisappearRoutine()
    {
        isTriggered = true;

        // Phase 1: Warning (Shake or Color Change)
        sr.color = warningColor;
        yield return new WaitForSeconds(timeBeforeDisappear);

        // Phase 2: Vanish
        sr.enabled = false;
        col.enabled = false;

        // Phase 3: Reappear (Optional)
        if (reappear)
        {
            yield return new WaitForSeconds(disappearDuration);
            sr.enabled = true;
            col.enabled = true;
            sr.color = originalColor;
            isTriggered = false;
        }
    }
}