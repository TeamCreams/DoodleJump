using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI_Joystick : UI_Base
{
    enum Images
    {
        Ground,
        Lever
    }

    //[SerializeField, Range(10f, 150f)]
    private float leverRange = 65f;

    private RectTransform _rectTransform = null;
    
    protected override void Init()
    {
        base.Init();
        BindImages(typeof(Images));
        _rectTransform = GetImage((int)Images.Ground).rectTransform;

        this.Get<Image>((int)Images.Lever).gameObject.BindEvent((evt) =>
        {
            Debug.Log("BeginDrag");
            JoystickMove();
        }, Define.EUIEvent.BeginDrag);

        this.Get<Image>((int)Images.Lever).gameObject.BindEvent((evt) =>
        {
            //Debug.Log("Drag");
            JoystickMove();
        }, Define.EUIEvent.Drag);

        this.Get<Image>((int)Images.Lever).gameObject.BindEvent((evt) =>
        {
            Debug.Log("EndDrag");
            GetImage((int)Images.Lever).rectTransform.anchoredPosition = Vector2.zero;
            Managers.Game.Amount = Vector2.zero;
        }, Define.EUIEvent.EndDrag);
    }

    public void JoystickMove()
    {
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle
     (_rectTransform, Input.mousePosition, null, out Vector2 localPoint))
        {
            var clampedDir = localPoint.magnitude < leverRange ?
           localPoint : localPoint.normalized * leverRange;

            GetImage((int)Images.Lever).rectTransform.anchoredPosition = clampedDir;

            Managers.Game.Amount = localPoint.normalized;
            //Debug.Log(Managers.Game.Amount);
        }
    }

}
