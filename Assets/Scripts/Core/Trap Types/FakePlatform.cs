using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(BoxCollider2D))]
public class FakePlatform : MonoBehaviour
{
    [SerializeField] private float fadeSpeed = 5f;
    private SpriteRenderer sr;
    private bool playerInside = false;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        GetComponent<BoxCollider2D>().isTrigger = true; // IMPORTANT: Must be a trigger
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInside = true;
        }
    }

    private void Update()
    {
        if (playerInside)
        {
            // Fade out rapidly to reveal it was fake
            Color c = sr.color;
            c.a = Mathf.MoveTowards(c.a, 0.2f, Time.deltaTime * fadeSpeed);
            sr.color = c;
        }
    }
}