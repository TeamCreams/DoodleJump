using System;
using Assets.HeroEditor.InventorySystem.Scripts.Data;
using Data;
using GameApi.Dtos;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static Define;

public class UI_PurchasePopup : UI_PurchasePopupBase
{
    private enum GameObjects
    {
        Noctice_ImageGroup,
    }
    private enum Images
    {
        Close_Button,
        Ok_Button
    }

    private enum Texts
    {
        Title_Text,
        Notice_Text,
        Gold_Text,
        Ok_Text 
    }
    private EvolutionData _item;
    private PurchaseStruct _purchaseStruct;

    private int _title = 0; // 이거 언어랑 버전 별로 만들어서 수정해야 함
    private int _notice = 0;
    public override bool Init()
    {
        if (base.Init() == false)
        {
            return false;
        }
        //bind
        BindObjects(typeof(GameObjects));
        //BindButtons(typeof(Buttons));
        BindTexts(typeof(Texts));
        BindImages(typeof(Images));
        BindCommonEvents();

        //get
        //GetButton((int)Buttons.Close_Button).gameObject.BindEvent(OnEvent_ClickClose, EUIEvent.Click);
        //GetButton((int)Buttons.Ok_Button).gameObject.BindEvent(OnEvent_ClickOk, EUIEvent.Click);
        GetObject((int)GameObjects.Noctice_ImageGroup).SetActive(false);

        //add Event
        Managers.Event.AddEvent(EEventType.SetLanguage, OnEvent_SetLanguage);
        return true;
    }
    protected override void OnDestroy()
    {
        base.OnDestroy();
        Managers.Event.RemoveEvent(EEventType.SetLanguage, OnEvent_SetLanguage);
    }
    public void SetInfo(PurchaseStruct purchaseStruct)
    {
        _purchaseStruct = purchaseStruct;

        switch(_purchaseStruct.ProductType)
        {
            case EProductType.Custom:  
            {
                UpdateCharacterStyle();
                _title = 91047;
                _notice = 91049;
                GetText((int)Texts.Gold_Text).text = HardCoding.ChangeStyleGold.ToString();
                _gold = HardCoding.ChangeStyleGold;
            } 
            break;
            case EProductType.Evolution:
            {
                _title = 91048;
                _notice = 91050;
                _item = Managers.Data.EvolutionDataDic[_purchaseStruct.Id];
                GetText((int)Texts.Gold_Text).text = _item.Gold.ToString();
                _gold = _item.Gold;
            }
            break;
            default:
            break;
        }
        OnEvent_SetLanguage(null, null);
    }

    protected override void OnClick_ClosePopup(PointerEventData eventData)
    {
        base.OnClick_ClosePopup(eventData);
        _purchaseStruct.OnClose?.Invoke();
    }

    protected override void OnClick_ClickOk(PointerEventData eventData)
    {
        Managers.Game.GoldTochange = 0;
        int remainingChange = Managers.Game.UserInfo.Gold - _gold;
        if(0 <= remainingChange)
        {
            Managers.Game.GoldTochange = remainingChange;
            UpdateUserGold(() => {
                _purchaseStruct.OnOkay?.Invoke();
            });
        }
        else
        {
            ShowGoldInsufficientError();
        }
    }

    protected override void AfterPurchaseProcess()
    {
        switch (_purchaseStruct.ProductType)
        {
            case EProductType.Custom:
            {
                Managers.Game.ChracterStyleInfo.IsChangedStyle = 1;
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
    }

    private void UpdateCharacterStyle()
    {
        GetObject((int)GameObjects.Noctice_ImageGroup).SetActive(true);

        if (Managers.Game.ChracterStyleInfo.Hair != Managers.Game.ChracterStyleInfo.TempHair)
        {
            SpawnItem(EEquipType.Hair);
        }

        if (Managers.Game.ChracterStyleInfo.Eyes != Managers.Game.ChracterStyleInfo.TempEyes)
        {
            SpawnItem(EEquipType.Eyes);
        }

        if (Managers.Game.ChracterStyleInfo.Eyebrows != Managers.Game.ChracterStyleInfo.TempEyebrows)
        {
            SpawnItem(EEquipType.Eyebrows);
        }

        if (Managers.Game.ChracterStyleInfo.CharacterId != Managers.Game.ChracterStyleInfo.TempCharacterId)
        {
            var item = Managers.UI.MakeSubItem<UI_CharacterStyleItemText>(parent: GetObject((int)GameObjects.Noctice_ImageGroup).transform, pooling: true);
            item.SetInfo(Managers.Game.ChracterStyleInfo.CharacterId, Managers.Game.ChracterStyleInfo.TempCharacterId);
        }
    }
    private void SpawnItem(EEquipType style)
    {
        var item = Managers.UI.MakeSubItem<UI_CharacterStyleItem>(parent: GetObject((int)GameObjects.Noctice_ImageGroup).transform, pooling: true);
        item.SetInfo(style);
        //_itemList.Add(item.gameObject);
    }

    void OnEvent_SetLanguage(Component sender, object param)
    {
        GetText((int)Texts.Title_Text).text = Managers.Language.LocalizedString(_title);
        GetText((int)Texts.Notice_Text).text = Managers.Language.LocalizedString(_notice);
        GetText((int)Texts.Ok_Text).text = Managers.Language.LocalizedString(91052);
    }
}
