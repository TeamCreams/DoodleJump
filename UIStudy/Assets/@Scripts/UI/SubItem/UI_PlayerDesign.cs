using System;
using System.Collections;
using System.Collections.Generic;
using GameApi.Dtos;
using UniRx;
using UnityEngine;
using static Define;

public class UI_PlayerDesign : UI_Base
{

    private enum Images
    {
        Hair,
        Eyes,
        Eyebrows,
    }

    public override bool Init()
    {
        if (base.Init() == false)
        {
            return false;
        }
        BindImages(typeof(Images));
        Managers.Event.AddEvent(EEventType.SetStyle_Player, OnEvent_SetStyle);
        Managers.Event.TriggerEvent(EEventType.SetStyle_Player);
        return true;
    }
    private void OnDestroy()
    {
        Managers.Event.RemoveEvent(EEventType.SetStyle_Player, OnEvent_SetStyle);
    }
    public void OnEvent_SetStyle(Component sender, object param)
    {
        GetImage((int)Images.Hair).sprite = Managers.Resource.Load<Sprite>($"{Managers.Game.ChracterStyleInfo.TempHair}.sprite");
        GetImage((int)Images.Eyebrows).sprite = Managers.Resource.Load<Sprite>($"{Managers.Game.ChracterStyleInfo.TempEyebrows}.sprite");
        GetImage((int)Images.Eyes).sprite = Managers.Resource.Load<Sprite>($"{Managers.Game.ChracterStyleInfo.TempEyes}.sprite");
        //SaveData();
    }
    void SaveData(Action onSuccess = null, Action onFailed = null)
    {
        Managers.WebContents.ReqDtoUpdateUserStyle(new ReqDtoUpdateUserStyle()
        {
            UserAccountId = Managers.Game.UserInfo.UserAccountId,
            CharacterId = Managers.Game.ChracterStyleInfo.CharacterId,
            HairStyle = Managers.Game.ChracterStyleInfo.Hair,
            EyebrowStyle = Managers.Game.ChracterStyleInfo.Eyebrows,
            EyesStyle = Managers.Game.ChracterStyleInfo.Eyes,
            Evolution = Managers.Game.UserInfo.EvolutionId
        },
        (response) =>
        {
                onSuccess?.Invoke();// 얘 안에서 씬 전환
                //Managers.Evolution.EvolutionDict();
        },
        (errorCode) =>
        {
                //UI_ErrorButtonPopup popup = Managers.UI.ShowPopupUI<UI_ErrorButtonPopup>();
                //ErrorStruct errorStruct = Managers.Error.GetError(EErrorCode.ERR_NetworkSettlementErrorResend);
                //Managers.Event.TriggerEvent(EEventType.ErrorButtonPopup, this, errorStruct.Notice);
                //popup.AddOnClickAction(onFailed);
        });
    }
}
