using UnityEngine;
using System.Collections;
using System.Numerics;
using System.Collections.Generic;
using TMPro;

public class UI_InventorySlot : UI_Base
{
    private ItemData _data = null;
    enum Images
    {
        Rare,
        Parts,
        Icon,
        Click
    }

    enum Texts
    {
        Level
    }

    private Color[] RareColors 
        = new Color[6] { Color.white, Color.yellow, Color.green, Color.blue, Color.red, Color.magenta };
    private Color[] PartsColors = new Color[6] {
        Color.white,
        new Color(1, 0.81f, 0.49f),
        new Color(0.56f, 1, 0.49f),
        new Color(0.49f, 0.71f,1),
        new Color(1, 0.49f, 0.55f),
        new Color(0.79f, 0.49f,1) 
    };


    private string _icon;
    private int _rare;
    private int _parts;
    private int _level;
    public override bool Init()
    {
        if (base.Init() == false)
        {
            return false;
        }
        BindImages(typeof(Images));
        BindTexts(typeof(Texts));
        
        GetImage((int)Images.Click).gameObject.BindEvent((evt) =>
        {
            //Managers.UI.ShowPopupUI<UI_Popup>();
        }, Define.EUIEvent.Click);
        return true;
    }

    public void SetInfo(ItemData data)
    {
        _data = data;
        Refresh();
    }

    void Refresh()
    {
        if(_data == null)
        {
            return;
        }
        
        _icon = _data.Icon;
        GetImage((int)Images.Icon).sprite = Resources.Load<Sprite>($"Items/{_icon}");
        _rare = _data.Rare;
        GetImage((int)Images.Rare).color
            = RareColors[_rare];
        _parts = _data.Parts;
        GetImage((int)Images.Parts).color
            = PartsColors[_parts];
        _level = _data.Level;
        
        GetText((int)Texts.Level).text = _data.Level.ToString();
    }

}

