using System;
using System.Collections;
using GameApi.Dtos;
using UnityEngine;
using UnityEngine.UI;
using static Define;

public class UI_SuberunkerScene : UI_Scene
{
    public override bool Init()
    {
        if (base.Init() == false)
        {
            return false;
        }
        Managers.UI.ShowUIBase<UI_TopBar>();
        return true;
    }

    #region Events
    #endregion
    
    #region Interface
    #endregion
}
