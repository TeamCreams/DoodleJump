using Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_TinyFarmMissionSlot : UI_Base
{

    enum GameObjects
    {
        Event
    }

    enum Texts
    {
        Title_Text,
        ChattingTMP, // details
        Ex_Text,
        Gold_Text
    }

    protected override void Init()
    {
        base.Init();
        BindObjects(typeof(GameObjects));
        BindTexts(typeof(Texts));
    }

    public void SetInfo(TinyFarmData data)
    {
        GetText((int)Texts.Title_Text).text = data.EventName;
        GetText((int)Texts.ChattingTMP).text = data.EventDetails;
        GetText((int)Texts.Ex_Text).text = data.Compensation1.ToString();
        GetText((int)Texts.Gold_Text).text = data.Compensation2.ToString();

        if (data.Event == 0)
        {
            GetObject((int)GameObjects.Event).SetActive(false);
        }
    }
}
