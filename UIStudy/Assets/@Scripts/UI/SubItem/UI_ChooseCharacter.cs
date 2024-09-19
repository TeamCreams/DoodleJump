using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEditor.PlayerSettings;

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
        _itemRoot = GameObject.Find("InventoryItemRoot");

        foreach (Transform slotObject in _itemRoot.transform)
        {
            Destroy(slotObject.gameObject);
        }
    }

    private void OnClick_HairItem(PointerEventData eventData)
    {
        AllPush();


        for (int id = 10001; id <= 10015; id++)
        {
            SpawnItem(id);
            // var slot = Managers.UI.MakeSubItem<UI_InventoryItem>(null, GetObject((int)GameObjects.InventoryItemRoot).transform); //pooling ㅅㅏ용 
        }
    }
    private void OnClick_EyesItem(PointerEventData eventData)
    {
        AllPush();
        for (int id = 11001; id <= 11014; id++)
        {
            SpawnItem(id);

        }

    }
    private void OnClick_EyebrowsItem(PointerEventData eventData)
    {
        AllPush();
        for (int id = 12001; id <= 12014; id++)
        {
            SpawnItem(id);
        }
    }

    private void AllPush()
    {
        foreach(var _item in _itemList)
        {
            _item.GetComponent<UI_InventoryItem>().CloseItem();
        }
        _itemList.Clear();
    }

    private void SpawnItem(int id)
    {
        var item = Managers.Resource.Instantiate("UI_InventoryItem", GetObject((int)GameObjects.InventoryItemRoot).transform, pooling: true);
        item.GetOrAddComponent<UI_InventoryItem>().SetInfo(id);
        if (item.transform.parent != _itemRoot.transform)
        {
            item.transform.SetParent(GetObject((int)GameObjects.InventoryItemRoot).transform, false);
            item.GetComponent<UI_InventoryItem>().MyParent = _itemRoot;
        }
        _itemList.Add(item);
    }



}