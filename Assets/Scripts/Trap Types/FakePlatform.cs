using UnityEngine;
using UnityEngine.Tilemaps;

public class FakePlatform : MonoBehaviour
{
    [SerializeField] private float fadeSpeed = 5f;
    private Tilemap tmr;
    private bool isTriggered = false;

    private void Awake()
    {
        tmr = GetComponent<Tilemap>();
        GetComponent<CompositeCollider2D>().isTrigger = true; 
    }

    // --- NEW PUBLIC METHOD ---
    public void Activate()
    {
        isTriggered = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Activate();
        }
    }

    private void Update()
    {
        if (isTriggered && tmr.color.a > 0)
        {
            Color c = tmr.color;
            c.a = Mathf.MoveTowards(c.a, 0f, Time.deltaTime * fadeSpeed);
            tmr.color = c;
        }
    }
}