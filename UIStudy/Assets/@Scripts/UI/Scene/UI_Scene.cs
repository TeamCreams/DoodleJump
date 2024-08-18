using UnityEngine;
using System.Collections;

public class UI_Scene : UI_Base
{
    protected UI_EventHandler _eventHandler;

    protected override void Init()
    {
        base.Init();

        this.gameObject.GetOrAddComponent<UI_EventHandler>();
    }
}

