﻿using System.Collections.Generic;
using UnityEngine;
using static Define;

public class UI_EvolutionPanel : UI_Base
{
    private enum GameObjects
    {
        EvolutionRoot,

    }
    private Transform _evolutionRoot = null;
    private List<GameObject> _itemList = new List<GameObject>();

    public override bool Init()
    {
        if (base.Init() == false)
        {
            return false;
        }
        BindObjects(typeof(GameObjects));
        _evolutionRoot = GetObject((int)GameObjects.EvolutionRoot).transform;
        Managers.Event.AddEvent(EEventType.Evolution, SetInventoryItems);

        return true;
    }
    private void OnDestroy()
    {
        Managers.Event.RemoveEvent(EEventType.Evolution, SetInventoryItems);
    }
    private void SetInventoryItems(Component sender = null, object param = null)
    {
        AllPush();
        var list = Managers.Data.EvolutionItemDataDic;
        foreach (var item in list)
        { 
            SpawnItem(item.Value.Id);
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
        var item = Managers.UI.MakeSubItem<UI_EvolutionItemSet>(parent: _evolutionRoot, pooling: true); //GetObject((int)GameObjects.InventoryItemRoot).transform
        item.SetInfo(id);
        _itemList.Add(item.gameObject);
    }
    
    
}
