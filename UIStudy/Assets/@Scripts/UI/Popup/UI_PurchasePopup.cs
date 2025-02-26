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
    private EProductType _productType;
    private int _gold = 0;
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
        _productType = productType;
        switch(_productType)
        {
            case EProductType.Custom:  
            {
                GetText((int)Texts.Gold_Text).text = HardCoding.ChangeStyleGold.ToString();
                _gold = _item.Gold;
            } 
            break;
            case EProductType.Evolution:
            {
                _item = Managers.Data.EvolutionDataDic[id];
                GetText((int)Texts.Gold_Text).text = _item.Gold.ToString();
                _gold = _item.Gold;
            }
            break;
            default:
            break;
        }
    }

    private void OnEvent_ClickClose(PointerEventData eventData)
    {
        Managers.UI.ClosePopupUI(this);
    }

    private void OnEvent_ClickOk(PointerEventData eventData)
    {
        // 서버랑 연결해서 돈 빼기 ->   UI_EvolutionItem에서 한 번에
        int remainingChange = Managers.Game.UserInfo.Gold - _gold;
        if(0 <= remainingChange)
        {
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
            Gold = _gold
        },
       (response) =>
       {
            onSuccess?.Invoke();
            switch(_productType)
            {
                case EProductType.Custom:  
                {
                    Managers.Event.TriggerEvent(EEventType.SetStyle_Player, this);
                } 
                break;
                case EProductType.Evolution:
                {
                    Managers.Game.UserInfo.EvolutionId = _item.Id;    
                }
                break;
                default:
                break;
            }
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
