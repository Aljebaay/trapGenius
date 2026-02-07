using UnityEngine;

public class TriggeredMoveTrap : MonoBehaviour
{
    [Header("Settings")]
    [Tooltip("How far it moves (e.g., Y = -3 to go down)")]
    [SerializeField] private Vector3 moveOffset = new Vector3(0, -3, 0); 
    [SerializeField] private float speed = 5f;
    [SerializeField] private bool triggerOnContact = true; // False if triggered by an external event

    private Vector3 initialPos;
    private Vector3 targetPos;
    private bool isTriggered = false;

    private void Awake()
    {
        initialPos = transform.position;
        targetPos = initialPos + moveOffset;
    }

    private void Update()
    {
        if (isTriggered)
        {
            // Move towards the target position smoothly
            transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (triggerOnContact && collision.gameObject.CompareTag("Player"))
        {
            Activate();
        }
    }

    // Public method so other triggers can activate this remotely
    public void Activate()
    {
        isTriggered = true;
    }
}