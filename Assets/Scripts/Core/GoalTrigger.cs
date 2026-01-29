using UnityEngine;

public class GoalTrigger : MonoBehaviour
{
    private bool activated = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Prevent double triggers
        if (activated) return;

        if (collision.CompareTag("Player"))
        {
            activated = true;
            
            // 1. Get the controller and freeze it
            PlayerController player = collision.GetComponent<PlayerController>();
            if (player != null)
            {
                player.StopMovement();
            }

            // 2. Tell GameManager to handle the level flow (wait 1s -> next level)
            if (GameManager.Instance != null)
            {
                GameManager.Instance.LevelComplete();
            }
        }
    }
}