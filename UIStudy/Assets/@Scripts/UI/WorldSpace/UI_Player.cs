using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Player : UI_Base
{
    Canvas _worldCanvas;

    public override bool Init()
    {
        if( base.Init() == false)
        {
            return false;
        }

        _worldCanvas = this.GetComponent<Canvas>();
        _worldCanvas.worldCamera = Camera.main;


        return true;
    }
}
