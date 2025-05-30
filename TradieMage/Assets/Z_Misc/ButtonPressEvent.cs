using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class ButtonPressEvent : MonoBehaviour, IPointerDownHandler
{

    [SerializeField] private UnityEvent _buttonPressed;
    

    public void OnPointerDown(PointerEventData eventData)
    {

        Debug.Log(eventData);
        _buttonPressed?.Invoke();

    }


}
