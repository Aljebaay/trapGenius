using UnityEngine;

public class TriggeredRotateTrap : TrapBase
{
    [Header("Settings")]
    [Tooltip("Rotation in degrees (e.g., 90)")]
    [SerializeField] private float rotationAmount = 90f;
    [SerializeField] private float speed = 100f;
    [Tooltip("Reference to the child object that rotates.")]
    [SerializeField] private Transform parentRotationPivot; 

    private Quaternion targetLocalRotation;
    private bool isTriggered = false;

    private void Awake()
    {
        if (parentRotationPivot == null)
        {
            // If user forgot to assign, try to find a child, or disable.
            if(transform.childCount > 0) parentRotationPivot = transform.GetChild(0);
            else { enabled = false; return; }
        }

        Quaternion initialLocalRotation = parentRotationPivot.localRotation;
        targetLocalRotation = Quaternion.Euler(0, 0, initialLocalRotation.eulerAngles.z + rotationAmount);
    }

    private void Update()
    {
        if (isTriggered)
        {
            parentRotationPivot.localRotation = Quaternion.RotateTowards(parentRotationPivot.localRotation, targetLocalRotation, speed * Time.deltaTime);
        }
    }

    public override void Activate()
    {
        if(!isTriggered)
        {
            // RNG CHECK
            if (!ShouldActivate()) return;
            
            isTriggered = true;
            if (changesPlayerData) ApplyMutationsToPlayer();
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