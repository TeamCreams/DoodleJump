using System.Collections;
using System.Collections.Generic;
using GameApi.Dtos;
using UnityEngine;
using static Define;

public class SuberunkerSceneHomeScene : BaseScene
{
    
    public override bool Init()
    {
        if (base.Init() == false)
        {
            return false;
        }
        Managers.UI.ShowSceneUI<UI_SuberunkerSceneHomeScene>();

        Managers.Sound.Stop(ESound.Bgm);
        Managers.Sound.Play(ESound.Bgm, "LobbyBGMSound", 0.2f);
        Managers.Event.AddEvent(EEventType.UpdateEnergy, OnEvent_UpdateEnergy);
        return true;
    }

    void OnDestroy()
    {
        Managers.Event.RemoveEvent(EEventType.UpdateEnergy, OnEvent_UpdateEnergy);
    }

    public void OnEvent_UpdateEnergy(Component sender, object param)
    {
        Debug.Log("OnEvent_UpdateEnergy!!!"); 

        Managers.WebContents.ReqDtoUpdateEnergy(new ReqDtoUpdateEnergy()
        {
            UserAccountId = Managers.Game.UserInfo.UserAccountId
        },
        (response) =>
        {
            Debug.Log("OnEvent_UpdateEnergy" + Managers.Game.UserInfo.LatelyEnergy);
            Managers.Game.UserInfo.Energy = response.Energy;
            Managers.Game.UserInfo.LatelyEnergy = response.LatelyEnergy;
            Managers.Event.TriggerEvent(EEventType.UIRefresh);
        },
        (errorCode) =>
        {
            Managers.UI.ShowPopupUI<UI_ErrorPopup>();
            ErrorStruct errorStruct = Managers.Error.GetError(EErrorCode.ERR_NetworkSaveError);
            Managers.Event.TriggerEvent(EEventType.ToastPopupNotice, this, errorStruct);
        }
        );
    }
}
