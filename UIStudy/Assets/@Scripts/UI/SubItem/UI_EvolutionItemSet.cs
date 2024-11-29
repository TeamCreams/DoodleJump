using UnityEngine;
using UnityEngine.UI;
using static Define;

public class UI_EvolutionItemSet : UI_Base
{

    enum Texts
    {
        Level_Text
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
    
    public void SetInfo(int id)
    {
        GetText((int)Texts.Level_Text).text = Managers.Data.EvolutionDataDic[id].Level.ToString();
        Managers.Event.TriggerEvent(EEventType.Evolution);

    }
}
