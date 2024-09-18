using System.Collections;
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
        GameObject foundObject = GameObject.Find("InventoryItemRoot");
        _toggle.group = foundObject.GetComponent<ToggleGroup>(); // 부모가 poolingRoot로 되어있음

        //toggle.group = this.transform.parent.gameObject.GetComponent<ToggleGroup>(); // 부모가 poolingRoot로 되어있음
    }

    public void SetInfo(int templateId)
    {
        Data = Managers.Data.CharacterItemSpriteDic[templateId];
        string spriteName = Data.SpriteName;
        GetImage((int)Images.Icon).sprite = Managers.Resource.Load<Sprite>($"{spriteName}Icon.sprite");
    }

    public void OnClick_SetCharacter(PointerEventData eventData)
    {
        Debug.Log("jijihihihihihihihi");
        _toggle.isOn = true;
        if (Data.Id < (int)State.Eyebrows)
        {
            Managers.Game.ChracterStyleInfo.Hair = Data.SpriteName;
        }
        else if (Data.Id < (int)State.Eyes)
        {
            Managers.Game.ChracterStyleInfo.Eyebrows = Data.SpriteName;
        }
        else if (Data.Id < (int)State.None)
        {
            Managers.Game.ChracterStyleInfo.Eyes = Data.SpriteName;
        }

        Managers.Event.TriggerEvent(EEventType.SetStyle_Player, this);

    }


    public void CloseItem()
    {
        Managers.Pool.Push(this.gameObject);
    }

}
