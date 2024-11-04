using UnityEngine;

public class SignInScene  : BaseScene
{
    public override bool Init()
    {
        if (base.Init() == false)
        {
            return false;
        }

        Managers.UI.ShowSceneUI<UI_SignInScene>();

        return true;
    }
}
