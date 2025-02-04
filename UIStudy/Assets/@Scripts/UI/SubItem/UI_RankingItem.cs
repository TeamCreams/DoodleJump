using GameApi.Dtos;
using UnityEngine;
using static Define;

public class UI_RankingItem : UI_Base
{
 
    private enum Texts
    {
        Ranking_Text,
        Nickname_Text,
        Score_Text
    }
    private string _minutesString = "분";
    private string _secondsString = "초";
    private int _recordMinutes;
    private float _recordSeconds;
    private int _recordScore;
    public override bool Init()
    {
        if (base.Init() == false)
        {
            return false;
        }
        BindTexts(typeof(Texts));
        Managers.Event.AddEvent(EEventType.SetLanguage, OnEvent_SetLanguage);
        OnEvent_SetLanguage(null, null);
        return true;
    }
    private void OnDestroy()
    {
        Managers.Event.RemoveEvent(EEventType.SetLanguage, OnEvent_SetLanguage);
    }

    public void SetInfo(ResDtoGetUserAccountListElement element, int rank)
    {
        GetText((int)Texts.Ranking_Text).text = rank.ToString();
        GetText((int)Texts.Nickname_Text).text = element.Nickname;
        _recordScore = element.HighScore;
        GetText((int)Texts.Score_Text).text = $"{_recordScore:N0}";

        //_recordMinutes = element.HighScore / 60;
        //_recordSeconds = element.HighScore % 60;
        //GetText((int)Texts.Score_Text).text = $"{_recordMinutes}m {_recordSeconds}s";
    }

    void OnEvent_SetLanguage(Component sender, object param)
    {
        //_minutesString = Managers.Language.LocalizedString(91004);
        //_secondsString = Managers.Language.LocalizedString(91005);
        //GetText((int)Texts.Score_Text).text = $"{_recordMinutes}{_minutesString} {_recordSeconds}{_secondsString}";
        GetText((int)Texts.Score_Text).text = $"{_recordScore:N0}";
    }
}
