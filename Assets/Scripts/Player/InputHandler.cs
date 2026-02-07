using UnityEngine;

public class InputHandler : MonoBehaviour
{
    [SerializeField] private PlayerController controller;

    // Internal state for Mobile Buttons
    private float mobileHorizontalInput = 0f;

    private void Update()
    {
        // --- 1. HORIZONTAL MOVEMENT ---
        
        // Get Keyboard Input (Returns -1, 0, or 1)
        float xInput = Input.GetAxisRaw("Horizontal");

        // Logic: If Keyboard is NOT being used, check Mobile Input
        // This prevents the keyboard (sending 0) from overriding the touch buttons
        if (Mathf.Abs(xInput) < 0.01f)
        {
            xInput = mobileHorizontalInput;
        }

        // Send final calculated input to the controller
        controller.SetInput(new Vector2(xInput, 0));


        // --- 2. JUMPING ---

        // Keyboard Jump (Spacebar)
        if (Input.GetButtonDown("Jump"))
        {
            controller.OnJumpPressed();
        }
        if (Input.GetButtonUp("Jump"))
        {
            controller.OnJumpReleased();
        }
    }

    // --- PUBLIC METHODS (Called by Mobile UI Buttons) ---

    public void SetMoveDirection(float direction) 
    {
        mobileHorizontalInput = direction;
    }

    public void OnJumpPressed()
    {
        controller.OnJumpPressed();
    }

    public void OnJumpReleased()
    {
        controller.OnJumpReleased();
    }
}