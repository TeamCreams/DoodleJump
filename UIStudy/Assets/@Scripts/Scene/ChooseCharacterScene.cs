using System;
using static Define;
using UnityEngine;
using GameApi.Dtos;

public class ChooseCharacterScene : BaseScene
{
    public override bool Init()
    {
        if (base.Init() == false)
        {
            return false;
        }
        // ui생성 및 스크립트 정보 넘기기
        var ui = Managers.UI.ShowSceneUI<UI_ChooseCharacterScene>();
        ui.SetInfo(this);

        // bgm 설정
        Managers.Sound.Stop(ESound.Bgm);
        Managers.Sound.Play(ESound.Bgm, "LobbyBGMSound", 0.6f);

        // event 추가    
        Managers.Event.RemoveEvent(EEventType.Purchase, OnEvent_ShowPurchasePopup);
        Managers.Event.AddEvent(EEventType.Purchase, OnEvent_ShowPurchasePopup);

        return true;
    }

    void OnDestroy()
    {
        Managers.Event.RemoveEvent(EEventType.Purchase, OnEvent_ShowPurchasePopup);
    }

    public void OnEvent_ShowPurchasePopup(Component sender = null, object param = null)
    {
        PurchaseStruct purchaseStructstruct = (PurchaseStruct)param;
        UI_PurchasePopup purchase = Managers.UI.ShowPopupUI<UI_PurchasePopup>();
        purchase.SetInfo(purchaseStructstruct);
        // if(id == 0)
        // {
        //     purchase.SetInfo(id, EProductType.Custom);
        // }
        // else
        // {            
        //     purchase.SetInfo(id, EProductType.Evolution);
        // }
    }

    public void SaveData(Action onSuccess = null, Action onFailed = null)
    {
        Managers.WebContents.UpdateUserStyle(new ReqDtoUpdateUserStyle()
        {
            UserAccountId = Managers.Game.UserInfo.UserAccountId,
            CharacterId = Managers.Game.ChracterStyleInfo.CharacterId,
            HairStyle = Managers.Game.ChracterStyleInfo.Hair,
            EyebrowStyle = Managers.Game.ChracterStyleInfo.Eyebrows,
            EyesStyle = Managers.Game.ChracterStyleInfo.Eyes,
            Evolution = Managers.Game.UserInfo.EvolutionId,
            EvolutionSetLevel = Managers.Game.UserInfo.EvolutionSetLevel
        },
        (response) =>
        {
                onSuccess?.Invoke();
                
        },
        (errorCode) =>
        {
                onFailed?.Invoke();
        });
    }

    public void LoadHomeScene()
    {
        Managers.Scene.LoadScene(EScene.SuberunkerSceneHomeScene);
    }
}

