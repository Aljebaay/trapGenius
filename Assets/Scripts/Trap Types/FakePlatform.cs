using UnityEngine;
using UnityEngine.Tilemaps;

public class FakePlatform : TrapBase
{
    [SerializeField] private float fadeSpeed = 5f;
    private Tilemap tmr;
    private bool isTriggered = false;

    private void Awake()
    {
        tmr = GetComponent<Tilemap>();
        GetComponent<CompositeCollider2D>().isTrigger = true; 
    }

    public override void Activate()
    {
        // RNG CHECK
        if (!ShouldActivate()) return;

        isTriggered = true;
        if (changesPlayerData) ApplyMutationsToPlayer();
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
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