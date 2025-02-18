using System;
using System.Collections;
using GameApi.Dtos;
using UnityEngine;
using UnityEngine.UI;
using static Define;

public class UI_SuberunkerScene : UI_Scene
{

    enum GameObjects
    {
    
    }

    enum Texts
    { 
    
    }

    enum Buttons
    {
    
    }

    public override bool Init()
    {
        if (base.Init() == false)
        {
            return false;
        }
        return true;
    }

    #region Events
    #endregion
    
    #region Interface
    private void ExitGame()
    {
        Managers.Scene.LoadScene(EScene.SuberunkerSceneHomeScene);
    }
    #endregion
}
