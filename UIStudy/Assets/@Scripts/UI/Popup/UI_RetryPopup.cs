using System.Collections;
using System.Collections.Generic;
using GameApi.Dtos;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static Define;

public class UI_RetryPopup : UI_Popup
{
    private enum Texts
    {
        Score_Text,
        RecordScore_Text,
        Gold_Text,
        Retry_Text,
        Home_Text
    }

    private enum Buttons
    {
        Retry_Button,
        Home_Button
    }

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
        Managers.UI.ClosePopupUI(this);
        Time.timeScale = 1;
        Managers.Game.Gold = 0;

        // 게임 난이도 초기화
        Managers.Game.DifficultySettingsInfo.StageId = 70001;
        Managers.Game.DifficultySettingsInfo.StageLevel = 1; // 이 값을 미션 달성에 사용할 때가 있기 떄문에. 그런데 게임을 새로 시작하면 미션 진행도도 초기화가 되기 때문에 이걸 따로 저장해놔야함.
        Managers.Game.DifficultySettingsInfo.AddSpeed = 0;
        Managers.Game.DifficultySettingsInfo.ChallengeScale = 0;
        // 한 게임당 누적 돌 개수 초기화
        Managers.Game.DifficultySettingsInfo.StoneCount = 0;

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
            Managers.Scene.LoadScene(EScene.SuberunkerScene);
       },
        (errorCode) =>
        {
            loadingComplete.Value = true;
            float time = 1;
            UI_ToastPopup.ShowError(Managers.Error.GetError(EErrorCode.ERR_EnergyInsufficient), time, ()=>Managers.Scene.LoadScene(EScene.SuberunkerSceneHomeScene));
        }
        );
    }
    private void OnClick_HomeButton(PointerEventData eventData)
    {
        // playerDead event 
        var loadingComplete = UI_LoadingPopup.Show();
        Managers.Score.GetScore((this),
        () =>
            {
                loadingComplete.Value = true;
                Managers.UI.ClosePopupUI(this);
                Time.timeScale = 1;
                Managers.Scene.LoadScene(EScene.SuberunkerSceneHomeScene);
            },
        ()=>
        {
            loadingComplete.Value = true;
            if(_failCount < HardCoding.MAX_FAIL_COUNT)
            {
                Time.timeScale = 1;
                _failCount++;
                Managers.Score.GetScore(this);
                return;
            }
            Time.timeScale = 1;
            _failCount = 0;
            Managers.UI.ClosePopupUI(this);
            Managers.Scene.LoadScene(EScene.SuberunkerSceneHomeScene);
        });
    }

    public override void SetOrder(int sortOrder)
    {
        base.SetOrder(sortOrder);
    }

    public void SetRecord()
    {        
        var loadingComplete = UI_LoadingPopup.Show();

        Managers.Score.GetScore(this, null,
        ()=> // 실패했을경우 
        {
            loadingComplete.Value = true;
            if(_failCount < HardCoding.MAX_FAIL_COUNT)
            {
                _failCount++;
                Managers.Score.GetScore(this);
                return;
            }
            _failCount = 0;
            Managers.Scene.LoadScene(EScene.StartLoadingScene);
        });
        GetText((int)Texts.RecordScore_Text).text = $"{_bestRecord} : {Managers.Game.UserInfo.RecordScore:N0}";
        GetText((int)Texts.Score_Text).text = $"{_recentRecord} : {Managers.Game.UserInfo.LatelyScore:N0}";

        GetText((int)Texts.Gold_Text).text = Managers.Game.Gold.ToString();
    }

    public void ProcessErrorFun()
    {
        Managers.Score.GetScore(this);
    }

    void OnEvent_SetLanguage(Component sender, object param)
    {
        _bestRecord = Managers.Language.LocalizedString(91001);
        _recentRecord = Managers.Language.LocalizedString(91002);
        GetText((int)Texts.Home_Text).text = Managers.Language.LocalizedString(91017);
        GetText((int)Texts.Retry_Text).text = Managers.Language.LocalizedString(91018);
    }

}
