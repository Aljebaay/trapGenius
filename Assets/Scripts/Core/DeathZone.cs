using UnityEngine;

public class DeathZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Disable player visually
            collision.gameObject.SetActive(false);
            
            // Trigger Death
            GameManager.Instance.GameOver();
        }
        // Optional: Destroy falling rocks/traps so they don't fall forever
        else if (collision.GetComponent<Rigidbody2D>() != null)
        {
            Destroy(collision.gameObject);
        }
    }

    // Draw a Red box in Editor to see where the death zone is
    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 0, 0, 0.5f);
        Gizmos.DrawCube(transform.position, transform.localScale);
    }
}