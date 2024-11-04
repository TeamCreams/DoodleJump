using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;
using static UI_InputNameScene;
using UnityEngine.EventSystems;

public class UI_SuberunkerSceneHomeScene : UI_Scene
{

    private enum GameObjects
    {
        Ranking,
        MyScore
    }

    private enum Texts
    {
        Best_Text,
        Current_Text,
        TotalGold_Text,
        Shop_Text,
        Mission_Text,
        ChooseCharacter_Text,
        Start_Text,
        Welcome_Text
    }

    private enum Buttons
    {
        Shop_Button,
        Mission_Button,
        ChooseCharacter_Button,
        Start_Button
    }

    private enum Images
    {
        MyScore_Button,
        Ranking_Button
    }

    private enum Toggles
    {
        Language_En,
        Language_Kr
    }

    private string _minutesString = "분";
    private string _secondsString = "초";
    private string _bestRecord = "최고 기록";
    private string _recentRecord = "최근 기록";
    private string _welcome = "환영합니다";
    private int _recordMinutes;
    private float _recordSeconds;
    private int _minutes;
    private float _seconds;


    public override bool Init()
    {
        if (base.Init() == false)
        {
            return false;
        }
        BindObjects(typeof(GameObjects));
        BindTexts(typeof(Texts));
        BindButtons(typeof(Buttons));
        BindImages(typeof(Images));
        //BindToggles(typeof(Toggles));

        GetButton((int)Buttons.ChooseCharacter_Button).gameObject.BindEvent((evt) =>
        {
            Managers.Scene.LoadScene(EScene.ChooseCharacterScene);
        }, EUIEvent.Click);

        GetButton((int)Buttons.Start_Button).gameObject.BindEvent((evt) =>
        {
            Managers.Scene.LoadScene(EScene.SuberunkerTimelineScene);
        }, EUIEvent.Click);

        GetObject((int)GameObjects.MyScore).SetActive(false);

        GetImage((int)Images.MyScore_Button).gameObject.BindEvent(OnClick_ShowMyScore, EUIEvent.Click);
        GetImage((int)Images.Ranking_Button).gameObject.BindEvent(OnClick_ShowRanking, EUIEvent.Click);
        //GetToggle((int)Toggles.Language_En).gameObject.BindEvent(OnClick_SetLanguage, EUIEvent.Click);
        //GetToggle((int)Toggles.Language_Kr).gameObject.BindEvent(OnClick_SetLanguage, EUIEvent.Click);

        GetText((int)Texts.TotalGold_Text).text = Managers.Game.TotalGold.ToString();
        Managers.Event.AddEvent(EEventType.SetLanguage, OnEvent_SetLanguage);
        Managers.Event.TriggerEvent(EEventType.SetLanguage);

        return true;
    }

    private void OnClick_ShowMyScore(PointerEventData eventData)
    {
        GetObject((int)GameObjects.MyScore).SetActive(true);
        GetObject((int)GameObjects.Ranking).SetActive(false);
        
        //Debug.Log(Managers.Game.PlayerInfo.PlayerId);
        int recordScore = Managers.Score.GetScore("Orange", EScoreType.RecordScore);
        _recordMinutes = recordScore / 60;
        _recordSeconds = recordScore % 60;
        GetText((int)Texts.Best_Text).text = $"{_bestRecord} : {_recordMinutes}{_minutesString} {_recordSeconds}{_secondsString}";

        int latelyScore = Managers.Score.GetScore("Orange", EScoreType.LatelyScore);
        _minutes = latelyScore / 60;
        _seconds = latelyScore % 60;
        GetText((int)Texts.Current_Text).text = $"{_recentRecord} : {_minutes}{_minutesString} {_seconds}{_secondsString}";
/*
        _recordMinutes = Managers.Game.PlayTimeRecord / 60;
        _recordSeconds = Managers.Game.PlayTimeRecord % 60;
        GetText((int)Texts.Best_Text).text = $"{_bestRecord} : {_recordMinutes}{_minutesString} {_recordSeconds}{_secondsString}";

        _minutes = Managers.Game.PlayTime / 60;
        _seconds = Managers.Game.PlayTime % 60;
        GetText((int)Texts.Current_Text).text = $"{_recentRecord} : {_minutes}{_minutesString} {_seconds}{_secondsString}";
  */
    }

    private void OnClick_ShowRanking(PointerEventData eventData)
    {
        GetObject((int)GameObjects.MyScore).SetActive(false);
        GetObject((int)GameObjects.Ranking).SetActive(true);
    }

    private void OnClick_SetLanguage(PointerEventData eventData)
    {
        if(GetToggle((int)Toggles.Language_En).isOn == true)
        {
            Managers.Language.ELanguageInfo = ELanguage.En;
        }
        else
        {
            Managers.Language.ELanguageInfo = ELanguage.Kr;
        }
        Managers.Event.TriggerEvent(EEventType.SetLanguage);
        Debug.Log($"language : {Managers.Language.ELanguageInfo}");
    }
    void OnEvent_SetLanguage(Component sender, object param)
    {
        _bestRecord = Managers.Language.LocalizedString(91001);
        _recentRecord = Managers.Language.LocalizedString(91002);
        _minutesString = Managers.Language.LocalizedString(91004);
        _secondsString = Managers.Language.LocalizedString(91005);
        GetText((int)Texts.Best_Text).text = $"{_bestRecord} : {_recordMinutes}{_minutesString} {_recordSeconds}{_secondsString}";
        GetText((int)Texts.Current_Text).text = $"{_recentRecord} : {_minutes}{_minutesString} {_seconds}{_secondsString}";

        GetText((int)Texts.Shop_Text).text = Managers.Language.LocalizedString(91006);
        GetText((int)Texts.Mission_Text).text = Managers.Language.LocalizedString(91007);
        GetText((int)Texts.ChooseCharacter_Text).text = Managers.Language.LocalizedString(91008);
        GetText((int)Texts.Start_Text).text = Managers.Language.LocalizedString(91009);
        _welcome = Managers.Language.LocalizedString(91028);
        GetText((int)Texts.Welcome_Text).text = $"{_welcome}, {"Orange"}!!";
    }

}
