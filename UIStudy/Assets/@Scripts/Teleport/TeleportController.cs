using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportController : ObjectBase
{
    private bool _isTeleportable = true;
    public bool IsTeleportable
    {
        get => _isTeleportable;
        set
        {
            _isTeleportable = value;
        }
    }

    public override bool Init()
    {
        if (base.Init() == false)
        {
            return false;
        }
        return true;
    }


}
