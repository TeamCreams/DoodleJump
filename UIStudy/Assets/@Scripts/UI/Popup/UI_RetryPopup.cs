using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
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

    private int _failCount = 0;

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
        OnEvent_SetLanguage(null, null);
        SetRecord();
        _failCount = 0;        
        GetButton((int)Buttons.Retry_Button).gameObject.BindEvent(OnClick_RetryButton, EUIEvent.Click);
        GetButton((int)Buttons.Home_Button).gameObject.BindEvent(OnClick_HomeButton, EUIEvent.Click);

        Time.timeScale = 0f;
        return true;
    }
    private void OnDestroy()
    {
        Managers.Event.RemoveEvent(EEventType.SetLanguage, OnEvent_SetLanguage);
    }
    private void OnClick_RetryButton(PointerEventData eventData)
    {        
        Managers.Scene.LoadScene(EScene.SuberunkerScene);
        Managers.UI.ClosePopupUI(this);
        Time.timeScale = 1;
    }
    private void OnClick_HomeButton(PointerEventData eventData)
    {
        // playerDead event 
        // Managers.Resource.Instantiate("UI_Loading", this.transform);
        // Managers.Event.TriggerEvent(EEventType.StartLoading);
        Managers.Score.GetScore((this), ProcessErrorFun,
            () =>
                {
                    //Managers.Event.TriggerEvent(EEventType.StopLoading);
                    Managers.UI.ClosePopupUI(this);
                    Time.timeScale = 1;
                    Managers.Scene.LoadScene(EScene.SuberunkerSceneHomeScene);
                },
            ()=>
            {
                // Managers.Event.TriggerEvent(EEventType.StopLoading);
                if(_failCount < HardCoding.MAX_FAIL_COUNT)
                {
                    Time.timeScale = 1;
                    _failCount++;
                    return;
                }
                Time.timeScale = 1;
                _failCount = 0;
                Managers.UI.ClosePopupUI(this);
                Managers.Scene.LoadScene(EScene.SuberunkerSceneHomeScene);
            }
        );
    }

    public override void SetOrder(int sortOrder)
    {
        base.SetOrder(sortOrder);
    }

    public void SetRecord()
    {        
        // Managers.Resource.Instantiate("UI_Loading", this.transform);
        // Managers.Event.TriggerEvent(EEventType.StartLoading);
        Managers.Score.GetScore(this, ProcessErrorFun, null,
        ()=> // 실패했을경우 
        {
            // Managers.Event.TriggerEvent(EEventType.StopLoading);
            if(_failCount < HardCoding.MAX_FAIL_COUNT)
            {
                _failCount++;
                return;
            }
            _failCount = 0;
            Managers.Scene.LoadScene(EScene.SignInScene);
        }
        );
        // Managers.Event.TriggerEvent(EEventType.StopLoading);
        // int recordMinutes = Managers.Game.UserInfo.RecordScore / 60;
        // float recordSeconds = Managers.Game.UserInfo.RecordScore % 60;
        // GetText((int)Texts.LifeRecordTime_Text).text = $"{_bestRecord} : {recordMinutes}{_minutes} {recordSeconds}{_seconds}";
        GetText((int)Texts.LifeRecordTime_Text).text = $"{_bestRecord} : {Managers.Game.UserInfo.RecordScore:N0}";

        // int minutes = Managers.Game.UserInfo.LatelyScore / 60;
        // float seconds = Managers.Game.UserInfo.LatelyScore % 60;
        // GetText((int)Texts.LifeTime_Text).text = $"{_recentRecord} : {minutes}{_minutes} {seconds}{_seconds}";
        GetText((int)Texts.LifeTime_Text).text = $"{_recentRecord} : {Managers.Game.UserInfo.LatelyScore:N0}";


        GetText((int)Texts.Gold_Text).text = Managers.Game.Gold.ToString();
        Managers.Game.Gold = 0;
    }

    public void ProcessErrorFun()
    {
        Managers.Score.GetScore(this);
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
