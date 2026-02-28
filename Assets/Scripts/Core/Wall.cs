using UnityEngine;

public class Wall : MonoBehaviour
{
    public enum WallState { Solid, Ghost, Open }

    [Header("Settings")]
    [SerializeField] private WallState initialState = WallState.Solid;
    
    [Range(0f, 1f)]
    [Tooltip("Opacity when in Ghost mode (0 = Invisible, 1 = Solid look)")]
    [SerializeField] private float ghostAlpha = 0.5f;

    private WallState currentState;
    private SpriteRenderer sr;
    
    // We store both types to handle both single sprites and composite tilemaps
    private Collider2D standardCol;
    private CompositeCollider2D compositeCol;
    
    private Color originalColor;

    private void Awake()
    {
        InitializeComponents();
        SetState(initialState);
    }

    private void OnValidate()
    {
        InitializeComponents();
        ApplyStateLogic(initialState); 
    }

    private void InitializeComponents()
    {
        // Try to find a renderer (SpriteRenderer for objects, TilemapRenderer for Tilemaps)
        if (sr == null) sr = GetComponent<SpriteRenderer>();
        if (sr == null) 
        {
            // If no SpriteRenderer, try TilemapRenderer (cast as Renderer to access basic material/color props if needed)
            // But usually we just want to control visibility/alpha. 
            // For simple alpha control on Tilemaps, we need the Tilemap component itself.
            var tr = GetComponent<UnityEngine.Tilemaps.TilemapRenderer>();
            if (tr != null) originalColor = Color.white; // Default for tilemaps if we can't easily get color
        }
        else
        {
             if (Application.isPlaying && originalColor == Color.clear) originalColor = sr.color;
        }

        // PRIORITY: Check for CompositeCollider2D first (for Tilemaps)
        compositeCol = GetComponent<CompositeCollider2D>();
        
        // If no composite, fallback to standard collider (Box, Circle, Capsule, etc.)
        if (compositeCol == null)
        {
            standardCol = GetComponent<Collider2D>();
        }
    }

    // --- PUBLIC FUNCTIONS ---

    public void BecomeSolid() => SetState(WallState.Solid);
    public void BecomeGhost() => SetState(WallState.Ghost);
    public void BecomeOpen() => SetState(WallState.Open);
    public void ToggleOpenSolid() => SetState(currentState == WallState.Open ? WallState.Solid : WallState.Open);

    // --- LOGIC ---

    private void SetState(WallState newState)
    {
        currentState = newState;
        ApplyStateLogic(currentState);
    }

    private void ApplyStateLogic(WallState state)
    {
        // 1. Handle Visuals
        HandleVisuals(state);

        // 2. Handle Physics
        // Note: For Open state, we disable the collider entirely.
        // For Ghost state, we enable it but set isTrigger = true.
        // For Solid state, we enable it and set isTrigger = false.

        bool shouldBeEnabled = (state != WallState.Open);
        bool shouldBeTrigger = (state == WallState.Ghost);

        if (compositeCol != null)
        {
            compositeCol.enabled = shouldBeEnabled;
            compositeCol.isTrigger = shouldBeTrigger;
        }
        else if (standardCol != null)
        {
            standardCol.enabled = shouldBeEnabled;
            standardCol.isTrigger = shouldBeTrigger;
        }
    }

    private void HandleVisuals(WallState state)
    {
        // Handle SpriteRenderer (Single Objects)
        if (sr != null)
        {
            if (state == WallState.Open)
            {
                sr.enabled = false;
            }
            else
            {
                sr.enabled = true;
                Color c = (Application.isPlaying) ? originalColor : sr.color;
                c.a = (state == WallState.Ghost) ? ghostAlpha : 1f;
                sr.color = c;
            }
        }
        // Handle Tilemap (Grid Objects)
        else
        {
            var tilemap = GetComponent<UnityEngine.Tilemaps.Tilemap>();
            if (tilemap != null)
            {
                // Toggle Renderer
                var tr = GetComponent<UnityEngine.Tilemaps.TilemapRenderer>();
                if (tr != null) tr.enabled = (state != WallState.Open);

                // Handle Alpha
                Color c = tilemap.color;
                c.a = (state == WallState.Ghost) ? ghostAlpha : 1f;
                tilemap.color = c;
            }
        }
    }
}