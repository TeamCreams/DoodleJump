using UnityEngine;
using static Define;

public class UI_CharacterStyleItemText : UI_Base
{
    enum Texts
    {
       PrevItem_Text,
       NextItem_Text
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

    public void SetInfo(int prev, int next)
    {
        string prevName = Managers.Data.PlayerDic[prev].Name;
        string nextName = Managers.Data.PlayerDic[next].Name;
        GetText((int)Texts.PrevItem_Text).text = prevName;
        GetText((int)Texts.NextItem_Text).text = nextName;
    }
}
