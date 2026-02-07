using UnityEngine;
using UnityEngine.EventSystems;

public class MobileJumpButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
{
    [Header("Dependencies")]
    [SerializeField] private InputHandler inputHandler;

    public void OnPointerDown(PointerEventData eventData)
    {
        inputHandler.OnJumpPressed();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        inputHandler.OnJumpReleased();
    }

    // Safety: If finger slides off button, treat it as releasing the jump
    public void OnPointerExit(PointerEventData eventData)
    {
        inputHandler.OnJumpReleased();
    }
}