﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using static Define;
using static UnityEditor.PlayerSettings;
using System.Linq;

public class UI_ChooseCharacter : UI_Base
{

    private List<GameObject> _itemList = new List<GameObject>();
    private GameObject _itemRoot = null;
    enum GameObjects
    {
        InventoryItemRoot,
    }
    enum Images
    {
        HairItem,
        EyesItem,
        EyebrowsItem,
    }

    protected override void Init()
    {
        base.Init();
        BindObjects(typeof(GameObjects));
        BindImages(typeof(Images));
        GetImage((int)Images.HairItem).gameObject.BindEvent(OnClick_HairItem, Define.EUIEvent.Click);
        GetImage((int)Images.EyesItem).gameObject.BindEvent(OnClick_EyesItem, Define.EUIEvent.Click);
        GetImage((int)Images.EyebrowsItem).gameObject.BindEvent(OnClick_EyebrowsItem, Define.EUIEvent.Click);
        _itemRoot = GetObject((int)GameObjects.InventoryItemRoot);

        foreach (Transform slotObject in _itemRoot.transform)
        {
            Managers.Resource.Destroy(slotObject.gameObject);
        }
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
        var item = Managers.UI.MakeSubItem<UI_InventoryItem>(parent: GetObject((int)GameObjects.InventoryItemRoot).transform, pooling: true);
        item.SetInfo(id);
        _itemList.Add(item.gameObject);
    }



}