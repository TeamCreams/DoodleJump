using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Define;

public class UI_RetryPopup : UI_Popup
{
    enum Texts
    {
        LifeTime_Text,
        LifeRecordTime_Text,
        Gold_Text,
        Retry_Text,
        Home_Text
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
        int recordMinutes = Mathf.FloorToInt(Managers.Game.PlayTimeRecord / 60);
        float recordSeconds = Managers.Game.PlayTimeRecord % 60;
        GetText((int)Texts.LifeRecordTime_Text).text = $"{_bestRecord} : {recordMinutes}{_minutes} {recordSeconds}{_seconds}";

        int minutes = Mathf.FloorToInt(Managers.Game.PlayTime / 60);
        float seconds = Managers.Game.PlayTime % 60;
        GetText((int)Texts.LifeTime_Text).text = $"{_recentRecord} : {minutes}{_minutes} {seconds}{_seconds}";

        GetText((int)Texts.Gold_Text).text = Managers.Game.Gold.ToString();
    }

    void OnEvent_SetLanguage(Component sender, object param)
    {
        _minutes = this.LocalizedString(ELocalizableTerms.Minutes);
        _seconds = this.LocalizedString(ELocalizableTerms.Seconds);
        _bestRecord = this.LocalizedString(ELocalizableTerms.BestRecord);
        _recentRecord = this.LocalizedString(ELocalizableTerms.RecentRecord);
        GetText((int)Texts.Home_Text).text = this.LocalizedString(ELocalizableTerms.Home);
        GetText((int)Texts.Retry_Text).text = this.LocalizedString(ELocalizableTerms.Restart);
    }

    public string LocalizedString(ELocalizableTerms eLocalizableTerm)
    {
        int stringId = 0;

        foreach (var gameLanguageData in Managers.Data.GameLanguageDataDic)
        {
            if (gameLanguageData.Value.LocalizableTerm == eLocalizableTerm)
            {
                stringId = gameLanguageData.Value.Id;
                break;
            }
        }

        var content = Managers.Data.GameLanguageDataDic[stringId];

        switch (Managers.Game.ELanguageInfo)
        {
            case ELanguage.Kr:
                return content.KrText;

            case ELanguage.En:
                return content.EnText;
        }

        return "";
    }
}
