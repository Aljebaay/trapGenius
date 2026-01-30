using UnityEngine;
using UnityEngine.Tilemaps;

public class TriggeredScaleTrap : MonoBehaviour
{
    public enum ScaleAnchor { Center, Left, Right, Top, Bottom }

    [Header("Settings")]
    [Tooltip("The final scale size.")]
    [SerializeField] private Vector3 targetScale = new Vector3(0, 1, 1);
    [SerializeField] private float speed = 5f;

    [Header("Anchor Settings")]
    [Tooltip("Which side should stay still?")]
    [SerializeField] private ScaleAnchor anchor = ScaleAnchor.Center;

    private Tilemap sr;
    private bool isTriggered = false;

    // We store the specific point on the object (in local space) that should act as the anchor
    private Vector3 localAnchorOffset;
    // We store where that point is in the World, so we can lock the object to it
    private Vector3 initialWorldAnchorPos;

    private void Awake()
    {
        sr = GetComponent<Tilemap>();

        if (sr == null)
        {
            Debug.LogError("No Tilemap component found!");
            return;
        }

        // 1. Force the Tilemap to recalculate bounds to fit exactly around the painted tiles
        sr.CompressBounds();

        // 2. Calculate the Local Anchor Point based on the unscaled bounds
        // localBounds returns the size in "Local Space" (ignoring the object's current scale), which is exactly what we want.
        Bounds b = sr.localBounds;
        localAnchorOffset = b.center; // Start at center

        switch (anchor)
        {
            case ScaleAnchor.Left:
                localAnchorOffset.x -= b.extents.x;
                break;
            case ScaleAnchor.Right:
                localAnchorOffset.x += b.extents.x;
                break;
            case ScaleAnchor.Top:
                localAnchorOffset.y += b.extents.y;
                break;
            case ScaleAnchor.Bottom:
                localAnchorOffset.y -= b.extents.y;
                break;
            case ScaleAnchor.Center:
                // Already at center
                break;
        }

        // 3. Store where this anchor point is currently in the world
        initialWorldAnchorPos = transform.TransformPoint(localAnchorOffset);
    }

    private void Update()
    {
        if (isTriggered)
        {
            // 1. Move Scale
            transform.localScale = Vector3.MoveTowards(transform.localScale, targetScale, speed * Time.deltaTime);

            // 2. Correct Position (The Anti-Drift Method)
            if (anchor != ScaleAnchor.Center)
            {
                // Calculate where the local anchor point is NOW, after the scale change
                Vector3 currentWorldAnchorPos = transform.TransformPoint(localAnchorOffset);

                // Calculate the difference between where it IS and where it SHOULD BE
                Vector3 correction = initialWorldAnchorPos - currentWorldAnchorPos;

                // Apply correction
                transform.position += correction;
            }
        }
    }

    public void Activate()
    {
        // Update the world anchor position just before triggering in case the object moved before activation
        if (!isTriggered)
        {
            initialWorldAnchorPos = transform.TransformPoint(localAnchorOffset);
            isTriggered = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) Activate();
    }
}