using UnityEngine;
using UnityEngine.Playables;

public class StartLoadingScene : BaseScene
{
    public override bool Init()
    {
        if (base.Init() == false)
        {
            return false;
        }
        return true;
    }
}
