using System;
using GameApi.Dtos;
using UnityEngine;
using UnityEngine.EventSystems;
using static Define;

public class UI_ChooseCharacterScene : UI_Scene
{
    private enum GameObjects
    {
        UI_ChooseCharacterPanel,
        UI_EvolutionPanel
    }
    private enum Buttons
    {
        Custom_Button,
        Home_Button,
        Evolution_Button
    }

    public override bool Init()
    {
        if (base.Init() == false)
        {
            return false;
        }
        BindObjects(typeof(GameObjects));
        BindButtons(typeof(Buttons));

        GetButton((int)Buttons.Custom_Button).gameObject.BindEvent(OnClick_CustomButton, EUIEvent.Click);
        GetButton((int)Buttons.Home_Button).gameObject.BindEvent(OnClick_HomeButton, EUIEvent.Click);
        GetButton((int)Buttons.Evolution_Button).gameObject.BindEvent(OnClick_EvolutionButton, EUIEvent.Click);

        return true;
    }

    private void OnClick_CustomButton(PointerEventData eventData)
    {
        Debug.Log("OnClick_CustomButton!");
        GetObject((int)GameObjects.UI_ChooseCharacterPanel).SetActive(true);
        GetObject((int)GameObjects.UI_EvolutionPanel).SetActive(false);
    }
    private void OnClick_HomeButton(PointerEventData eventData)
    {
        Debug.Log("OnClick_HomeButton!");
        ChangeStyle();
    }
    private void OnClick_EvolutionButton(PointerEventData eventData)
    {
        Debug.Log("OnClick_EvolutionButton!");
        GetObject((int)GameObjects.UI_ChooseCharacterPanel).SetActive(false);
        GetObject((int)GameObjects.UI_EvolutionPanel).SetActive(true);
        Managers.Event.TriggerEvent(EEventType.Evolution);
    }

    private void ChangeStyle()
    {
        // _scene.ChangeStyle();
        // 스타일 변화 감지
        var result = Managers.Game.ChracterStyleInfo.CheckAppearance();
        if(result)
        {
            Managers.Event.TriggerEvent(EEventType.Purchase, this, 0);
        }
        else
        {
            SaveData();
        }
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
                onSuccess?.Invoke();
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