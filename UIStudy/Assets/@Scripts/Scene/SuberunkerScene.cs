using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

public class SuberunkerScene : BaseScene
{
    private bool _isInput = false;
    public override bool Init()
    {
        if (base.Init() == false)
        {
            return false;
        }

        Managers.UI.ShowSceneUI<UI_SuberunkerScene>();

        Managers.Input.KeyAction -= KeyActionEvent;
        Managers.Input.KeyAction += KeyActionEvent;

        return true;
    }

    void KeyActionEvent()
    {
        if(Input.GetMouseButton(0))
        {
            _isInput = !_isInput;
        }
    }
}
