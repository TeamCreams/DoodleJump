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
    
    public void SetInfo(string notice, Type type = Type.Info, float time = 2f, Action action = null)
    {
        _notice = notice;
        _type = type;
        _time = time;
        SetBackgroundColor();
        StartCoroutine(ToastPopup_Co(action));
    }
    private void SetBackgroundColor()
    {
        string name = "";
        switch(_type)
        {
            case Type.Info:
                name = "ToastMessage_Topbar_Info";
            break;
            case Type.Warning:
                name = "ToastMessage_Topbar_Info";
            break;
            case Type.Error:
                name = "ToastMessage_Topbar_Error";
            break;
            case Type.Critical:
                name = "ToastMessage_Topbar_Error";
            break;
        }
        GetImage((int)Images.Background_Image).sprite = Managers.Resource.Load<Sprite>($"{name}.sprite");
    }

    private IEnumerator ToastPopup_Co(Action action = null)
    {
        GetText((int)Texts.Notice_Text).text = _notice;
        yield return new WaitForSeconds(_time);
        Managers.UI.ClosePopupUI(this);
        action?.Invoke();
    }
}
