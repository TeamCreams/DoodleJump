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
    private EvolutionData _item;
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
    public void SetInfo(int id, EProductType productType)
    {
        switch(productType)
        {
            case EProductType.Custom:
            break;
            case EProductType.Evolution:
                _item = Managers.Data.EvolutionDataDic[id];
            break;
            default:
            break;
        }
        GetText((int)Texts.Gold_Text).text = _item.Gold.ToString();
    }

    private void OnEvent_ClickClose(PointerEventData eventData)
    {
        Managers.UI.ClosePopupUI(this);
    }

    private void OnEvent_ClickOk(PointerEventData eventData)
    {
        // 서버랑 연결해서 돈 빼기 ->   UI_EvolutionItem에서 한 번에
        int remainingChange = Managers.Game.UserInfo.Gold - _item.Gold;
        if(0 <= remainingChange)
        {
            Managers.Game.UserInfo.EvolutionId = _item.Id;    
            UpdateUserGold();
        }
        else
        {
            Managers.UI.ShowPopupUI<UI_ToastPopup>();
            ErrorStruct errorStruct = Managers.Error.GetError(EErrorCode.ERR_GoldInsufficient);
            Managers.Event.TriggerEvent(EEventType.ToastPopupNotice, null, errorStruct.Notice);
        }
    }

    private void UpdateUserGold(Action onSuccess = null, Action onFailed = null)
    {
        Managers.WebContents.ReqDtoUpdateUserGold(new ReqDtoUpdateUserGold()
        {
            UserAccountId = Managers.Game.UserInfo.UserAccountId,
            Gold = _item.Gold
        },
       (response) =>
       {
            onSuccess?.Invoke();
            Managers.UI.ClosePopupUI(this);
       },
       (errorCode) =>
       {
            UI_ErrorButtonPopup popup = Managers.UI.ShowPopupUI<UI_ErrorButtonPopup>();
            ErrorStruct errorStruct = Managers.Error.GetError(EErrorCode.ERR_NetworkSettlementErrorResend);
            Managers.Event.TriggerEvent(EEventType.ErrorButtonPopup, this, errorStruct.Notice);
            popup.AddOnClickAction(onFailed);
       });
    }
}
