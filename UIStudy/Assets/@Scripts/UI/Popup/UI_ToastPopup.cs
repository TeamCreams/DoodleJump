using System;
using System.Collections;
using UnityEngine;
using static Define;

//SYSTEM : 곧 점검이 시작됩니다. (INFO)
//SYSTEM : 돈이 부족합니다. (WARNING)
//SYSTEM : 네트워크가 불안정합니다. (ERROR)

public class UI_ToastPopup : UI_Popup
{
    public enum Type
    {
        Info,
        Warning,
        Error,
        Critical
    }
    private enum Images
    {
        Background_Image,
    }
    private enum Texts
    {
        Notice_Text,
    }
    private string _notice;
    private Type _type;
    private float _time;
    public override bool Init()
    {
        if (base.Init() == false)
        {
            return false;
        }
        BindImages(typeof(Images));
        BindTexts(typeof(Texts));

        return true;
    }
    
    private void SetInfo(string notice, Type type = Type.Info, float time = 2f, Action action = null)
    {
        _notice = notice;
        _type = type;
        _time = time;
        SetBackgroundColor();
        StartCoroutine(ToastPopup_Co(action));
    }
    private void SetBackgroundColor()
    {
        Color color = new();
        switch(_type)
        {
            case Type.Info:
                color = ToastPopupColor.InfoWhite;
            break;
            case Type.Warning:
                color = ToastPopupColor.WarningYellow;
            break;
            case Type.Error:
                color = ToastPopupColor.ErrorGray;
            break;
            case Type.Critical:
                color = ToastPopupColor.CriticalRed;
            break;
        }
        GetImage((int)Images.Background_Image).color = color;
    }

    private IEnumerator ToastPopup_Co(Action action = null)
    {
        GetText((int)Texts.Notice_Text).text = _notice;
        yield return new WaitForSeconds(_time);
        Managers.UI.ClosePopupUI(this);
        action?.Invoke();
    }

    public static void ShowInfo(ErrorStruct errorStruct, float time = 2f, Action action = null)
    {
        UI_ToastPopup toast = Managers.UI.ShowPopupUI<UI_ToastPopup>();
        toast.SetInfo(errorStruct.Notice, UI_ToastPopup.Type.Info, time, action);
    }
    public static void ShowWarning(ErrorStruct errorStruct, float time = 2f, Action action = null)
    {
        UI_ToastPopup toast = Managers.UI.ShowPopupUI<UI_ToastPopup>();
        toast.SetInfo(errorStruct.Notice, UI_ToastPopup.Type.Warning, time, action);
    }
    public static void ShowError(ErrorStruct errorStruct, float time = 2f, Action action = null)
    {
        UI_ToastPopup toast = Managers.UI.ShowPopupUI<UI_ToastPopup>();
        toast.SetInfo(errorStruct.Notice, UI_ToastPopup.Type.Error, time, action);
    }
    public static void ShowCritical(ErrorStruct errorStruct, float time = 2f, Action action = null)
    {
        UI_ToastPopup toast = Managers.UI.ShowPopupUI<UI_ToastPopup>();
        toast.SetInfo(errorStruct.Notice, UI_ToastPopup.Type.Critical, time, action);
    }

    public class ToastPopupColor
    {
        public static readonly Color InfoWhite = new Color(255f / 255f, 246f / 255f, 225f / 255f, 1f);
        public static readonly Color WarningYellow = new Color(255f / 255f, 186f / 255f, 28f / 255f, 1f);
        public static readonly Color ErrorGray = new Color(159f / 255f, 159f / 255f, 159f / 255f, 1f);
        public static readonly Color CriticalRed = new Color(255f / 255f, 177f / 255f, 177f / 255f, 1f);
    }
}
