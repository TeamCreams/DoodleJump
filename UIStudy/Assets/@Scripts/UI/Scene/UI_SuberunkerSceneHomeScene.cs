using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;
using static UI_InputNicknameScene;
using UnityEngine.EventSystems;
using UniRx;

public class UI_SuberunkerSceneHomeScene : UI_Scene
{

    private enum GameObjects
    {
        Ranking,
        MyScore,
        UI_MissionPanel
    }

    private enum Texts
    {
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
        Start_Button,
        Setting_Button
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

    private string _welcome = "환영합니다";
    System.IDisposable _rechargeTimer;

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

        GetButton((int)Buttons.Start_Button).gameObject.BindEvent(OnClick_StartGame, EUIEvent.Click);
        GetImage((int)Images.MyScore_Button).gameObject.BindEvent(OnClick_ShowMyScore, EUIEvent.Click);
        GetImage((int)Images.Ranking_Button).gameObject.BindEvent(OnClick_ShowRanking, EUIEvent.Click);
        GetButton((int)Buttons.Mission_Button).gameObject.BindEvent(OnClick_ShowMission, EUIEvent.Click);
        GetButton((int)Buttons.Setting_Button).gameObject.BindEvent(OnClick_SettingButton, EUIEvent.Click);

        GetObject((int)GameObjects.UI_MissionPanel).SetActive(false);
        GetText((int)Texts.TotalGold_Text).text = Managers.Game.UserInfo.Gold.ToString();
        Managers.Event.AddEvent(EEventType.SetLanguage, OnEvent_SetLanguage);
        OnEvent_SetLanguage(null, null);
        Managers.Event.AddEvent(EEventType.UIGoldRefresh, OnEvent_RefreshGold);

        ShowRanking();

        return true;
    }
    private void OnDestroy()
    {
        Managers.Event.RemoveEvent(EEventType.SetLanguage, OnEvent_SetLanguage);
        Managers.Event.RemoveEvent(EEventType.UIGoldRefresh, OnEvent_RefreshGold);
    }

    public void ShowMyScore()
    {
        GetObject((int)GameObjects.MyScore).SetActive(false);
        GetObject((int)GameObjects.Ranking).SetActive(true);
    }

    public void ShowRanking()
    {
        GetObject((int)GameObjects.MyScore).SetActive(true);
        GetObject((int)GameObjects.Ranking).SetActive(false);
    }

    private void OnClick_ShowMyScore(PointerEventData eventData)
    {
        Managers.Event.TriggerEvent(EEventType.GetMyScore);
        //GetText((int)Texts.TotalGold_Text).text = Managers.Game.UserInfo.Gold.ToString();
        GetObject((int)GameObjects.MyScore).SetActive(true);
        GetObject((int)GameObjects.Ranking).SetActive(false);
    }

    private void OnClick_ShowRanking(PointerEventData eventData)
    {
        Managers.Event.TriggerEvent(EEventType.GetUserScoreList);
        //GetText((int)Texts.TotalGold_Text).text = Managers.Game.UserInfo.Gold.ToString();
        GetObject((int)GameObjects.MyScore).SetActive(false);
        GetObject((int)GameObjects.Ranking).SetActive(true);
    }

    private void OnClick_ShowMission(PointerEventData eventData)
    {
        GetObject((int)GameObjects.UI_MissionPanel).SetActive(true);
        Managers.Event.TriggerEvent(EEventType.Mission);
    }

    private void OnClick_SettingButton(PointerEventData eventData)
    {
        UI_SettingPopup settingPopup = Managers.UI.ShowPopupUI<UI_SettingPopup>();
        settingPopup.ActiveInfo();
    }

    private void OnEvent_RefreshGold(Component sender, object param)
    {
        GetText((int)Texts.TotalGold_Text).text = Managers.Game.UserInfo.Gold.ToString();
    }
    private void OnClick_StartGame(PointerEventData eventData)
    {
        /*
        if(Managers.Game.Energy < 1)
        {
            // 에너지 없다고 문구 띄우기
            return;
        }
        Managers.Game.Energy--;
        
        //이걸 하는 함수를 따로 만들어야함.
        _rechargeTimer = Observable.Interval(new System.TimeSpan(0, 0, 1))
            .Subscribe(_ =>
            {   
                // 시간 변수인 Managers.Game.RechargeTimer ++
                // 5분에 한 번씩 Managers.Game.Energy ++ 
                // 10/10으로 충전이 전부 되어있으면 멈춤.
                // PlayerPrefs에 저장
                // HardCoding.Energy
            }).AddTo(this.gameObject);
            */
        Managers.Scene.LoadScene(EScene.SuberunkerTimelineScene);
    }
    void OnEvent_SetLanguage(Component sender, object param)
    {
        GetText((int)Texts.Shop_Text).text = Managers.Language.LocalizedString(91006);
        GetText((int)Texts.Mission_Text).text = Managers.Language.LocalizedString(91007);
        GetText((int)Texts.ChooseCharacter_Text).text = Managers.Language.LocalizedString(91008);
        GetText((int)Texts.Start_Text).text = Managers.Language.LocalizedString(91009);
        _welcome = Managers.Language.LocalizedString(91028);
        GetText((int)Texts.Welcome_Text).text = $"{_welcome}, {Managers.Game.UserInfo.UserNickname}!!";
    }

}
