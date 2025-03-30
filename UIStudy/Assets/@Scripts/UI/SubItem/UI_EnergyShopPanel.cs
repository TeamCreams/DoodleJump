using System;
using GameApi.Dtos;
using UnityEngine;
using UnityEngine.EventSystems;
using static Define;

public class UI_EnergyShopPanel : UI_Popup
{
    private enum Images
    {
        Close_Button,
        Ok_Button,
    }
    private enum Texts
    {
        Shop_Text
    }

    private int _gold = 0;
    public override bool Init()
    {
        if (base.Init() == false)
        {
            return false;
        }
        //Bind
        BindImages(typeof(Images));
        BindTexts(typeof(Texts));
        //Get
        GetImage((int)Images.Close_Button).gameObject.BindEvent(OnClick_ClosePopup, EUIEvent.Click);
        GetImage((int)Images.Ok_Button).gameObject.BindEvent(OnEvent_ClickOk, EUIEvent.Click);

        //Event
        //Managers.Event.AddEvent(EEventType.EnterShop, SetMerchandiseItems);
        
        return true;
    }
    void OnDestroy()
    {
        //Managers.Event.RemoveEvent(EEventType.EnterShop, SetMerchandiseItems);
    }  

    public void SetInfo()
    {
        int purchaseMultiplier = Managers.Game.UserInfo.PurchaseEnergyCountToday + 1;
        _gold = purchaseMultiplier * HardCoding.ChangeStyleGold; // 임시 가격
    }
    private void OnClick_ClosePopup(PointerEventData eventData)
    {
        Managers.UI.ClosePopupUI(this);
    }

    private void OnEvent_ClickOk(PointerEventData eventData)
    {
        int remainingChange = Managers.Game.UserInfo.Gold - _gold;
        if(0 <= remainingChange)
        {
            Managers.Game.RemainingChange = remainingChange;
            UpdateUserGold();
        }
        else
        {
            UI_ToastPopup.ShowError(Managers.Error.GetError(EErrorCode.ERR_GoldInsufficient));
        }
    }

    private void UpdateUserGold(Action onSuccess = null, Action onFailed = null)
    {
        Debug.Log("UpdateUserGold");
        Managers.WebContents.ReqDtoUpdateUserGold(new ReqDtoUpdateUserGold()
        {
            UserAccountId = Managers.Game.UserInfo.UserAccountId,
            Gold = _gold
        },
       (response) =>
       {
            onSuccess?.Invoke();
            
            // 에너지 추가
            //UI_ToastPopup.Show("Energy", UI_ToastPopup.Type.Debug, 1);
            
            Managers.Event.TriggerEvent(EEventType.UpdateGold);
            UpdateEnergy();
            //Managers.UI.ClosePopupUI(this);
       },
       (errorCode) =>
        {
            UI_ErrorButtonPopup.ShowErrorButton(Managers.Error.GetError(Define.EErrorCode.ERR_NetworkSettlementErrorResend), onFailed, EScene.SuberunkerSceneHomeScene);
       });
    }

    private void UpdateEnergy(Action onSuccess = null, Action onFailed = null)
    {
        Debug.Log("UpdateEnergy");
        Managers.WebContents.ReqDtoInsertEnergy(new ReqDtoInsertEnergy()
        {
            UserAccountId = Managers.Game.UserInfo.UserAccountId,
            Energy = 10 // 구정값인지 아닌지는 수정 알아서
        },
       (response) =>
       {
            onSuccess?.Invoke();
            
            Managers.Game.UserInfo.Energy = response.Energy;
            Managers.Game.UserInfo.PurchaseEnergyCountToday = response.PurchaseEnergyCountToday;
            Managers.Event.TriggerEvent(EEventType.UIRefresh);
            Managers.UI.ClosePopupUI(this);
       },
       (errorCode) =>
        {
            UI_ErrorButtonPopup.ShowErrorButton(Managers.Error.GetError(Define.EErrorCode.ERR_NetworkSettlementErrorResend), onFailed, EScene.SuberunkerSceneHomeScene);
       });
    }
}
