using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class UI_TopBar : UI_Base
{

    public enum GameObjects
    {
        FirstHeart_Image,
        SecondHeart_Image,
        ThirdHeart_Image
    }
    public enum Texts
    {
        Time_Text,
        GameOver_Text
    }

    private int _time = 0;
    System.IDisposable _lifeTimer; 
    protected override void Init()
    {
        base.Init();
        BindObjects(typeof(GameObjects));
        BindTexts(typeof(Texts));
        GetText((int)Texts.GameOver_Text).enabled = false;

        Managers.Game.OnChangedLife -= OnChangedLife;
        Managers.Game.OnChangedLife += OnChangedLife;

        _lifeTimer = Observable.Interval(new System.TimeSpan(0, 0, 1))
            .Subscribe(_ =>
            {
                _time++;
                int minutes = Mathf.FloorToInt(_time / 60);
                float seconds = _time % 60;
                GetText((int)Texts.Time_Text).text = string.Format($"{minutes}분 {seconds}초");
            }).AddTo(this.gameObject);
    }

    void OnChangedLife(int life)
	{
        if(life < 0)
		{
            _lifeTimer?.Dispose();
        }
        UpdateLifeImage();
    }

	private void UpdateLifeImage()
    {
        int life = Managers.Game.Life;

        if (0 <= life && life < 3)
        {
            GetObject((int)(GameObjects.FirstHeart_Image + (2 - life))).SetActive(false);
        }
        else if(life < 0)
        {
            GetText((int)Texts.GameOver_Text).enabled = true;
        }
    }
}
