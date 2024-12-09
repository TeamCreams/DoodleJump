using Assets.HeroEditor.InventorySystem.Scripts.Data;
using Data;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static Define;

public class UI_PurchasePopup : UI_Popup
{

    private enum Buttons
    {
        Close_Button,
        Ok_Button
    }

    private enum Texts
    {
        Title_Text,
        Notice_Text,
        Gold_Text
    }
    private EvolutionData item;
    public override bool Init()
    {
        if (base.Init() == false)
        {
            return false;
        }
        BindButtons(typeof(Buttons));
        BindTexts(typeof(Texts));

        GetButton((int)Buttons.Close_Button).gameObject.BindEvent(OnEvent_ClickClose, EUIEvent.Click);
        GetButton((int)Buttons.Ok_Button).gameObject.BindEvent(OnEvent_ClickOk, EUIEvent.Click);
        
        return true;
    }
    public void SetInfo(int id)
    {
        item = Managers.Data.EvolutionDataDic[id];
        GetText((int)Texts.Gold_Text).text = item.Gold.ToString();
    }

    private void OnEvent_ClickClose(PointerEventData eventData)
    {
        Managers.UI.ClosePopupUI(this);
    }

    private void OnEvent_ClickOk(PointerEventData eventData)
    {
        // 서버랑 연결해서 돈 빼기 ->   UI_EvolutionItemd에서 한 번에
        int afterGold = Managers.Game.UserInfo.Gold - item.Gold;
        if(0 <= afterGold)
        {
            Managers.Game.UserInfo.Gold -= item.Gold;
            Managers.Game.UserInfo.EvolutionId = item.Id;
        }
        else
        {
            // 골드부족
        }
        Managers.UI.ClosePopupUI(this);
    }
}
