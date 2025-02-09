using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using GameApi.Dtos;
using static Define;
using System.Linq;
using Data;
using System;

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
        SaveData();
    }
        private void OnClick_EvolutionButton(PointerEventData eventData)
    {
        Debug.Log("OnClick_EvolutionButton!");
        GetObject((int)GameObjects.UI_ChooseCharacterPanel).SetActive(false);
        GetObject((int)GameObjects.UI_EvolutionPanel).SetActive(true);
        Managers.Event.TriggerEvent(EEventType.Evolution);
    }

    public void SaveData(Action onSuccess = null, Action onFailed = null)
    {
        // int hair = StringToInt(EEquipType.Hair);
        // int eyebrows = StringToInt(EEquipType.Eyebrows);
        // int eyes = StringToInt(EEquipType.Eyes);

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
       },
       (errorCode) =>
       {
            UI_ErrorButtonPopup popup = Managers.UI.ShowPopupUI<UI_ErrorButtonPopup>();
            ErrorStruct errorStruct = Managers.Error.GetError(EErrorCode.ERR_NetworkSettlementErrorResend);
            Managers.Event.TriggerEvent(EEventType.ErrorButtonPopup, this, errorStruct.Notice);
            popup.AddOnClickAction(onFailed);
       });
    }

    private int StringToInt(EEquipType eEquipType)
    {
        int answer = 0;
        var equipList = Managers.Data.CharacterItemSpriteDic.Where(cis => cis.Value.EquipType == eEquipType);
        foreach (var characterItemSprite in equipList)
        { 
            if(characterItemSprite.Value.SpriteName != Managers.Game.ChracterStyleInfo.Hair)
            {
                continue;
            }
            answer = characterItemSprite.Key;
        }
        return answer;
    }

/*
    private enum GameObjects
    {
        InventoryItemRoot,
    }

    private enum Images
    {
        HairItem,
        EyesItem,
        EyebrowsItem,
    }
    private enum Buttons
    {
        Custom_Button,
        Home_Button,
        Evolution_Button
    }


    private List<GameObject> _itemList = new List<GameObject>();
    private GameObject _itemRoot = null;

    public override bool Init()
    {
        if (base.Init() == false)
        {
            return false;
        }


        BindObjects(typeof(GameObjects));
        BindImages(typeof(Images));
        BindButtons(typeof(Buttons));
 
        GetImage((int)Images.HairItem).gameObject.BindEvent(OnClick_HairItem, EUIEvent.Click);
        GetImage((int)Images.EyesItem).gameObject.BindEvent(OnClick_EyesItem, EUIEvent.Click);
        GetImage((int)Images.EyebrowsItem).gameObject.BindEvent(OnClick_EyebrowsItem, EUIEvent.Click);

        GetButton((int)Buttons.Custom_Button).gameObject.BindEvent(OnClick_CustomButton, EUIEvent.Click);
        GetButton((int)Buttons.Home_Button).gameObject.BindEvent(OnClick_HomeButton, EUIEvent.Click);
        GetButton((int)Buttons.Evolution_Button).gameObject.BindEvent(OnClick_EvolutionButton, EUIEvent.Click);
        _itemRoot = GetObject((int)GameObjects.InventoryItemRoot);
        Managers.Event.TriggerEvent(EEventType.SetLanguage);

        foreach (Transform slotObject in _itemRoot.transform)
        {
            Managers.Resource.Destroy(slotObject.gameObject);
        }

        //StartLoadAssets("PreLoad");
        SetInventoryItems(EEquipType.Hair);

        return true;
    }

    private void OnClick_HairItem(PointerEventData eventData)
    {
        SetInventoryItems(EEquipType.Hair);
    }

    private void OnClick_EyesItem(PointerEventData eventData)
    {
        SetInventoryItems(EEquipType.Eyes);
    }

    private void OnClick_EyebrowsItem(PointerEventData eventData)
    {
        SetInventoryItems(EEquipType.Eyebrows);
    }

    private void OnClick_CustomButton(PointerEventData eventData)
    {
        Debug.Log("OnClick_CustomButton!");
        Managers.UI.ShowPopupUI<>();
    }
        private void OnClick_HomeButton(PointerEventData eventData)
    {
        Debug.Log("OnClick_HomeButton!");
        Managers.Scene.LoadScene(EScene.SuberunkerSceneHomeScene);
    }
        private void OnClick_EvolutionButton(PointerEventData eventData)
    {
        Debug.Log("OnClick_EvolutionButton!");
        Managers.UI.ShowPopupUI<>();
    }

    private void SetInventoryItems(EEquipType equipType)
    {
        AllPush();
        var equipList = Managers.Data.CharacterItemSpriteDic.Where(cis => cis.Value.EquipType == equipType);
        foreach (var characterItemSprite in equipList)
        { 
            SpawnItem(characterItemSprite.Key);
        }
    }

    private void AllPush()
    {
        foreach(var _item in _itemList)
        {
            Managers.Resource.Destroy(_item.gameObject);
        }
        _itemList.Clear();
    }

    private void SpawnItem(int id)
    {
        var item = Managers.UI.MakeSubItem<UI_InventoryItem>(parent: _itemRoot.transform, pooling: true); //GetObject((int)GameObjects.InventoryItemRoot).transform
        item.SetInfo(id);
        _itemList.Add(item.gameObject);
    }

    void StartLoadAssets(string label)
    {
        Managers.Resource.LoadAllAsync<UnityEngine.Object>(label, (key, count, totalCount) =>
        {
            
            if (count == totalCount)
            {
                Debug.Log("Load Complete");
                Managers.Data.Init();
            }
            
        });
    }
*/
}