using Data;
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
    private enum GameObjects
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
        BindObjects(typeof(GameObjects));
        return true;
    }
    
    public void SetInfo(int id)
    {
        EvolutionItemData item = Managers.Data.EvolutionItemDataDic[id];
        GetText((int)Texts.Level_Text).text = item.Level.ToString();
        ToggleGroup parent = this.transform.parent.gameObject.GetComponent<ToggleGroup>();
        GetToggle((int)Toggles.EvolutionItem_Mask).group = parent;
        GetToggle((int)Toggles.EvolutionItem_Armor).group = parent;
        GetToggle((int)Toggles.EvolutionItem_Boots).group = parent;
        GetObject((int)GameObjects.EvolutionItem_Boots).GetComponent<UI_EvolutionItem>().SetIcon(item.EvolutionDataId[0]);
        GetObject((int)GameObjects.EvolutionItem_Armor).GetComponent<UI_EvolutionItem>().SetIcon(item.EvolutionDataId[1]);
        GetObject((int)GameObjects.EvolutionItem_Mask).GetComponent<UI_EvolutionItem>().SetIcon(item.EvolutionDataId[2]);
    }
}
