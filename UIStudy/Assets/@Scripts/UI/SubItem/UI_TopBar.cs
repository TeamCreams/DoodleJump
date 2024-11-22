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
        GameOver_Text,
        Level_Text,
        Gold_Text
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

        GetText((int)Texts.GameOver_Text).enabled = false;

        //_heartRoot = GetObject((int)GameObjects.UI_HeartRoot).GetComponent<UI_HeartRoot>();

        //Managers.Game.OnChangedLife -= OnChangedLife;
        //Managers.Game.OnChangedLife += OnChangedLife;
        Managers.Event.RemoveEvent(EEventType.ChangePlayerLife, OnEvent_ChangedLife);
        Managers.Event.AddEvent(EEventType.ChangePlayerLife, OnEvent_ChangedLife);
        Managers.Event.AddEvent(EEventType.LevelStageUp, OnEvent_LevelUpTextChange);
        Managers.Event.AddEvent(EEventType.GetGold, OnEvent_GetGold);
        Managers.Event.AddEvent(EEventType.SetLanguage, OnEvent_SetLanguage);
        Managers.Event.TriggerEvent(EEventType.SetLanguage);

        string str = "Lv." + Managers.Game.DifficultySettingsInfo.StageLevel.ToString();
        GetText((int)Texts.Level_Text).text = str;

        GetButton((int)Buttons.Pause_Button).gameObject.BindEvent(OnClick_ShowPausePopup, EUIEvent.Click);

        _lifeTimer = Observable.Interval(new System.TimeSpan(0, 0, 1))
            .Subscribe(_ =>
            {
                _time++;
                _minutes = _time / 60;
                _seconds = _time % 60;
                GetText((int)Texts.Time_Text).text = string.Format($"{_minutes}{_minutesString} {_seconds}{_secondsString}");
            }).AddTo(this.gameObject);
        return true;
    }

	private void OnDestroy()
    {
        Managers.Event.RemoveEvent(EEventType.ChangePlayerLife, OnEvent_ChangedLife);
    }
   
    void OnEvent_LevelUpTextChange(Component sender, object param)
    {
        //Debug.Log("OnEvent_LevelUpTextChange");
        string str = "Lv." + Managers.Game.DifficultySettingsInfo.StageLevel.ToString();
        GetText((int)Texts.Level_Text).text = str;
    }
	void OnEvent_ChangedLife(Component sender, object param)
	{
        float life = (float)param;
        Debug.Log($"Life : {life}");
        StartCoroutine(UpdateLife(life));
        
        //UpdateLifeImage(life);        
    }

    void OnEvent_GetGold(Component sender, object param)
    {
        GetText((int)Texts.Gold_Text).text = Managers.Game.Gold.ToString();
    }

    private void OnClick_ShowPausePopup(PointerEventData eventData)
    {

    }
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

        if (toHp <= 0)
        {
            GetSlider((int)Sliders.UI_HpProgressBar).value = 0;
            _lifeTimer?.Dispose();
            Managers.Game.UserInfo.LatelyScore = _time;
            Managers.Game.UserInfo.Gold += Managers.Game.Gold;

            Managers.Event.TriggerEvent(EEventType.OnPlayerDead, this, 0);
        }


        //float currentHp = GetImage((int)Images.Hp).fillAmount;

        ////event로 바꿔야함
        //if (nextHp < currentHp)
        //{
        //    while (nextHp < currentHp)
        //    {
        //        currentHp -= 0.05f; 
        //        if (currentHp < nextHp)
        //        {
        //            currentHp = nextHp;
        //        }

        //        GetImage((int)Images.Hp).fillAmount = currentHp;

        //        yield return new WaitForSeconds(0.05f);
        //    }

        //    if (currentHp <= 0)
        //    {
        //        _lifeTimer?.Dispose();
        //        Managers.Game.UserInfo.LatelyScore = _time;
        //        Managers.Game.UserInfo.Gold += Managers.Game.Gold;
        //        Managers.Resource.Instantiate("UI_Loading", this.transform);
        //        Managers.Event.TriggerEvent(EEventType.StartLoading);
        //        Managers.Score.SetScore(this, ProcessErrorFun);
        //        Managers.Event.TriggerEvent(EEventType.StopLoading);
        //    }
        //}
        //else
        //{
        //    while (currentHp < nextHp)
        //    {
        //        currentHp += 0.05f; 
        //        if (nextHp < currentHp) 
        //        {
        //            currentHp = nextHp;
        //        }

        //        GetImage((int)Images.Hp).fillAmount = currentHp;

        //        yield return new WaitForSeconds(0.05f);
        //    }
        //}
    }

    public void ProcessErrorFun()
    {
        //Debug.Log("ProcessErrorFun");
        //Managers.Resource.Instantiate("UI_Loading", this.transform);
        //Managers.Event.TriggerEvent(EEventType.StartLoading);
        //Managers.Score.SetScore(this, null, null,
        //    ()=>
        //    {
        //        Managers.Event.TriggerEvent(EEventType.StopLoading);
        //        Managers.UI.ShowPopupUI<UI_ToastPopup>();
        //        Managers.Event.TriggerEvent(EEventType.ToastPopupNotice, this, "Failed to save...");
        //        Invoke("ExitGame", 2.5f);
        //        return;
        //    }        
        //);
        //Managers.Event.TriggerEvent(EEventType.StopLoading);
    }

    private void ExitGame()
    {
        Managers.Scene.LoadScene(EScene.SuberunkerSceneHomeScene);
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
