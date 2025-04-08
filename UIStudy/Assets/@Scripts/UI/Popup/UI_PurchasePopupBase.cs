using System;
using GameApi.Dtos;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static Define;

public abstract class UI_PurchasePopupBase : UI_Popup
{
    protected enum BaseImages
    {
        Close_Button,
        Ok_Button
    }

    protected enum BaseTexts
    {
        Title_Text,
        Gold_Text
    }

    protected int _gold = 0;

    public override bool Init()
    {
        if (base.Init() == false)
        {
            return false;
        }

        // bind 공통 UI
        BindImages(typeof(BaseImages));
        //BindTexts(typeof(BaseTexts));

        // event
        GetImage((int)BaseImages.Close_Button).gameObject.BindEvent(OnClick_ClosePopup, EUIEvent.Click);
        GetImage((int)BaseImages.Ok_Button).gameObject.BindEvent(OnEvent_ClickOk, EUIEvent.Click);

        return true;
    }
    protected virtual void OnDestroy()
    {
        // 이벤트 제거 (필요한 경우)
    }

    protected virtual void OnClick_ClosePopup(PointerEventData eventData)
    {
        Managers.UI.ClosePopupUI(this);
    }
    protected abstract void OnEvent_ClickOk(PointerEventData eventData);

    protected virtual void UpdateUserGold(Action onSuccess = null, Action onFailed = null)
    {
        Managers.WebContents.ReqDtoUpdateUserGold(new ReqDtoUpdateUserGold()
        {
            UserAccountId = Managers.Game.UserInfo.UserAccountId,
            Gold = _gold
        },
       (response) =>
       {
            onSuccess?.Invoke();
            Managers.Event.TriggerEvent(EEventType.PayGold);
            AfterPurchaseProcess();
            Managers.UI.ClosePopupUI(this);
       },
       (errorCode) =>
        {
            UI_ErrorButtonPopup.ShowErrorButton(Managers.Error.GetError(Define.EErrorCode.ERR_NetworkSettlementErrorResend), onFailed, EScene.SuberunkerSceneHomeScene);
       });
    }

    protected abstract void AfterPurchaseProcess();

    protected virtual void ShowGoldInsufficientError()
    {
        UI_ToastPopup.ShowError(Managers.Error.GetError(EErrorCode.ERR_GoldInsufficient));
        Managers.UI.ShowPopupUI<UI_ShopPanel>();
        Managers.Event.TriggerEvent(EEventType.EnterShop);
    }
}
