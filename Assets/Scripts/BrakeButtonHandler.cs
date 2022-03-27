using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Handles the Brake button press
/// </summary>
public class BrakeButtonHandler : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
{
    public void OnPointerDown(PointerEventData eventData)
    {
        GameManager.gameManagerPInstance.carController.SetAccelerationInput(-1f);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        GameManager.gameManagerPInstance.carController.SetAccelerationInput(0f);
    }
}
