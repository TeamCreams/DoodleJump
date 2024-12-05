using UnityEngine;
using UnityEngine.UI;
using static Define;

public class UI_EvolutionItemSet : UI_Base
{

    private enum Texts
    {
        Level_Text
    }
    private enum Toggles
    {
        EvolutionItem_Mask,
        EvolutionItem_Armor,
        EvolutionItem_Boots
    }
    public override bool Init()
    {
        if (base.Init() == false)
        {
            return false;
        }
        BindTexts(typeof(Texts));
        BindToggles(typeof(Toggles));
        return true;
    }
    
    public void SetInfo(int id)
    {
        GetText((int)Texts.Level_Text).text = Managers.Data.EvolutionDataDic[id].Level.ToString();
        ToggleGroup parent = this.transform.parent.gameObject.GetComponent<ToggleGroup>();
        GetToggle((int)Toggles.EvolutionItem_Mask).group = parent;
        GetToggle((int)Toggles.EvolutionItem_Armor).group = parent;
        GetToggle((int)Toggles.EvolutionItem_Boots).group = parent;
    }
}
