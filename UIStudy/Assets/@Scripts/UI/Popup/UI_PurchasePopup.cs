using System;
using Assets.HeroEditor.InventorySystem.Scripts.Data;
using Data;
using GameApi.Dtos;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static Define;

public class UI_PurchasePopup : UI_Popup
{
    private enum GameObjects
    {
        Noctice_ImageGroup,
    }
    private enum Buttons
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
    private int _gold = 0;

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
        BindButtons(typeof(Buttons));
        BindTexts(typeof(Texts));

        //get
        GetButton((int)Buttons.Close_Button).gameObject.BindEvent(OnEvent_ClickClose, EUIEvent.Click);
        GetButton((int)Buttons.Ok_Button).gameObject.BindEvent(OnEvent_ClickOk, EUIEvent.Click);
        GetObject((int)GameObjects.Noctice_ImageGroup).SetActive(false);

        //add Event
        Managers.Event.AddEvent(EEventType.SetLanguage, OnEvent_SetLanguage);
        return true;
    }
    private void OnDestroy()
    {
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

    private void OnEvent_ClickClose(PointerEventData eventData)
    {
        _purchaseStruct.OnClose?.Invoke();
        Managers.UI.ClosePopupUI(this);
    }

    private void OnEvent_ClickOk(PointerEventData eventData)
    {
        Managers.Game.RemainingChange = 0;
        int remainingChange = Managers.Game.UserInfo.Gold - _gold;
        if(0 <= remainingChange)
        {
            Managers.Game.RemainingChange = remainingChange;
            UpdateUserGold();
            // call event
        }
        else
        {
            UI_ToastPopup.ShowError(Managers.Error.GetError(EErrorCode.ERR_GoldInsufficient));
        }
    }

    private void UpdateUserGold(Action onSuccess = null, Action onFailed = null)
    {
        Managers.WebContents.ReqDtoUpdateUserGold(new ReqDtoUpdateUserGold()
        {
            UserAccountId = Managers.Game.UserInfo.UserAccountId,
            Gold = _gold
        },
       (response) =>
       {
            onSuccess?.Invoke();
            _purchaseStruct.OnOkay?.Invoke();
            Managers.Event.TriggerEvent(EEventType.UpdateGold);
            switch(_purchaseStruct.ProductType)
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
       },
       (errorCode) =>
        {
            // UI_ErrorButtonPopup popup = Managers.UI.ShowPopupUI<UI_ErrorButtonPopup>();
            // ErrorStruct errorStruct = Managers.Error.GetError(EErrorCode.ERR_NetworkSettlementErrorResend);
            // popup.SetInfo(errorStruct.Notice, onFailed, EScene.SuberunkerSceneHomeScene);
            UI_ErrorButtonPopup.ShowErrorButton(Managers.Error.GetError(Define.EErrorCode.ERR_NetworkSettlementErrorResend), onFailed, EScene.SuberunkerSceneHomeScene);
       });
    }

    private void UpdateCharacterStyle()
    {
        GetObject((int)GameObjects.Noctice_ImageGroup).SetActive(true);

        if(Managers.Game.ChracterStyleInfo.Hair != Managers.Game.ChracterStyleInfo.TempHair)
        {
            SpawnItem(EEquipType.Hair);
        }

        if(Managers.Game.ChracterStyleInfo.Eyes != Managers.Game.ChracterStyleInfo.TempEyes)
        {
            SpawnItem(EEquipType.Eyes);
        }

        if(Managers.Game.ChracterStyleInfo.Eyebrows != Managers.Game.ChracterStyleInfo.TempEyebrows)
        {
            SpawnItem(EEquipType.Eyebrows);
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
