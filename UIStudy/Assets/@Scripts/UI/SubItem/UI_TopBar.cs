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
    
    private Coroutine _updateLifeCoroutine = null;
    private bool _isDeathProcessed = false;  // 사망 처리 완료 여부
    private bool _isResurrectionUsed = false; // 부활 여부
    
    public override bool Init()
    {
        if (base.Init() == false)
        {
            return false;
        }

        BindTexts(typeof(Texts));
        BindSliders(typeof(Sliders));
        BindButtons(typeof(Buttons));

        //_heartRoot = GetObject((int)GameObjects.UI_HeartRoot).GetComponent<UI_HeartRoot>();

        //Managers.Game.OnChangedLife -= OnChangedLife;
        //Managers.Game.OnChangedLife += OnChangedLife;

        // 기존 이벤트 해제 후 재등록
        Managers.Event.RemoveEvent(EEventType.ChangePlayerLife, OnEvent_ChangedLife);
        Managers.Event.AddEvent(EEventType.ChangePlayerLife, OnEvent_ChangedLife);
        Managers.Event.AddEvent(EEventType.LevelStageUp, OnEvent_LevelUpTextChange);
        Managers.Event.AddEvent(EEventType.GetGold, OnEvent_GetGold);
        Managers.Event.AddEvent(EEventType.SetLanguage, OnEvent_SetLanguage);
        Managers.Event.AddEvent(EEventType.UIStoneCountRefresh, OnEvent_ChallengeScaleCount);
        Managers.Event.AddEvent(EEventType.OnPlayerRevive, OnEvent_PlayerRevive);
        Managers.Event.AddEvent(EEventType.OnPlayerDead, OnEvent_PlayerDead);

        OnEvent_SetLanguage(null, null);

        // 스테이지 레벨 표시
        string str = Managers.Game.DifficultySettingsInfo.StageLevel.ToString();
        GetText((int)Texts.Level_Text).text = str;

        GetButton((int)Buttons.Pause_Button).gameObject.BindEvent(OnClick_ShowPausePopup, EUIEvent.Click);

        // 게임 타이머 시작 (1초마다 실행)
        _lifeTimer = Observable.Interval(new System.TimeSpan(0, 0, 1))
            .Subscribe(_ =>
            {
                _time++;
                Managers.Game.GetScore.Total++;
                _minutes = _time / 60;
                _seconds = _time % 60;
                GetText((int)Texts.Time_Text).text = string.Format($"{_minutes}{_minutesString} {_seconds}{_secondsString}");
            }).AddTo(this.gameObject);
        return true;
    }

    private void OnDestroy()
    {
        // 이벤트 해제
        Managers.Event.RemoveEvent(EEventType.ChangePlayerLife, OnEvent_ChangedLife);
        Managers.Event.RemoveEvent(EEventType.LevelStageUp, OnEvent_LevelUpTextChange);
        Managers.Event.RemoveEvent(EEventType.GetGold, OnEvent_GetGold);
        Managers.Event.RemoveEvent(EEventType.SetLanguage, OnEvent_SetLanguage);
        Managers.Event.RemoveEvent(EEventType.UIStoneCountRefresh, OnEvent_ChallengeScaleCount);
        Managers.Event.RemoveEvent(EEventType.OnPlayerRevive, OnEvent_PlayerRevive);
        Managers.Event.RemoveEvent(EEventType.OnPlayerDead, OnEvent_PlayerDead);
        
        // 코루틴 삭제
        if(_updateLifeCoroutine != null)
        {
            StopCoroutine(_updateLifeCoroutine);
            _updateLifeCoroutine = null;
        }
    }

    // 플레이어 부활 이벤트 처리
    private void OnEvent_PlayerRevive(Component sender, object param)
    {
        _isDeathProcessed = false;
        float maxHp = Managers.Object.Player.Stats.StatDic[EStat.MaxHp].Value;
        Managers.Event.TriggerEvent(Define.EEventType.ChangePlayerLife, this, maxHp);
    }
    
    // 플레이어 사망 이벤트 처리
    private void OnEvent_PlayerDead(Component sender, object param)
    {
        // 타이머 중지 및 리소스 정리
        _lifeTimer?.Dispose();
        _lifeTimer = null;
    }
    
    private void OnEvent_LevelUpTextChange(Component sender, object param)
    {
        //Debug.Log("OnEvent_LevelUpTextChange");
        string str = Managers.Game.DifficultySettingsInfo.StageLevel.ToString();
        GetText((int)Texts.Level_Text).text = str;
    }

    private void OnEvent_ChangedLife(Component sender, object param)
    {
        float life = (float)param;
        Debug.Log($"[UI_TopBar] Life changed to: {life}, IsDeathProcessed: {_isDeathProcessed}");
        
        // 기존 코루틴이 실행 중이면 중지
        if(_updateLifeCoroutine != null)
        {
            StopCoroutine(_updateLifeCoroutine);
            _updateLifeCoroutine = null;
        }
        
        // 새로운 코루틴 시작
        _updateLifeCoroutine = StartCoroutine(UpdateLife(life));
    }

    private void OnEvent_GetGold(Component sender, object param)
    {
        GetText((int)Texts.Gold_Text).text = Managers.Game.Gold.ToString();
    }
    
    private void OnEvent_ChallengeScaleCount(Component sender, object param)
    {
        GetText((int)Texts.Stone_Text).text = Managers.Game.DifficultySettingsInfo.ChallengeScaleCount.ToString();
    }
    
    private void OnClick_ShowPausePopup(PointerEventData eventData)
    {
        Managers.UI.ShowPopupUI<UI_PausePopup>();
    }
    
    IEnumerator UpdateLife(float nextHp)
    {
        // 체력 비율 계산
        float toHp = (float)nextHp / Managers.Object.Player.Stats.StatDic[EStat.MaxHp].Value;
        float fromHp = GetSlider((int)Sliders.UI_HpProgressBar).value;

        float maxDuration = 0.15f;
        float duration = maxDuration;
        
        // 체력바 애니메이션 처리
        while(0 < duration)
        {
            GetSlider((int)Sliders.UI_HpProgressBar).value = Mathf.Lerp(fromHp, toHp, 1 - duration / maxDuration);

            duration -= UnityEngine.Time.deltaTime;
            yield return null;
        }
        GetSlider((int)Sliders.UI_HpProgressBar).value = toHp;

        if(toHp <= 0 && !_isDeathProcessed)
        {
            _isDeathProcessed = true; // 사망 처리 완료 표시
            
            // 타이머 중지
            if (_lifeTimer != null)
            {
                _lifeTimer.Dispose();
                _lifeTimer = null;
            }

            Managers.Game.UserInfo.LatelyScore = Managers.Game.GetScore.Total;
            Managers.Game.GetScore.LatelyPlayTime = _time;

            // 레벨에 따라 계속하기 or 죽음
            if(1 < Managers.Game.DifficultySettingsInfo.StageLevel && !_isResurrectionUsed)
            {
                _isResurrectionUsed = true; // 부활 한 번만
                Managers.UI.ShowPopupUI<UI_ContinuePopup>().SetInfo();
            }
            else
            {
                Managers.Event.TriggerEvent(EEventType.OnPlayerDead, this, 0);
            }
        }
        _updateLifeCoroutine = null;
    }

    private void OnEvent_SetLanguage(Component sender, object param)
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