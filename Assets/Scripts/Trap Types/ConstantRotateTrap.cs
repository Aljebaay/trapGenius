using UnityEngine;

public class ConstantRotateTrap : TrapBase
{
    public enum RotateDirection { Clockwise, CounterClockwise }

    [Header("Rotation Settings")]
    [SerializeField] private RotateDirection direction = RotateDirection.Clockwise;
    [SerializeField] private float rotationSpeed = 100f;
    [SerializeField] private bool autoStart = true;

    private bool isSpinning = false;

    private void Start()
    {
        if (autoStart) Activate();
    }

    public override void Activate()
    {
        // RNG CHECK
        if (!ShouldActivate()) return;

        isSpinning = true;
        if (changesPlayerData) ApplyMutationsToPlayer();
    }

    private void Update()
    {
        if (!isSpinning) return;

        float dirMultiplier = (direction == RotateDirection.CounterClockwise) ? 1f : -1f;
        float zRotation = Mathf.Abs(rotationSpeed) * dirMultiplier * Time.deltaTime;
        transform.Rotate(0, 0, zRotation);
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);

        if (collision.gameObject.CompareTag("Player") && CanKillFromCollision(collision))
        {
            KillPlayer(collision.gameObject);
        }
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);

        var trapCol = GetComponent<Collider2D>();
        if (collision.CompareTag("Player") && CanKillFromTrigger(collision, trapCol))
        {
            KillPlayer(collision.gameObject);
        }
    }

    private void KillPlayer(GameObject player)
    {
        player.SetActive(false);
        if (GameManager.Instance != null) GameManager.Instance.GameOver();
    }
}