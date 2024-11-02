using UnityEngine;

public class SingUpScene : BaseScene
{
    public override bool Init()
    {
        if (base.Init() == false)
        {
            return false;
        }

        Managers.UI.ShowSceneUI<UI_SingUpScene>();

        return true;
    }
}
