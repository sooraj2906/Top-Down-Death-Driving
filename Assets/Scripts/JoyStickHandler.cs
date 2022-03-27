using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// This class handles the joystick input in the UI
/// </summary>
public class JoyStickHandler : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
    [SerializeField] private Image joystickContainer;
    [SerializeField] private Image joystick;

    private Vector3 direction;

    //This method checks the position of the joystick with respect to the container in which the joystick is in and
    //returns the direction in which the joystick is moved. We'll be using only the horizontal values of the joystick
    //as we are only using the joystick to steer the car
    public void OnDrag(PointerEventData eventData)
    {
        var position = Vector2.zero;

        RectTransformUtility.ScreenPointToLocalPointInRectangle
            (joystickContainer.rectTransform, eventData.position, eventData.pressEventCamera, out position);

        var sizeDelta = joystickContainer.rectTransform.sizeDelta;
        position.x = (position.x / sizeDelta.x);
        position.y = (position.y / sizeDelta.y);

        var x = (joystickContainer.rectTransform.pivot.x == 1f) ? position.x * 2 + 1 : position.x * 2 - 1;
        var y = (joystickContainer.rectTransform.pivot.y == 1f) ? position.y * 2 + 1 : position.y * 2 - 1;

        direction = new Vector3(x, y, 0);
        direction = (direction.magnitude > 1) ? direction.normalized : direction;

        joystick.rectTransform.anchoredPosition = new Vector3(
            direction.x * (joystickContainer.rectTransform.sizeDelta.x / 3)
            , direction.y * (joystickContainer.rectTransform.sizeDelta.y) / 3);
    }

    //When joystick is moved pass the data to the OnDrag method
    public void OnPointerDown(PointerEventData eventData)
    {
        OnDrag(eventData);
    }

    //When joystick stops moving set the value to zero
    public void OnPointerUp(PointerEventData eventData)
    {
        direction = Vector3.zero;
        joystick.rectTransform.anchoredPosition = Vector3.zero;
    }

    //Returns the direction in which the joystick is moving
    public Vector3 GetDirectionVector()
    {
        return direction;
    }
}