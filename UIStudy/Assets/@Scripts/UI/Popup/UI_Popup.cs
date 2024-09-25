using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Popup : UI_Base
{
    enum Images
    {
        Image_Test
    }

    public override bool Init()
    {
        if (base.Init() == false)
        {
            return false;
        }
        return true;

        /*
        BindImages(typeof(Images));
        GetImage((int)Images.Image_Test).color = 
            new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f));
        GetImage((int)Images.Image_Test).gameObject.BindEvent((evt) =>
        {
            Managers.UI.ClosePopupUI(this);
        }, Define.EUIEvent.Click);
        */
    }

    public virtual void SetOrder(int sortOrder)
    {
        this.GetComponent<Canvas>().sortingOrder = sortOrder;
    }
}
