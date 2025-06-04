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

    private enum Texts
    {
        Custom_Text,
        Home_Text,
        Evolution_Text
    }
    private ChooseCharacterScene _scene;
    private int _prevEvolutionId = 0;
    public override bool Init()
    {
        if (base.Init() == false)
        {
            return false;
        }
        // Bind
        BindObjects(typeof(GameObjects));
        BindButtons(typeof(Buttons));
        BindTexts(typeof(Texts));

        // Get
        GetButton((int)Buttons.Custom_Button).gameObject.BindEvent(OnClick_CustomButton, EUIEvent.Click);
        GetButton((int)Buttons.Home_Button).gameObject.BindEvent(OnClick_HomeButton, EUIEvent.Click);
        GetButton((int)Buttons.Evolution_Button).gameObject.BindEvent(OnClick_EvolutionButton, EUIEvent.Click);
        _prevEvolutionId = Managers.Game.UserInfo.EvolutionId;

        // add event
        Managers.Event.AddEvent(EEventType.SetLanguage, OnEvent_SetLanguage);

        _scene = Managers.Scene.CurrentScene as ChooseCharacterScene;

        return true;
    }

    private void OnDestroy()
    {
        Managers.Event.RemoveEvent(EEventType.SetLanguage, OnEvent_SetLanguage);
    }
    
    public void SetInfo(ChooseCharacterScene scene)
    {
        _scene = scene;
    }

    private void OnClick_CustomButton(PointerEventData eventData)
    {
        GetObject((int)GameObjects.UI_ChooseCharacterPanel).SetActive(true);
        GetObject((int)GameObjects.UI_EvolutionPanel).SetActive(false);
    }
    private void OnClick_HomeButton(PointerEventData eventData)
    {
        ChangeStyle();
    }
    private void OnClick_EvolutionButton(PointerEventData eventData)
    {
        GetObject((int)GameObjects.UI_ChooseCharacterPanel).SetActive(false);
        GetObject((int)GameObjects.UI_EvolutionPanel).SetActive(true);
        Managers.Event.TriggerEvent(EEventType.Evolution);
    }
    private void ChangeStyle()
    {
        // _scene.ChangeStyle();
        // 스타일 변화 감지
        var result = Managers.Game.ChracterStyleInfo.CheckAppearance();
        if (result)
        {
            PurchaseStruct purchaseStruct = new PurchaseStruct(0, EProductType.Custom, 
                () => 
                {
                    Managers.Game.ChracterStyleInfo.UpdateValuesFromTemp();
                    _scene.SaveData();
                }, 
                () => 
                {
                    Managers.Game.ChracterStyleInfo.ResetValuesFromTemp();
                    Managers.Event.TriggerEvent(EEventType.SetStyle_Player);
                    _scene.SaveData();
                });
            Managers.Event.TriggerEvent(EEventType.Purchase, this, purchaseStruct);
            return;
        }
        // 스타일 변화도 없는데 진화구매를 하지 않았으면 저장할 필요도 없음
        if (_prevEvolutionId != Managers.Game.UserInfo.EvolutionId)
        {
            _scene.SaveData();
            return;
        }
        else
        {
            _scene.LoadHomeScene();
        }
    }

    void OnEvent_SetLanguage(Component sender, object param)
    {
        GetText((int)Texts.Custom_Text).text = Managers.Language.LocalizedString(91051);
        GetText((int)Texts.Home_Text).text = Managers.Language.LocalizedString(91017);
        GetText((int)Texts.Evolution_Text).text = Managers.Language.LocalizedString(91053);
    }
}