using UnityEngine;

public class TriggeredRotateTrap : MonoBehaviour
{
    [Header("Settings")]
    [Tooltip("Rotation in degrees (e.g., 90)")]
    [SerializeField] private float rotationAmount = 90f;
    [SerializeField] private float speed = 100f;

    private Quaternion targetRotation;
    private bool isTriggered = false;

    private void Awake()
    {
        // Calculate the final rotation based on current rotation
        targetRotation = Quaternion.Euler(0, 0, transform.eulerAngles.z + rotationAmount);
    }

    private void Update()
    {
        if (isTriggered)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, speed * Time.deltaTime);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isTriggered = true;
        }
    }
}