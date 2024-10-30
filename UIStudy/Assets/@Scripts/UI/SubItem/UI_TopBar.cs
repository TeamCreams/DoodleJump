using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using static Define;

public class UI_TopBar : UI_Base
{

    public enum GameObjects
    {
        UI_HeartRoot,
    }
    public enum Texts
    {
        Time_Text,
        GameOver_Text,
        Level_Text
    }
    public enum Images
    {
        Hp,
    }

    private int _time = 0;

    public int Time
    { 
        get { return _time; } 
        set {  _time = value; } 
    }

    System.IDisposable _lifeTimer;

    private UI_HeartRoot _heartRoot;
    private string _minutes = "분";
    private string _seconds = "초";

    public override bool Init()
    {
        if (base.Init() == false)
        {
            return false;
        }
        BindImages(typeof(Images));

        //BindObjects(typeof(GameObjects));
        BindTexts(typeof(Texts));
        GetText((int)Texts.GameOver_Text).enabled = false;

        //_heartRoot = GetObject((int)GameObjects.UI_HeartRoot).GetComponent<UI_HeartRoot>();

        //Managers.Game.OnChangedLife -= OnChangedLife;
        //Managers.Game.OnChangedLife += OnChangedLife;
        Managers.Event.RemoveEvent(EEventType.ChangePlayerLife, OnChangedLife);
        Managers.Event.AddEvent(EEventType.ChangePlayerLife, OnChangedLife);
        Managers.Event.AddEvent(EEventType.LevelStageUp, OnEvent_LevelUpTextChange);
        Managers.Event.AddEvent(EEventType.SetLanguage, OnEvent_SetLanguage);
        Managers.Event.TriggerEvent(EEventType.SetLanguage);

        string str = "Lv." + Managers.Game.DifficultySettingsInfo.StageLevel.ToString();
        GetText((int)Texts.Level_Text).text = str;

        

        _lifeTimer = Observable.Interval(new System.TimeSpan(0, 0, 1))
            .Subscribe(_ =>
            {
                _time++;
                int minutes = _time / 60;
                float seconds = _time % 60;
                GetText((int)Texts.Time_Text).text = string.Format($"{minutes}{_minutes} {seconds}{_seconds}");
            }).AddTo(this.gameObject);
        return true;
    }

	private void OnDestroy()
    {
        Managers.Event.RemoveEvent(Define.EEventType.ChangePlayerLife, OnChangedLife);
    }
   
    void OnEvent_LevelUpTextChange(Component sender, object param)
    {
        //Debug.Log("OnEvent_LevelUpTextChange");
        string str = "Lv." + Managers.Game.DifficultySettingsInfo.StageLevel.ToString();
        GetText((int)Texts.Level_Text).text = str;
    }
	void OnChangedLife(Component sender, object param)
	{
        float life = (float)param;
        Debug.Log($"Life : {life}");
        StartCoroutine(UpdateLife(life));
        //UpdateLifeImage(life);        
    }

    IEnumerator UpdateLife(float nextHp)
    {
        float currentHp = GetImage((int)Images.Hp).fillAmount;

        if (nextHp < currentHp)
        {
            while (nextHp < currentHp)
            {
                currentHp -= 0.05f; 
                if (currentHp < nextHp)
                {
                    currentHp = nextHp;
                }

                GetImage((int)Images.Hp).fillAmount = currentHp;

                yield return new WaitForSeconds(0.05f);
            }

            if (currentHp <= 0)
            {
                Managers.Game.PlayTime = _time;
                _lifeTimer?.Dispose();
                if (Managers.Game.PlayTimeRecord < _time)
                {
                    Managers.Game.PlayTimeRecord = _time;
                }
                Managers.UI.ShowPopupUI<UI_RetryPopup>();
            }
        }
        else
        {
            while (currentHp < nextHp)
            {
                currentHp += 0.05f; 
                if (nextHp < currentHp) 
                {
                    currentHp = nextHp;
                }

                GetImage((int)Images.Hp).fillAmount = currentHp;

                yield return new WaitForSeconds(0.05f);
            }
        }
    }

    void OnEvent_SetLanguage(Component sender, object param)
    {
        _minutes = Managers.Language.LocalizedString(91004);
        _seconds = Managers.Language.LocalizedString(91005);
    }
}

// 2가지 방법
// 1. 프로그레스바로 만들기 (x)
// 2. 2번째
//   - 상위부모를 Horizon Layout
//    - 그아래에 생성한다.
