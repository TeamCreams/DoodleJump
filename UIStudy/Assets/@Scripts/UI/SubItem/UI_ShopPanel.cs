using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using static Define;

public class UI_ShopPanel : UI_Popup
{
    private enum Images
    {
        Close_Button,
    }
    private enum Texts
    {
        Shop_Text
    }
    private enum GameObjects
    {
        Items,
        Content
    }
    private Transform _itemRoot = null;
    private Transform _contentRoot = null;

    private List<GameObject> _itemList = new List<GameObject>();
    public override bool Init()
    {
        if (base.Init() == false)
        {
            return false;
        }
        //Bind
        BindImages(typeof(Images));
        BindTexts(typeof(Texts));
        BindObjects(typeof(GameObjects));
        //Get
        GetImage((int)Images.Close_Button).gameObject.BindEvent(OnClick_ClosePopup, EUIEvent.Click);

        //Event
        Managers.Event.AddEvent(EEventType.EnterShop, SetMerchandiseItems);
        _contentRoot = GetObject((int)GameObjects.Content).transform;
        _itemRoot = GetObject((int)GameObjects.Items).transform;
        return true;
    }
    void OnDestroy()
    {
        Managers.Event.RemoveEvent(EEventType.EnterShop, SetMerchandiseItems);
    }
    private void OnClick_ClosePopup(PointerEventData eventData)
    {
        Managers.UI.ClosePopupUI(this);
    }
    private void SetMerchandiseItems(Component sender = null, object param = null)
    {
        AllPush();

        var loginReward = Managers.UI.MakeSubItem<UI_LoginReward>(parent: _contentRoot);
        loginReward.SetInfo();

        var list = Managers.Data.CashItemDataDic;
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
        var item = Managers.UI.MakeSubItem<UI_Merchandise>(parent: _itemRoot, pooling: true);
        item.SetInfo(id);
        _itemList.Add(item.gameObject);
    }

}
