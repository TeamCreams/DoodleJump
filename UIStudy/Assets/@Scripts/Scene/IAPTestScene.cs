using UnityEngine;
using UnityEngine.EventSystems;

public class IAPTestScene : BaseScene
{

    public override bool Init()
    {
        if (base.Init() == false)
        {
            return false;
        }

        return true;
    }


    public override void Clear()
    {
    }
}
