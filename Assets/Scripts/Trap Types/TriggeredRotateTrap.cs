using UnityEngine;

public class TriggeredRotateTrap : MonoBehaviour
{
    [Header("⚠️ SETUP INSTRUCTIONS")]
    [TextArea(2, 3)] 
    [SerializeField] private string setupNote = "This trap is PASSIVE. It will not move until triggered.\n\n1. Create an empty ParentGO.\n2. Position it at the centre of the platform u want to rotate.";

    
    
    [Header("Settings")]
    [Tooltip("Rotation in degrees (e.g., 90)")]
    [SerializeField] private float rotationAmount = 90f;
    [SerializeField] private float speed = 100f;
    [Tooltip("Reference to the actual visual trap child object that will rotate.")]
    [SerializeField] private Transform parentRotationPivot; 

    private Quaternion initialLocalRotation;
    private Quaternion targetLocalRotation;
    private bool isTriggered = false;

    private void Awake()
    {
        if (parentRotationPivot == null)
        {
            Debug.LogError("Rotating Child not assigned! Please assign the actual trap GameObject to 'Rotating Child' in the Inspector.", this);
            enabled = false; // Disable script if no child is assigned
            return;
        }

        initialLocalRotation = parentRotationPivot.localRotation;
        // Calculate the target local rotation relative to the parent
        targetLocalRotation = Quaternion.Euler(0, 0, initialLocalRotation.eulerAngles.z + rotationAmount);
    }

    private void Update()
    {
        if (isTriggered)
        {
            parentRotationPivot.localRotation = Quaternion.RotateTowards(parentRotationPivot.localRotation, targetLocalRotation, speed * Time.deltaTime);
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