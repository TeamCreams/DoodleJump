using UnityEngine;

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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            Systems.GoogleLoginWebView.SignIn();
        }
    }
}
