using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class UI_EventHandler : MonoBehaviour, IPointerDownHandler, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Action<PointerEventData> OnPointerDownHandler;
    public Action<PointerEventData> OnClickHandler;

    public Action<PointerEventData> OnBeginDragHandler;
    public Action<PointerEventData> OnDragHandler;
    public Action<PointerEventData> OnEndDragHandler;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (OnClickHandler != null)
        {
            AudioClip selectAudio = Managers.Resource.Load<AudioClip>("SelectSound");
            Managers.Sound.Play(Define.ESound.Effect, selectAudio, 0.7f);
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

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (OnBeginDragHandler != null)
        {
            OnBeginDragHandler.Invoke(eventData);
        }
    }
    public void OnDrag(PointerEventData eventData)
    {
        if (OnDragHandler != null)
        {
            OnDragHandler.Invoke(eventData);
        }
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        if (OnEndDragHandler != null)
        {
            OnEndDragHandler.Invoke(eventData);
        }
    }
}

