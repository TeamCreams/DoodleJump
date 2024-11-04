using UnityEngine;

public class SignUpScene : BaseScene
{
    public override bool Init()
    {
        if (base.Init() == false)
        {
            return false;
        }

        Managers.UI.ShowSceneUI<UI_SignUpScene>();

        return true;
    }
}
