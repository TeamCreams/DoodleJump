using System;
using GameApi.Dtos;
using UnityEngine;
using UnityEngine.EventSystems;
using static Define;

public class UI_EnergyShopPanel : UI_PurchasePopupBase
{
    private enum Images
    {
        Close_Button,
        Ok_Button,
    }
    private enum Texts
    {
        Title_Text,
        Gold_Text,
        UpdateTime_Text
    }

    public override bool Init()
    {
        if (base.Init() == false)
        {
            return false;
        }
        //Bind
        BindTexts(typeof(Texts));
        BindImages(typeof(Images));
        BindCommonEvents();
        //Get
        //GetImage((int)Images.Close_Button).gameObject.BindEvent(OnClick_ClosePopup, EUIEvent.Click);
        //GetImage((int)Images.Ok_Button).gameObject.BindEvent(OnEvent_ClickOk, EUIEvent.Click);

        //Event
        //Managers.Event.AddEvent(EEventType.EnterShop, SetMerchandiseItems);
        return true;
    }
    protected override void OnDestroy()
    {
        base.OnDestroy();
        //Managers.Event.RemoveEvent(EEventType.EnterShop, SetMerchandiseItems);
    }  

    public void SetInfo()
    {
        int purchaseMultiplier = Managers.Game.UserInfo.PurchaseEnergyCountToday + 1;
        _gold = purchaseMultiplier * HardCoding.ChangeStyleGold; // 임시 가격
        GetText((int)Texts.Gold_Text).text = _gold.ToString();
    }

    protected override void OnClick_ClickOk(PointerEventData eventData)
    {
        int remainingChange = Managers.Game.UserInfo.Gold - _gold;
        if(0 <= remainingChange)
        {
            Managers.Game.GoldTochange = remainingChange;
            UpdateUserGold(() => {
                UpdateEnergy();
            });
        }
        else
        {
            ShowGoldInsufficientError();
        }
    }

    protected override void AfterPurchaseProcess()
    {
        Managers.UI.ClosePopupUI(this);
        // 에너지 추가는 UpdateEnergy에서 처리하므로 여기서는 아무것도 하지 않음
    }

    private void UpdateEnergy(Action onSuccess = null, Action onFailed = null)
    {
        Debug.Log("UpdateEnergy");
        Managers.WebContents.InsertEnergy(new ReqDtoInsertEnergy()
        {
            UserAccountId = Managers.Game.UserInfo.UserAccountId,
            Energy = 10 // 고정값인지 아닌지는 수정 알아서
        },
       (response) =>
       {
            onSuccess?.Invoke();
            
            Managers.Game.UserInfo.Energy = response.Energy;
            Managers.Game.UserInfo.PurchaseEnergyCountToday = response.PurchaseEnergyCountToday;
            
            Managers.Event.TriggerEvent(EEventType.UIRefresh);
            //Managers.UI.ClosePopupUI(this); // UpdateUserGold에서 닫음
       },
       (errorCode) =>
        {
            UI_ErrorButtonPopup.ShowErrorButton(Managers.Error.GetError(Define.EErrorCode.ERR_NetworkSettlementErrorResend), onFailed, EScene.SuberunkerSceneHomeScene);
       });
    }
}
