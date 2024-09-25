using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class UI_TopBar : UI_Base
{

    public enum GameObjects
    {
        UI_HeartRoot,
    }
    public enum Texts
    {
        Time_Text,
        GameOver_Text
    }

    private int _time = 0;

    public int Time
    { 
        get { return _time; } 
        set {  _time = value; } 
    }

    System.IDisposable _lifeTimer;

    UI_HeartRoot _heartRoot;

    public override bool Init()
    {
        if (base.Init() == false)
        {
            return false;
        }
        BindObjects(typeof(GameObjects));
        BindTexts(typeof(Texts));
        GetText((int)Texts.GameOver_Text).enabled = false;

        _heartRoot = GetObject((int)GameObjects.UI_HeartRoot).GetComponent<UI_HeartRoot>();

        Managers.Game.OnChangedLife -= OnChangedLife;
        Managers.Game.OnChangedLife += OnChangedLife;

        _lifeTimer = Observable.Interval(new System.TimeSpan(0, 0, 1))
            .Subscribe(_ =>
            {
                _time++;
                int minutes = _time / 60;
                float seconds = _time % 60;
                GetText((int)Texts.Time_Text).text = string.Format($"{minutes}분 {seconds}초");
            }).AddTo(this.gameObject);
        return true;
    }

	private void OnDestroy()
	{
        Managers.Game.OnChangedLife -= OnChangedLife;
    }

	void OnChangedLife(int life)
	{
        if(life <= 0)
        {
            _lifeTimer?.Dispose();
            if (Managers.Game.TimeRecord < _time)
            {
                Managers.Game.TimeRecord = _time;
            }
            Debug.Log($"최고기록 : {Managers.Game.TimeRecord}");
            Managers.UI.ShowPopupUI<UI_RetryPopup>();
        }
        UpdateLifeImage();        
    }

    private void UpdateLifeImage()
    {
        int life = Managers.Game.Life;

        _heartRoot.SetLife(life);
        if (life == 0)
		{
			GetText((int)Texts.GameOver_Text).enabled = true;
		}
	}
}

// 2가지 방법
// 1. 프로그레스바로 만들기 (x)
// 2. 2번째
//   - 상위부모를 Horizon Layout
//    - 그아래에 생성한다.
