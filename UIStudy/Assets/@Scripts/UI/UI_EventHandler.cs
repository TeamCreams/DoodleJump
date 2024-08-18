using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System;

public class UI_EventHandler : MonoBehaviour, IPointerDownHandler, IPointerClickHandler
{
    public Action<PointerEventData> OnPointerDownHandler;
    public Action<PointerEventData> OnClickHandler;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (OnClickHandler != null)
        {
            OnClickHandler.Invoke(eventData);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if(OnPointerDownHandler != null)
        {
            OnPointerDownHandler.Invoke(eventData);
        }
    }
}

