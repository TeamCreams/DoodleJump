using System.Collections;
using System.Collections.Generic;
using Assets.HeroEditor.Common.Scripts.Common;
using Data;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static Define;

public class UI_InventoryItem : UI_Base
{
    private enum State
    {
        Hair = 10000,
        Eyebrows = 11000,
        Eyes = 12000,
        None = 13000,
    }

    enum Images
    {
        Icon,
        Frame,
    }

    private Toggle _toggle = null;
  
    private CharacterItemSpriteData _data;
    public CharacterItemSpriteData Data
    {
        get => _data;
        private set
        {
            _data = value;
        }
    }
    private bool _isScrolling = false;

    private ScrollRect _parentScrollRect = null;

    public override bool Init()
    {
        if (base.Init() == false)
        {
            return false;
        }
        BindImages(typeof(Images));
        GetImage((int)Images.Frame).gameObject.BindEvent(OnClick_SetCharacter, EUIEvent.Click);
        GetImage((int)Images.Frame).gameObject.BindEvent(OnBeginDrag, EUIEvent.BeginDrag);
        GetImage((int)Images.Frame).gameObject.BindEvent(OnDrag, EUIEvent.Drag);
        GetImage((int)Images.Frame).gameObject.BindEvent(OnEndDrag, EUIEvent.EndDrag);
        _toggle = this.gameObject.GetComponent<Toggle>();

        return true;
    }

    public void SetInfo(int templateId)
    {
        Data = Managers.Data.CharacterItemSpriteDic[templateId];
        string spriteName = Data.SpriteName;
        GetImage((int)Images.Icon).sprite = Managers.Resource.Load<Sprite>($"{spriteName}Icon.sprite");
        _toggle.group = this.transform.parent.gameObject.GetComponent<ToggleGroup>();
        
        _parentScrollRect = this.transform.GetComponentInParent<ScrollRect>();
        if(_parentScrollRect == null)
        {
            UI_ToastPopup.Show("_parentScrollRect is null", UI_ToastPopup.Type.Debug);
        }
    }
    
    public void OnClick_SetCharacter(PointerEventData eventData)
    {
        if(_isScrolling)
        {
            return;
        }
        _toggle.isOn = true;

        switch (Data.EquipType)
        {
            case EEquipType.Hair:
                Managers.Game.ChracterStyleInfo.TempHair = Data.SpriteName;
                break;
            case EEquipType.Eyebrows:
                Managers.Game.ChracterStyleInfo.TempEyebrows = Data.SpriteName;
                break;
            case EEquipType.Eyes:
                Managers.Game.ChracterStyleInfo.TempEyes = Data.SpriteName;
                break;
            case EEquipType.None:
                //에러 팝업
                break;
        }
        Managers.Event.TriggerEvent(EEventType.SetStyle_Player);
    }

    private void OnBeginDrag(PointerEventData eventData)
	{
        _parentScrollRect.OnBeginDrag(eventData); // 부모한테 이벤트 전달
        //GetImage((int)Images.Frame).raycastTarget = false;
        //Debug.Log("is OnBeginDrag");
        //_isScrolling = true;
    }
    private void OnDrag(PointerEventData eventData)
    {
        _parentScrollRect.OnDrag(eventData);
        //GetImage((int)Images.Frame).raycastTarget = false;
        //Debug.Log("is OnDrag");
        //_isScrolling = true;
    }
    private void OnEndDrag(PointerEventData eventData)
    {
        _parentScrollRect.OnEndDrag(eventData);
        //GetImage((int)Images.Frame).raycastTarget = true;
        //Debug.Log("is OnEndDrag");
        //_isScrolling = false;
    }

}
