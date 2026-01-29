using UnityEngine;

public class MovingTrap : MonoBehaviour
{
    [Header("Movement Config")]
    [SerializeField] private Vector3 moveOffset = new Vector3(3, 0, 0); // Distance to move
    [SerializeField] private float speed = 2f;
    
    private Vector3 startPos;

    private void Start()
    {
        startPos = transform.position;
    }

    private void Update()
    {
        // PingPong creates a value that bounces between 0 and 1
        float t = Mathf.PingPong(Time.time * speed, 1f);
        
        // Lerp smoothly interpolates between start and end
        transform.position = Vector3.Lerp(startPos, startPos + moveOffset, t);
    }
}