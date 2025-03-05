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

    private enum Texts
    {
        Notice_Text
    }

    public override bool Init()
    {
        if (base.Init() == false)
        {
            return false;
        }
        BindTexts(typeof(Texts));
        Managers.Event.AddEvent(EEventType.ToastPopupNotice, OnEvent_Notice);

        return true;
    }

    private void OnDestroy()
    {
        Managers.Event.RemoveEvent(EEventType.ToastPopupNotice, OnEvent_Notice);
    }
    void OnEvent_Notice(Component sender, object param)
    {
        string str = param as string;
        StartCoroutine(ToastPopup_Co(str));
    }

    public IEnumerator ToastPopup_Co(string notice)
    {
        GetText((int)Texts.Notice_Text).text = notice;
        yield return new WaitForSeconds(2);
        Managers.UI.ClosePopupUI(this);
    }
}
