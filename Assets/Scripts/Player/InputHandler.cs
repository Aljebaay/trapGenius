using UnityEngine;

public class InputHandler : MonoBehaviour
{
    [SerializeField] private PlayerController controller;

    // --- Movement Logic ---
    // Called by UI Left/Right buttons
    public void SetMoveDirection(float direction) 
    {
        // direction: -1 (Left), 0 (Stop), 1 (Right)
        controller.SetInput(new Vector2(direction, 0));
    }

    // --- Jump Logic ---
    // Called by UI Jump button
    public void OnJumpPressed()
    {
        controller.OnJumpPressed();
    }

    public void OnJumpReleased()
    {
        controller.OnJumpReleased();
    }
}