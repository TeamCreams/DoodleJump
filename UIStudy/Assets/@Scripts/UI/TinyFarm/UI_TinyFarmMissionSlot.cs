using Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_TinyFarmMissionSlot : UI_Base
{
    private TinyFarmData _data;
    public TinyFarmData Data
	{
        get => _data;
        private set
		{
            _data = value;
            RefreshUI();
		}
	}
    enum GameObjects
    {
        Event
    }

    enum Texts
    {
        Title_Text,
        TextInBox, // details
        Ex_Text,
        Gold_Text
    }

    public override bool Init()
    {
        if (base.Init() == false)
        {
            return false;
        }
        BindObjects(typeof(GameObjects));
        BindTexts(typeof(Texts));
        return true;
    }

    public void SetInfo(TinyFarmData data)
    {
        Data = data;
    }

    public void RefreshUI()
    {
        GetText((int)Texts.Title_Text).text = Data.EventName;
        GetText((int)Texts.TextInBox).text = Data.EventDetails;
        GetText((int)Texts.Ex_Text).text = Data.Compensation1.ToString();
        GetText((int)Texts.Gold_Text).text = Data.Compensation2.ToString();
        if (Data.Event == 0)
        {
            GetObject((int)GameObjects.Event).SetActive(false);
        }
    }
}
