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
    private bool _isLast = false;
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
        // Debug.Log($"Item Id : {_itemId}");
        if(Managers.Data.EvolutionDataDic[_itemId].PrevEvolutionId != Managers.Game.UserInfo.EvolutionId)
        {
            return;
        }

        PurchaseStruct purchaseStruct;
        if(!_isLast)
        {
            purchaseStruct = new PurchaseStruct(_itemId, EProductType.Evolution, null, null);
        }
        else
        {
            purchaseStruct = new PurchaseStruct(_itemId, EProductType.Evolution, () => EvolutionSetLevel(), null);
        }

        Managers.Event.TriggerEvent(EEventType.Purchase, this, purchaseStruct);
    }
    public void SetIcon(int id)
    {
        _itemId = id;
        EvolutionData evolutionData = Managers.Data.EvolutionDataDic[_itemId];
        string str = "";
        switch(evolutionData.StatOption)
        {
            case EStat.MoveSpeed:
                str = $"{evolutionData.ItemSprite}Icon.sprite";
            break;
            case EStat.MaxHp:
                str = $"{evolutionData.ItemSprite}Icon.sprite";
            break;
            case EStat.Luck:
                {
                    str = $"{evolutionData.ItemSprite}Icon.sprite";
                    _isLast = true;
                }
            break;
        }
        var sprite = Managers.Resource.Load<Sprite>(str);
        GetImage((int)Images.Icon).sprite = sprite;
    }
    public void EvolutionSetLevel()
    {
        Managers.Game.UserInfo.EvolutionSetLevel ++;
    }
}
