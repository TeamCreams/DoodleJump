using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;
using static UI_InputNicknameScene;
using UnityEngine.EventSystems;
using GameApi.Dtos;
using UniRx;
using System;
using System.Threading;
using System.Threading.Tasks;

public class UI_SuberunkerSceneHomeScene : UI_Scene
{

    private enum GameObjects
    {
        Ranking,
        MyScore,
    }

    private enum Texts
    {
        TotalGold_Text,
        Energy_Text,
        EnergyTimer_Text,
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

    private string _welcome = "환영합니다";
    // System.IDisposable _rechargeTimer;
    // private int _displayTime = 0;
    // private int _calculateTime = 0;
    // private DateTime _serverTime = new DateTime();
    // private DateTime _startTime = new DateTime();
    // private bool _isRunningTimer = false;
    // private bool _isSettingComplete = false;
    // private Coroutine _tickCo;

    public override bool Init()
    {
        if (base.Init() == false)
        {
            return false;
        }
        // Bind
        BindObjects(typeof(GameObjects));
        BindTexts(typeof(Texts));
        BindButtons(typeof(Buttons));
        BindImages(typeof(Images));

        // Bind Event
        GetButton((int)Buttons.ChooseCharacter_Button).gameObject.BindEvent(OnClick_ShowChooseCharacterScene, EUIEvent.Click);
        GetButton((int)Buttons.Start_Button).gameObject.BindEvent(OnClick_GameStart, EUIEvent.Click);
        GetImage((int)Images.MyScore_Button).gameObject.BindEvent(OnClick_ShowMyScore, EUIEvent.Click);
        GetImage((int)Images.Ranking_Button).gameObject.BindEvent(OnClick_ShowRanking, EUIEvent.Click);
        GetButton((int)Buttons.Mission_Button).gameObject.BindEvent(OnClick_ShowMission, EUIEvent.Click);
        //GetButton((int)Buttons.Setting_Button).gameObject.BindEvent(OnClick_SettingButton, EUIEvent.Click);
        GetButton((int)Buttons.Shop_Button).gameObject.BindEvent(OnClick_ShowShop, EUIEvent.Click);
        
        // add mission
        Managers.Event.AddEvent(EEventType.SetLanguage, OnEvent_SetLanguage);
        //Managers.Event.AddEvent(EEventType.UIRefresh, OnEvent_Refresh);

        // Default setting
        //_startTime = Managers.Game.UserInfo.LatelyEnergy;
        OnEvent_SetLanguage(null, null);
        //OnEvent_Refresh(null, null);
        ShowRanking();
        
        //Managers.SignalR.OnChangedHeartBeat -= CheckServerTime; // 구독 해제
        //Managers.SignalR.OnChangedHeartBeat += CheckServerTime; // 이벤트 구독
        return true;
    }
    private void OnDestroy()
    {
        Managers.Event.RemoveEvent(EEventType.SetLanguage, OnEvent_SetLanguage);
        //Managers.Event.RemoveEvent(EEventType.UIRefresh, OnEvent_Refresh);
        //Managers.SignalR.OnChangedHeartBeat -= CheckServerTime; // 구독 해제
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
    private void OnClick_ShowChooseCharacterScene(PointerEventData eventData)
    {
        Managers.Game.ChracterStyleInfo.ResetValuesFromTemp();
        Managers.Scene.LoadScene(EScene.ChooseCharacterScene);
    }
    private void OnClick_ShowMyScore(PointerEventData eventData)
    {
        GetObject((int)GameObjects.MyScore).SetActive(true);
        GetObject((int)GameObjects.Ranking).SetActive(false);
    }

    private void OnClick_ShowRanking(PointerEventData eventData)
    {
        GetObject((int)GameObjects.MyScore).SetActive(false);
        GetObject((int)GameObjects.Ranking).SetActive(true);
    }

    private void OnClick_ShowMission(PointerEventData eventData)
    {
        Managers.UI.ShowPopupUI<UI_MissionPanel>();
        //GetObject((int)GameObjects.UI_MissionPanel).SetActive(true);
        Managers.Event.TriggerEvent(EEventType.Mission);
    }

    private void OnClick_ShowShop(PointerEventData eventData)
    {
        Managers.UI.ShowPopupUI<UI_ShopPanel>();
        Managers.Event.TriggerEvent(EEventType.EnterShop);
    }
    

    // private void OnEvent_Refresh(Component sender, object param)
    // {
    //     GetText((int)Texts.Energy_Text).text = $"{Managers.Game.UserInfo.Energy} / 10";
    //     GetText((int)Texts.TotalGold_Text).text = Managers.Game.UserInfo.Gold.ToString();
    // }
    private void OnClick_GameStart(PointerEventData eventData)
    {
        var loadingComplete = UI_LoadingPopup.Show();
        Managers.WebContents.GameStart(new ReqDtoGameStart()
        {
            UserAccountId = Managers.Game.UserInfo.UserAccountId
        },
        (response) =>
        {
            loadingComplete.Value = true;
            Managers.Game.UserInfo.Energy = response.Energy;
            Managers.Game.UserInfo.LatelyEnergy = response.LatelyEnergy;

            // 게임 난이도 초기화
            Managers.Game.DifficultySettingsInfo.StageId = 70001;
            Managers.Game.DifficultySettingsInfo.StageLevel = 1; // 이 값을 미션 달성에 사용할 때가 있기 떄문에. 그런데 게임을 새로 시작하면 미션 진행도도 초기화가 되기 때문에 이걸 따로 저장해놔야함.
            Managers.Game.DifficultySettingsInfo.AddSpeed = 0;
            Managers.Game.DifficultySettingsInfo.ChallengeScale = 0;
            // 한 게임당 누적 돌 개수 초기화
            Managers.Game.DifficultySettingsInfo.StoneCount = 0;
            
            Managers.Event.TriggerEvent(EEventType.UIRefresh);
            Managers.Scene.LoadSceneWithProgress(EScene.SuberunkerTimelineScene);
       },
        (errorCode) =>
        {
            loadingComplete.Value = true;
            UI_ToastPopup.ShowError(Managers.Error.GetError(EErrorCode.ERR_EnergyInsufficient));
        }
        );
    }
    // private void EnergyTimer()
    // {
    //     // 5초마다 서버에서 시간을 받아옴.
    //     // 300초가 되면 updateEnergy 요청

    //     // 게임어플을 아예 꺼버렸을 때 laytelyTime을 저장할 방법은?
    //     if(10 <= Managers.Game.UserInfo.Energy)
    //     {
    //         _isRunningTimer = false;
    //         _isSettingComplete = false;
    //         if(_tickCo != null)
    //         {
    //             StopCoroutine(_tickCo);
    //             _tickCo = null;
    //             _rechargeTimer?.Dispose();
    //         }
    //         return;
    //     }

    //     if(_isRunningTimer)
    //     {
    //         return;
    //     }
    //     _isRunningTimer = true;
    //     _calculateTime = (int)(_serverTime - _startTime).TotalSeconds;
    //     // Debug.Log($"OPOPOPO _startTime : {_startTime}");
    //     // Debug.Log($"OPOPOPO _serverTime : {_serverTime}");
    //     // Debug.Log($"OPOPOPO _calculateTime : {_calculateTime}");
    //     _isSettingComplete = true;
    //     EnergyRecharge();
    // }
    // public void CheckServerTime(DateTime newHeartBeat)
    // {
    //     _serverTime = newHeartBeat; // 5초마다 웹소켓에서 전해준 값으로 서버시간 업데이트. 
    //     EnergyTimer(); //.ConfigureAwait(false); // 비동기로 실행. 끝나고 메인스레드로 돌아가지 않음.
    // }
    // private void EnergyRecharge()
    // {
    //     // 코루틴은 메인스레드에서 진행 됨.
    //     // 서버시간을 받으면서 백그라운드 스레드로 진행되기 때문에 메인스레드로 전환이 필요.
    //     if(_tickCo == null)
    //     {
    //         _tickCo = StartCoroutine(EnergyRechargeCoroutine());
    //     }
    // }
    // private IEnumerator EnergyRechargeCoroutine()
    // {
    //     yield return new WaitWhile(() => _isSettingComplete == false);
    //     _rechargeTimer?.Dispose();
    //     _rechargeTimer = Observable.Interval(new TimeSpan(0, 0, 1))
    //         .Subscribe(_ =>
    //         {   
    //             _calculateTime++;
    //             _displayTime = 600 - _calculateTime;
    //             if(300 <= _calculateTime)
    //             {
    //                 _startTime = _serverTime;
    //                 _displayTime = 0;
    //                 _calculateTime = 0;
    //                 Managers.Event.TriggerEvent(EEventType.UpdateEnergy, this);
    //             }
    //             GetText((int)Texts.EnergyTimer_Text).text = string.Format($"{(_displayTime-300) / 60} : {(_displayTime-300) % 60}");
    //         }).AddTo(this.gameObject);
    // }
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
