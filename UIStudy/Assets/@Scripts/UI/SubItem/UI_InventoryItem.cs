﻿using System.Collections;
using System.Collections.Generic;
using Data;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static Define;

public class UI_InventoryItem : UI_Base
{
    private Toggle _toggle = null;
    private GameObject _myParent = null;
    public GameObject MyParent
    {
        get =>_myParent;
        
        set
        {
            _myParent = value;
        }

    }

    
    private enum State
    {
        Hair = 10000,
        Eyebrows = 11000,
        Eyes = 12000,
        None = 13000,
    }
  
    private State _state = State.Eyebrows;
    private CharacterItemSpriteData _data;
    public CharacterItemSpriteData Data
    {
        get => _data;
        private set
        {
            _data = value;
        }
    }

    enum Images
    {
        Icon
    }

    protected override void Init()
    {
        base.Init();
        BindImages(typeof(Images));
        this.gameObject.BindEvent(OnClick_SetCharacter, Define.EUIEvent.Click);
        _toggle = this.gameObject.GetComponent<Toggle>();
    }

    public void SetInfo(int templateId)
    {
        Data = Managers.Data.CharacterItemSpriteDic[templateId];
        string spriteName = Data.SpriteName;
        GetImage((int)Images.Icon).sprite = Managers.Resource.Load<Sprite>($"{spriteName}Icon.sprite");
        _toggle.group = this.transform.parent.gameObject.GetComponent<ToggleGroup>(); // 부모가 poolingRoot로 되어있음
    }

    public void OnClick_SetCharacter(PointerEventData eventData)
    {
        _toggle.isOn = true;

        switch (Data.EquipType)
        {
            case EEquipType.Hair:
                Managers.Game.ChracterStyleInfo.Hair = Data.SpriteName;
                break;
            case EEquipType.Eyebrows:
                Managers.Game.ChracterStyleInfo.Eyebrows = Data.SpriteName;
                break;
            case EEquipType.Eyes:
                Managers.Game.ChracterStyleInfo.Eyes = Data.SpriteName;
                break;
            case EEquipType.None:
                //에러 팝업
                break;
        }

        Managers.Event.TriggerEvent(EEventType.SetStyle_Player, this);

    }



}