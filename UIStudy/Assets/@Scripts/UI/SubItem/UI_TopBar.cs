using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static Define;

public class UI_TopBar : UI_Base
{

    private enum GameObjects
    {
        UI_HeartRoot,
    }
    private enum Texts
    {
        Time_Text,
        Level_Text,
        Gold_Text,
        Stone_Text
    }
    private enum Sliders
    {
        UI_HpProgressBar,
    }
    private enum Buttons
    {
        Pause_Button
    }

    private int _time = 0;

    public int Time
    { 
        get { return _time; } 
        set {  _time = value; } 
    }

    System.IDisposable _lifeTimer;

    private UI_HeartRoot _heartRoot;
    private string _minutesString = "분";
    private string _secondsString = "초";
    private int _minutes;
    private float _seconds;
    // 추가: 부활 처리 중인지 확인하는 플래그
    private bool _isRevivalInProgress = false;
    
    public override bool Init()
    {
        if (base.Init() == false)
        {
            return false;
        }

        //BindObjects(typeof(GameObjects));
        BindTexts(typeof(Texts));
        BindSliders(typeof(Sliders));
        BindButtons(typeof(Buttons));

        //_heartRoot = GetObject((int)GameObjects.UI_HeartRoot).GetComponent<UI_HeartRoot>();

        //Managers.Game.OnChangedLife -= OnChangedLife;
        //Managers.Game.OnChangedLife += OnChangedLife;
        Managers.Event.RemoveEvent(EEventType.ChangePlayerLife, OnEvent_ChangedLife);
        Managers.Event.AddEvent(EEventType.ChangePlayerLife, OnEvent_ChangedLife);
        Managers.Event.AddEvent(EEventType.LevelStageUp, OnEvent_LevelUpTextChange);
        Managers.Event.AddEvent(EEventType.GetGold, OnEvent_GetGold);
        Managers.Event.AddEvent(EEventType.SetLanguage, OnEvent_SetLanguage);
        Managers.Event.AddEvent(EEventType.UIStoneCountRefresh, OnEvent_ChallengeScaleCount);
        // 추가: 플레이어 부활 이벤트 추가
        Managers.Event.AddEvent(EEventType.OnPlayerRevive, OnEvent_PlayerRevive);
        Managers.Event.AddEvent(EEventType.OnPlayerDead, OnEvent_PlayerDead);
        
        OnEvent_SetLanguage(null, null);
        
        string str = Managers.Game.DifficultySettingsInfo.StageLevel.ToString();
        GetText((int)Texts.Level_Text).text = str;

        GetButton((int)Buttons.Pause_Button).gameObject.BindEvent(OnClick_ShowPausePopup, EUIEvent.Click);

        _lifeTimer = Observable.Interval(new System.TimeSpan(0, 0, 1))
            .Subscribe(_ =>
            {
                _time++;
                Managers.Game.GetScore.Total ++;
                _minutes = _time / 60;
                _seconds = _time % 60;
                GetText((int)Texts.Time_Text).text = string.Format($"{_minutes}{_minutesString} {_seconds}{_secondsString}");
            }).AddTo(this.gameObject);
        return true;
    }

	private void OnDestroy()
    {
        Managers.Event.RemoveEvent(EEventType.ChangePlayerLife, OnEvent_ChangedLife);
        Managers.Event.RemoveEvent(EEventType.LevelStageUp, OnEvent_LevelUpTextChange);
        Managers.Event.RemoveEvent(EEventType.GetGold, OnEvent_GetGold);
        Managers.Event.RemoveEvent(EEventType.SetLanguage, OnEvent_SetLanguage);
        Managers.Event.RemoveEvent(EEventType.UIStoneCountRefresh, OnEvent_ChallengeScaleCount);
        // 추가: 이벤트 제거
        Managers.Event.RemoveEvent(EEventType.OnPlayerRevive, OnEvent_PlayerRevive);
        Managers.Event.RemoveEvent(EEventType.OnPlayerDead, OnEvent_PlayerDead);
    }
   
    // 추가: 플레이어 부활 이벤트 처리
    void OnEvent_PlayerRevive(Component sender, object param)
    {
        // 부활 처리 중 플래그 설정
        _isRevivalInProgress = true;
        isSettleComplete = false;
        
        // 타이머는 계속 실행 중이므로 다시 시작할 필요 없음
        // 기존 상태(시간, 점수, 레벨 등)를 유지
    }
    
    void OnEvent_PlayerDead(Component sender, object param)
    {
        if (_isRevivalInProgress)
        {
            _isRevivalInProgress = false;
            return;
        }
        
        // 타이머 중지 및 리소스 정리
        _lifeTimer?.Dispose();
        _lifeTimer = null;
    }
    
    void OnEvent_LevelUpTextChange(Component sender, object param)
    {
        //Debug.Log("OnEvent_LevelUpTextChange");
        string str = Managers.Game.DifficultySettingsInfo.StageLevel.ToString();
        GetText((int)Texts.Level_Text).text = str;
    }
	void OnEvent_ChangedLife(Component sender, object param)
	{
        float life = (float)param;
        Debug.Log($"Life : {life}");
        
        if (life > 0 && GetSlider((int)Sliders.UI_HpProgressBar).value <= 0)
        {
            _isRevivalInProgress = true;
            isSettleComplete = false;
        }
        
        // 체력 업데이트 애니메이션 실행
        StartCoroutine(UpdateLife(life));
    }

    void OnEvent_GetGold(Component sender, object param)
    {
        GetText((int)Texts.Gold_Text).text = Managers.Game.Gold.ToString();
    }
    void OnEvent_ChallengeScaleCount(Component sender, object param)
    {
        GetText((int)Texts.Stone_Text).text = Managers.Game.DifficultySettingsInfo.ChallengeScaleCount.ToString();
    }
    private void OnClick_ShowPausePopup(PointerEventData eventData)
    {
        Managers.UI.ShowPopupUI<UI_PausePopup>();
    }

    bool isSettleComplete = false;
    IEnumerator UpdateLife(float nextHp)
    {
        float toHp = (float)nextHp / Managers.Object.Player.Stats.StatDic[EStat.MaxHp].Value;
        float fromHp = GetSlider((int)Sliders.UI_HpProgressBar).value;
        float maxDuration = 0.15f;
        float duration = maxDuration;
        while (0 < duration)
        {
            GetSlider((int)Sliders.UI_HpProgressBar).value = Mathf.Lerp(fromHp, toHp, 1 - duration / maxDuration);

            duration -= UnityEngine.Time.deltaTime;
            yield return null;
        }

        // 최종 체력값 설정
        GetSlider((int)Sliders.UI_HpProgressBar).value = toHp;

        // 사망 처리: 체력이 0 이하이고, 아직 처리되지 않았으며, 부활 처리 중이 아닐 때
        if (toHp <= 0 && !isSettleComplete && !_isRevivalInProgress)
        {
            // 타이머 중지
            if (_lifeTimer != null)
            {
                _lifeTimer.Dispose();
                _lifeTimer = null;
            }
            
            // 최근 점수와 플레이 시간 저장
            Managers.Game.UserInfo.LatelyScore = Managers.Game.GetScore.Total;
            Managers.Game.GetScore.LatelyPlayTime = _time;
            
            // 레벨에 따라 계속하기 팝업 또는 사망 이벤트 발생
            if (1 < Managers.Game.UserInfo.StageLevel)
            {
                Managers.UI.ShowPopupUI<UI_ContinuePopup>().SetInfo();
            }
            else
            {
                Managers.Event.TriggerEvent(EEventType.OnPlayerDead, this, 0);
            }
            isSettleComplete = true;
        }
    }

    void OnEvent_SetLanguage(Component sender, object param)
    {
        _minutesString = Managers.Language.LocalizedString(91004);
        _secondsString = Managers.Language.LocalizedString(91005);
        GetText((int)Texts.Time_Text).text = $"{_minutes}{_minutesString} {_seconds}{_secondsString}";
    }
}

// 2가지 방법
// 1. 프로그레스바로 만들기 (x)
// 2. 2번째
//   - 상위부모를 Horizon Layout
//    - 그아래에 생성한다.