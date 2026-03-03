using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class ArrowProjectile : MonoBehaviour
{
    [Header("Sprite Settings")]
    [Tooltip("Adjust this if your sprite faces Up (90) or Left (180) by default.")]
    [SerializeField] private float rotationOffset = 0f;

    private float speed;
    private Vector3 direction;
    private float lifeTime = 5f; 
    private bool killPlayer = true;
    private List<PlayerMutation> mutationsToApply;

    public void Initialize(Vector3 dir, float spd, bool kill, List<PlayerMutation> mutations)
    {
        direction = dir.normalized;
        speed = spd;
        killPlayer = kill;
        mutationsToApply = mutations;

        // 1. Calculate the angle in degrees
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        
        // 2. Apply rotation with your custom offset
        transform.rotation = Quaternion.Euler(0, 0, angle + rotationOffset);

        Destroy(gameObject, lifeTime);
    }

    private void Update()
    {
        // Move in the direction calculated (World Space)
        transform.position += direction * speed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Ignore self and trap
        if (collision.gameObject.GetComponent<ArrowTrap>() != null || 
            collision.gameObject.GetComponent<ArrowProjectile>() != null) 
        {
            return;
        }

        if (collision.CompareTag("Player"))
        {
            PlayerController player = collision.GetComponent<PlayerController>();

            // Apply Mutations
            if (mutationsToApply != null && mutationsToApply.Count > 0 && player != null)
            {
                player.ApplyMutations(mutationsToApply);
            }

            // Kill Logic
            if (killPlayer)
            {
                collision.gameObject.SetActive(false);
                if (GameManager.Instance != null) GameManager.Instance.GameOver();
            }

            Destroy(gameObject); 
        }
        else if (!collision.isTrigger) 
        {
            // Destroy on walls
            Destroy(gameObject);
        }
    }
}