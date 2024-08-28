using System.Collections;
using System.Collections.Generic;
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

    protected override void Init()
    {
        base.Init();
        BindObjects(typeof(GameObjects));
        BindTexts(typeof(Texts));
        StartCoroutine(UpdateLifeTime());
        GetText((int)Texts.GameOver_Text).enabled = false;

    }
    private void Update()
    {
        UpdateLifeImage();
    }

    IEnumerator UpdateLifeTime()
    {
        _time++;
        int minutes = Mathf.FloorToInt(_time / 60);
        float seconds = _time % 60;
        GetText((int)Texts.Time_Text).text = string.Format($"{minutes}분 {seconds}초");
        yield return new WaitForSeconds(1);
        if(0 < Managers.Game.Life)
        {
            StartCoroutine(UpdateLifeTime());
        }
    }
    /*private void UpdateLifeTime()
    {
        float deltaTime = Time.time;

        int minutes = Mathf.FloorToInt(deltaTime / 60);
        float seconds = deltaTime % 60;

        GetText((int)Texts.Time_Text).text = string.Format($"{minutes}분 {seconds:F2}초");
    }*/
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
