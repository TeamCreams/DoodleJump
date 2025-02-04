using System.Collections;
using Data;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static Define;

public class UI_EvolutionItem : UI_Base
{
    private enum GameObjects
    {
        Selected
    }
    private enum Images
    {
        isClick,
        Icon
    }
    private Toggle _toggle = null;
    private int _itemId = 0;

    public override bool Init()
    {
        if (base.Init() == false)
        {
            return false;
        }
        BindObjects(typeof(GameObjects));
        BindImages(typeof(Images));

        _toggle = this.gameObject.GetComponent<Toggle>();
        _toggle.onValueChanged.AddListener(OnClick_IsClickItem);
        return true;
    }

    private void OnClick_IsClickItem(bool isOn)
    {
        GetObject((int)GameObjects.Selected).SetActive(isOn);
        if(isOn == false)
        {
            return;
        }
        // 서버연결    
        
        Debug.Log($"Item Id : {_itemId}");
        if(Managers.Data.EvolutionDataDic[_itemId].PrevEvolutionId != Managers.Game.UserInfo.EvolutionId)
        {
            return;
        }
        Managers.Event.TriggerEvent(EEventType.Purchase, this, _itemId);
    }
    public void SetIcon(int id)
    {
        _itemId = id;
        EvolutionData evolutionData = Managers.Data.EvolutionDataDic[_itemId];
        //_itemId = Managers.Game.UserInfo.EvolutionId;
        string str = "";
        switch(evolutionData.Item)
        {
            case EItemType.Boots:
                str = $"{evolutionData.ItemSprite}BootsIcon.sprite";
            break;
            case EItemType.Armor:
                str = $"{evolutionData.ItemSprite}Icon.sprite";
            break;
            case EItemType.Mask:
                str = $"{evolutionData.ItemSprite}Icon.sprite";
            break;
        }
        var sprite = Managers.Resource.Load<Sprite>(str);
        GetImage((int)Images.Icon).sprite = sprite;
    }
}
