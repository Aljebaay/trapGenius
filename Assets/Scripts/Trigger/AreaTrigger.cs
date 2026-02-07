using UnityEngine;
using UnityEngine.Events; // Allows drag & drop linking in Inspector

public class AreaTrigger : MonoBehaviour
{
    [Header("What happens when Player enters?")]
    public UnityEvent onTriggerEnter;

    private bool hasTriggered = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!hasTriggered && collision.CompareTag("Player"))
        {
            hasTriggered = true;
            onTriggerEnter.Invoke();
        }
    }
    
    // Draw a green box in Editor so you can see it
    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(0, 1, 0, 0.3f);
        Gizmos.DrawCube(transform.position, transform.localScale);
    }
}