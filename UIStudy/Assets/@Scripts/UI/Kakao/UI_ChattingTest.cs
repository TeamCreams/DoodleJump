using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_ChattingTest : UI_Base
{
    public enum Texts
    {
        TextInBox
    }

    public override bool Init()
    {
        if (base.Init() == false)
        {
            return false;
        }
        BindTexts(typeof(Texts));
        return true;
    }

    public void SetText(string text)
    {
        GetText((int)Texts.TextInBox).text = text;
    }
}