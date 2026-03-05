using UnityEngine;
using UnityEngine.Tilemaps;

public class TriggeredScaleTrap : TrapBase
{
    public enum ScaleAnchor { Center, Left, Right, Top, Bottom }

    [Header("Scale Settings")]
    [SerializeField] private Vector3 targetScale = new Vector3(0, 1, 1);
    [SerializeField] private float speed = 5f;
    [SerializeField] private ScaleAnchor anchor = ScaleAnchor.Center;

    private Tilemap sr;
    private bool isTriggered = false;
    private Vector3 localAnchorOffset;
    private Vector3 initialWorldAnchorPos;

    private void Awake()
    {
        sr = GetComponent<Tilemap>();
        if (sr != null)
        {
            sr.CompressBounds();
            Bounds b = sr.localBounds;
            localAnchorOffset = b.center; 

            switch (anchor)
            {
                case ScaleAnchor.Left: localAnchorOffset.x -= b.extents.x; break;
                case ScaleAnchor.Right: localAnchorOffset.x += b.extents.x; break;
                case ScaleAnchor.Top: localAnchorOffset.y += b.extents.y; break;
                case ScaleAnchor.Bottom: localAnchorOffset.y -= b.extents.y; break;
            }
            initialWorldAnchorPos = transform.TransformPoint(localAnchorOffset);
        }
    }

    private void Update()
    {
        if (isTriggered)
        {
            transform.localScale = Vector3.MoveTowards(transform.localScale, targetScale, speed * Time.deltaTime);

            if (anchor != ScaleAnchor.Center)
            {
                Vector3 currentWorldAnchorPos = transform.TransformPoint(localAnchorOffset);
                Vector3 correction = initialWorldAnchorPos - currentWorldAnchorPos;
                transform.position += correction;
            }
        }
    }

    public override void Activate()
    {
        if (!isTriggered)
        {
            if (!ShouldActivate()) return;
            // Recalculate anchor just before triggering to be safe
            initialWorldAnchorPos = transform.TransformPoint(localAnchorOffset);
            isTriggered = true;
            if(changesPlayerData) ApplyMutationsToPlayer();
        }
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Activate();

            if (CanKillFromCollision(collision))
            {
                KillPlayer(collision.gameObject);
            }
        }
    }

    private void KillPlayer(GameObject player)
    {
        player.SetActive(false);
        if (GameManager.Instance != null) GameManager.Instance.GameOver();
    }
}