using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Chatting : UI_Base
{
    public enum Texts
    {
        TextInBox
    }

    protected override void Init()
    {
        base.Init();
        BindTexts(typeof(Texts));

    }

    public void SetText(string text)
    {
        GetText((int)Texts.TextInBox).text = text;
    }
}