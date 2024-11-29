using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using static Define;

public class UI_ChooseCharacterPanel : UI_Base
{
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
 
        GetImage((int)Images.HairItem).gameObject.BindEvent(OnClick_HairItem, EUIEvent.Click);
        GetImage((int)Images.EyesItem).gameObject.BindEvent(OnClick_EyesItem, EUIEvent.Click);
        GetImage((int)Images.EyebrowsItem).gameObject.BindEvent(OnClick_EyebrowsItem, EUIEvent.Click);

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

}
