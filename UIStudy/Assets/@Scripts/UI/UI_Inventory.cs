using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Inventory : UI_Base
{
    public enum GameObjects
    {
        UI_SubItemRoot
    }
    public enum Texts
    {

    }
    public enum Buttons
    {

    }
    GameObject _subItemRoot = null;
    bool _isSubItemRootActive = false;

    private List<ItemData> _itemList;
    protected override void Init()
    {
        base.Init();
        _itemList = Managers.Game.Items;
        BindObjects(typeof(GameObjects));

        _subItemRoot = GetObject((int)GameObjects.UI_SubItemRoot);

        foreach(Transform slotObject in _subItemRoot.transform)
        {
            Destroy(slotObject.gameObject);
        }

        for (int i = 0; i < _itemList.Count; i++)
        {
            var slot = Managers.UI.MakeSubItem<UI_InventorySlot>(null, _subItemRoot.transform);
            //item.GeneratorIcon(i);
            slot.SetInfo(_itemList[i]);
        }

    }

}