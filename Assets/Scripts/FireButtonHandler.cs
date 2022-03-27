using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Handles the input of the fire button
/// </summary>
public class FireButtonHandler : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
{
    private bool pressed;

    public void OnPointerDown(PointerEventData eventData)
    {
        foreach (var weapon in GameManager.gameManagerPInstance.playerWeapons)
        {
            weapon.isButtonPressed = true;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        foreach (var weapon in GameManager.gameManagerPInstance.playerWeapons)
        {
            weapon.isButtonPressed = false;
        }
    }
}