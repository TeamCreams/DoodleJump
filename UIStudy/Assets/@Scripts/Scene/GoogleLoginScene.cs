using UnityEngine;
using UnityEngine.EventSystems;

public class GoogleLoginScene : BaseScene
{

    public override bool Init()
    {
        if (base.Init() == false)
        {
            return false;
        }

        return true;
    }

    public void SignInGoogle()
    {
        Systems.GoogleLoginWebView.SignIn();
    }


    public override void Clear()
    {
    }
}
