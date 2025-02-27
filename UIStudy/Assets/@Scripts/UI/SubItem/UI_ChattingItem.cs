using UnityEngine;

public class UI_ChattingItem : UI_Base
{
    public enum Texts
    {
        Chatting_Text
    }
    private string _nickName = "";
    private string _message = "";

    public override bool Init()
    {
        if (base.Init() == false)
        {
            return false;
        }
        BindTexts(typeof(Texts));

        return true;
    }
    public void SetInfo(ChattingStruct chattingData)
    {
        Debug.Log($"WOWOW");
        _nickName = Managers.Game.ChattingInfo.SenderNickname;
        _message = chattingData.Message;

        if(chattingData.IsPrivateMessage)
        {
            GetText((int)Texts.Chatting_Text).color = Color.blue;
        }
        else
        {
            GetText((int)Texts.Chatting_Text).color = Color.magenta;
        }
        GetText((int)Texts.Chatting_Text).text = $"[{_nickName}] : {_message}";
    }
}
