using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputNameScene : BaseScene
{
    public override bool Init()
    {
        if (base.Init() == false)
        {
            return false;
        }

        Managers.UI.ShowSceneUI<UI_InputNameScene>();

        return true;
    }
}
