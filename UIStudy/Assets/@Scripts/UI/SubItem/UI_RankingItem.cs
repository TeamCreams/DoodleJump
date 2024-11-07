using GameApi.Dtos;
using UnityEngine;

public class UI_RankingItem : UI_Base
{
 
    private enum Texts
    {
        Ranking_Text,
        Nickname_Text,
        Score_Text
    }
    public override bool Init()
    {
        if (base.Init() == false)
        {
            return false;
        }
        BindTexts(typeof(Texts));
        return true;
    }

    public void SetInfo(ResDtoGetUserAccountListElement element, int rank)
    {
        GetText((int)Texts.Ranking_Text).text = rank.ToString();
        GetText((int)Texts.Nickname_Text).text = element.Nickname;
        int recordMinutes = element.HighScore / 60;
        int recordSeconds = element.HighScore % 60;
        GetText((int)Texts.Score_Text).text = $"{recordMinutes}m {recordSeconds}s";
    }

}
