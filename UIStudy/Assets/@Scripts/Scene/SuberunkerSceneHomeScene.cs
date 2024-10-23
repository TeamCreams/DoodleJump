using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class SuberunkerSceneHomeScene : BaseScene
{
    public override bool Init()
    {
        if (base.Init() == false)
        {
            return false;
        }
        Managers.UI.ShowSceneUI<UI_SuberunkerSceneHomeScene>();


        return true;
    }
}
