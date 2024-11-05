using System.Collections;
using UnityEngine;
using static Define;

public class UI_ToastPopup : UI_Popup
{

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
