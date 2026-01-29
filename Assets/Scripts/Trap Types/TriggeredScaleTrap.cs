using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class TriggeredScaleTrap : MonoBehaviour
{
    public enum ScaleAnchor { Center, Left, Right, Top, Bottom }

    [Header("Settings")]
    [Tooltip("The final scale size.")]
    [SerializeField] private Vector3 targetScale = new Vector3(0, 1, 1); // Default: Shrink X to 0
    [SerializeField] private float speed = 5f;
    
    [Header("Anchor Settings")]
    [Tooltip("Which side should stay still?")]
    [SerializeField] private ScaleAnchor anchor = ScaleAnchor.Center;

    private SpriteRenderer sr;
    private Vector3 initialScale;
    private Vector2 unscaledSpriteSize; // The size of the sprite if scale was 1,1
    private bool isTriggered = false;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        initialScale = transform.localScale;

        // Calculate the "Raw" size of the sprite (World Size / Local Scale)
        // This allows us to know how big the sprite is regardless of current scale
        if (transform.localScale.x != 0 && transform.localScale.y != 0)
        {
            unscaledSpriteSize = new Vector2(
                sr.bounds.size.x / transform.localScale.x,
                sr.bounds.size.y / transform.localScale.y
            );
        }
    }

    private void Update()
    {
        if (isTriggered)
        {
            // 1. Calculate the NEW scale
            Vector3 currentScale = transform.localScale;
            Vector3 newScale = Vector3.MoveTowards(currentScale, targetScale, speed * Time.deltaTime);

            // 2. Apply the Scale
            transform.localScale = newScale;

            // 3. Compensate Position (The "Pivot" Trick)
            // We calculate how much the scale changed this frame, and move the object 
            // in the opposite direction to keep one side fixed.
            if (anchor != ScaleAnchor.Center)
            {
                ApplyPositionCompensation(currentScale, newScale);
            }
        }
    }

    private void ApplyPositionCompensation(Vector3 oldScale, Vector3 newScale)
    {
        Vector3 scaleChange = newScale - oldScale;
        Vector3 adjustment = Vector3.zero;

        // Logic: If we shrink X, the center moves. We must move the center 
        // back to where the edge used to be.
        
        // Handle Horizontal Anchors (Uses transform.right to support Rotation)
        if (anchor == ScaleAnchor.Left)
        {
            // If shrinking (neg change), we pull the center LEFT (neg direction)
            // adjustment = (Change * Size * 0.5)
            float moveAmount = scaleChange.x * unscaledSpriteSize.x * 0.5f;
            adjustment += transform.right * moveAmount; 
        }
        else if (anchor == ScaleAnchor.Right)
        {
            // If shrinking, we pull the center RIGHT
            float moveAmount = scaleChange.x * unscaledSpriteSize.x * 0.5f;
            adjustment -= transform.right * moveAmount;
        }

        // Handle Vertical Anchors (Uses transform.up)
        if (anchor == ScaleAnchor.Bottom)
        {
            float moveAmount = scaleChange.y * unscaledSpriteSize.y * 0.5f;
            adjustment += transform.up * moveAmount;
        }
        else if (anchor == ScaleAnchor.Top)
        {
            float moveAmount = scaleChange.y * unscaledSpriteSize.y * 0.5f;
            adjustment -= transform.up * moveAmount;
        }

        transform.position += adjustment;
    }

    public void Activate()
    {
        isTriggered = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
         if (collision.gameObject.CompareTag("Player")) Activate();
    }
}