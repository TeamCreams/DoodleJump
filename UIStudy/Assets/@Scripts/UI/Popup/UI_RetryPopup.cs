﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_RetryPopup : UI_Popup
{

    enum Texts
    {
        LifeTime_Text,
    }

    enum GameObjects
    {
        LifeRecord,
    }


    enum Buttons
    {
        Retry_Button,
    }


    protected override void Init()
    {
        base.Init();
        BindTexts(typeof(Texts));
        BindButtons(typeof(Buttons));
        BindObjects(typeof(GameObjects));
        {
            int minutes = Mathf.FloorToInt(Managers.Game.TimeRecord / 60);
            float seconds = Managers.Game.TimeRecord % 60;
            GetText((int)Texts.LifeTime_Text).text = $"생존 시간 : {minutes}분 {seconds}초";
        }

        GetButton((int)Buttons.Retry_Button).gameObject.BindEvent((evt) =>
        {
            Managers.Scene.LoadScene(Define.EScene.SuberunkerScene);
            Managers.UI.ClosePopupUI(this);
            Time.timeScale = 1;
        }, Define.EUIEvent.Click);

        Time.timeScale = 0f;
    }


    public override void SetOrder(int sortOrder)
    {
        base.SetOrder(sortOrder);
    }
}