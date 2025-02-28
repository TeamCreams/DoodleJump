using UnityEngine;

public class SignInScene  : BaseScene
{
    public override bool Init()
    {
        if (base.Init() == false)
        {
            return false;
        }

        var ui = Managers.UI.ShowSceneUI<UI_SignInScene>();
        ui.SetInfo(this);
        return true;
    }
    
}
