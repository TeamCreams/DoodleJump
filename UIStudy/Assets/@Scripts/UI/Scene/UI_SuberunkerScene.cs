using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

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

    protected override void Init()
    {
        base.Init();
    
        StartLoadAssets();

        //Managers.Game.OnChangedLife -= RefreshUI;
        //Managers.Game.OnChangedLife += RefreshUI;
    }

    void StartLoadAssets()
    {
        Managers.Resource.LoadAllAsync<UnityEngine.Object>("PreLoad", (key, count, totalCount) =>
        {
            Debug.Log($"{key} {count}/{totalCount}");

            if (count == totalCount)
            {
                Debug.Log("Load Complete");
                Managers.Data.Init();
            }
        });
    }
}
