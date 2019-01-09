using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
public class CustomEvent : MonoBehaviour, IPointerClickHandler,IPointerEnterHandler
{

    public UnityEvent MyClickEvent;
    public UnityEvent MyPointerEnterEvent;
    public void OnPointerClick(PointerEventData eventData)
    {
        if (MyClickEvent != null)
            MyClickEvent.Invoke();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (MyPointerEnterEvent != null)
            MyPointerEnterEvent.Invoke();
    }
}
