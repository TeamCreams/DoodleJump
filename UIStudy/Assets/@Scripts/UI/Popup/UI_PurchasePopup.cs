using System;
using Assets.HeroEditor.InventorySystem.Scripts.Data;
using Data;
using GameApi.Dtos;
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
            UpdateUserGold();
        }
        else
        {
            Managers.UI.ShowPopupUI<UI_ToastPopup>();
            (string title, string notice) = Managers.Error.GetError(EErrorCode.ERR_GoldInsufficient);
            Managers.Event.TriggerEvent(EEventType.ToastPopupNotice, null, notice);
        }
    }

    private void UpdateUserGold(Action onSuccess = null, Action onFailed = null)
    {
        Managers.WebContents.ReqDtoUpdateUserGold(new ReqDtoUpdateUserGold()
        {
            UserAccountId = Managers.Game.UserInfo.UserAccountId,
            Gold = Managers.Game.UserInfo.Gold
        },
       (response) =>
       {
            onSuccess?.Invoke();
            Managers.UI.ClosePopupUI(this);
       },
       (errorCode) =>
       {
            UI_ErrorButtonPopup popup = Managers.UI.ShowPopupUI<UI_ErrorButtonPopup>();
            (string title, string notice) = Managers.Error.GetError(EErrorCode.ERR_NetworkSettlementErrorResend);
            Managers.Event.TriggerEvent(EEventType.ErrorButtonPopup, this, notice);
            popup.AddOnClickAction(onFailed);
       });
    }
}
