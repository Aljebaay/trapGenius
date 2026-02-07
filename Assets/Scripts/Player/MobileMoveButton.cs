using UnityEngine;
using UnityEngine.EventSystems;

public class MobileMoveButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
{
    [Header("Settings")]
    [Tooltip("-1 for Left, 1 for Right")]
    [SerializeField] private float direction = 1f;

    [Header("Dependencies")]
    [SerializeField] private InputHandler inputHandler;

    // Player touches the button
    public void OnPointerDown(PointerEventData eventData)
    {
        inputHandler.SetMoveDirection(direction);
    }

    // Player lifts finger
    public void OnPointerUp(PointerEventData eventData)
    {
        StopMovement();
    }

    // Player slides finger off the button (Safety Feature)
    public void OnPointerExit(PointerEventData eventData)
    {
        StopMovement();
    }

    private void StopMovement()
    {
        // Only stop if this specific button was the one driving the input
        // (Prevents conflict if you slide from Left button directly to Right button)
        inputHandler.SetMoveDirection(0f);
    }
}