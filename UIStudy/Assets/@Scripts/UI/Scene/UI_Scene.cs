using UnityEngine;
using System.Collections;

public class UI_Scene : UI_Base
{
    protected UI_EventHandler _eventHandler;

    public override bool Init()
    {
        if (base.Init() == false)
        {
            return false;
        }

        this.gameObject.GetOrAddComponent<UI_EventHandler>();
        return true;
    }
}

