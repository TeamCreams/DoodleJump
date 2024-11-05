using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Define;

public class UI_RetryPopup : UI_Popup
{
    private enum Texts
    {
        LifeTime_Text,
        LifeRecordTime_Text,
        Gold_Text,
        Retry_Text,
        Home_Text
    }

    private enum GameObjects
    {
        LifeRecord,
    }

    private enum Buttons
    {
        Retry_Button,
        Home_Button
    }

    private string _minutes = "분";
    private string _seconds = "초";
    private string _bestRecord = "최고 기록";
    private string _recentRecord = "최근 기록";

    public override bool Init()
    {
        if (base.Init() == false)
        {
            return false;
        }
        BindTexts(typeof(Texts));
        BindButtons(typeof(Buttons));
        BindObjects(typeof(GameObjects));
        Managers.Event.AddEvent(EEventType.SetLanguage, OnEvent_SetLanguage);
        Managers.Event.TriggerEvent(EEventType.SetLanguage);
        SetRecord();

        Managers.Game.TotalGold += Managers.Game.Gold;

        GetButton((int)Buttons.Retry_Button).gameObject.BindEvent((evt) =>
        {
            Managers.Scene.LoadScene(EScene.SuberunkerScene);
            Managers.UI.ClosePopupUI(this);
            Time.timeScale = 1;
        }, EUIEvent.Click);

        GetButton((int)Buttons.Home_Button).gameObject.BindEvent((evt) =>
        {
            Managers.Scene.LoadScene(EScene.SuberunkerSceneHomeScene);
            Managers.UI.ClosePopupUI(this);
            Time.timeScale = 1;
        }, EUIEvent.Click);

        Time.timeScale = 0f;
        return true;
    }



    public override void SetOrder(int sortOrder)
    {
        base.SetOrder(sortOrder);
    }

    public void SetRecord()
    {
        int recordScore = Managers.Score.GetScore(Managers.Game.PlayerInfo.PlayerId, EScoreType.RecordScore);
        int recordMinutes = recordScore / 60;
        float recordSeconds = recordScore % 60;//Managers.Game.PlayTimeRecord % 60;
        GetText((int)Texts.LifeRecordTime_Text).text = $"{_bestRecord} : {recordMinutes}{_minutes} {recordSeconds}{_seconds}";

        int latelyScore = Managers.Score.GetScore(Managers.Game.PlayerInfo.PlayerId, EScoreType.LatelyScore);

        int minutes = latelyScore / 60;
        float seconds = latelyScore % 60;
        GetText((int)Texts.LifeTime_Text).text = $"{_recentRecord} : {minutes}{_minutes} {seconds}{_seconds}";

        GetText((int)Texts.Gold_Text).text = Managers.Game.Gold.ToString();
    }

    void OnEvent_SetLanguage(Component sender, object param)
    {
        _bestRecord = Managers.Language.LocalizedString(91001);
        _recentRecord = Managers.Language.LocalizedString(91002);
        _minutes = Managers.Language.LocalizedString(91004);
        _seconds = Managers.Language.LocalizedString(91005);
        GetText((int)Texts.Home_Text).text = Managers.Language.LocalizedString(91017);
        GetText((int)Texts.Retry_Text).text = Managers.Language.LocalizedString(91018);
    }

}
