using GameApi.Dtos;
using UnityEngine;
using static Define;

public class SignUpScene : BaseScene
{
    public override bool Init()
    {
        if (base.Init() == false)
        {
            return false;
        }

        Managers.UI.ShowSceneUI<UI_SignUpScene>();
        Managers.Event.AddEvent(EEventType.GoogleSignup,Event_GoogleAccountSignup);

        return true;
    }

    void OnDestroy()
    {
        Managers.Event.RemoveEvent(EEventType.GoogleSignup,Event_GoogleAccountSignup);
    }

    void Event_GoogleAccountSignup(Component sender, object param)
    {
        var loadingComplete = UI_LoadingPopup.Show();

        Managers.WebContents.AddGoogleAccount(new ReqDtoAddGoogleAccount()
        {
            UserAccountId = Managers.Game.UserInfo.UserAccountId,
            //GoogleAccount = Managers.Game.UserInfo.GoogleAccount
        },
       (response) =>
       {
            if(Managers.Game.UserInfo.UserNickname == "")
            {
                Managers.Scene.LoadScene(EScene.InputNicknameScene);
            }
            else
            {
                Managers.Scene.LoadScene(EScene.SuberunkerSceneHomeScene);
            }
       },
       (errorCode) =>
       {

       });
        loadingComplete.Value = true;
    }
}
