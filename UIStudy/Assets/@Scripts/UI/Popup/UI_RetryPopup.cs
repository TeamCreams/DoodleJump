using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_RetryPopup : UI_Popup
{

    enum Texts
    {
        LifeTime_Text,
        LifeRecordTime_Text
    }

    enum GameObjects
    {
        LifeRecord,
    }


    enum Buttons
    {
        Retry_Button,
        Home_Button
    }


    public override bool Init()
    {
        if (base.Init() == false)
        {
            return false;
        }
        BindTexts(typeof(Texts));
        BindButtons(typeof(Buttons));
        BindObjects(typeof(GameObjects));
        {
            int recordMinutes = Mathf.FloorToInt(Managers.Game.PlayTimeRecord / 60);
            float recordSeconds = Managers.Game.PlayTimeRecord % 60;
            GetText((int)Texts.LifeRecordTime_Text).text = $"최고 생존 시간 : {recordMinutes}분 {recordSeconds}초";

            int minutes = Mathf.FloorToInt(Managers.Game.PlayTime / 60);
            float seconds = Managers.Game.PlayTime % 60;
            GetText((int)Texts.LifeTime_Text).text = $"생존 시간 : {minutes}분 {seconds}초";
        }

        GetButton((int)Buttons.Retry_Button).gameObject.BindEvent((evt) =>
        {
            Managers.Scene.LoadScene(Define.EScene.SuberunkerScene);
            Managers.UI.ClosePopupUI(this);
            Time.timeScale = 1;
        }, Define.EUIEvent.Click);

        GetButton((int)Buttons.Home_Button).gameObject.BindEvent((evt) =>
        {
            Managers.Scene.LoadScene(Define.EScene.SuberunkerSceneHomeScene);
            Managers.UI.ClosePopupUI(this);
            Time.timeScale = 1;
        }, Define.EUIEvent.Click);

        Time.timeScale = 0f;
        return true;
    }


    public override void SetOrder(int sortOrder)
    {
        base.SetOrder(sortOrder);
    }
}
