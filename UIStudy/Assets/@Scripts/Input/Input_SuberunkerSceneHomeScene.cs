using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using System.Net.NetworkInformation;

using static Define;

public class Input_SuberunkerSceneHomeScene : MonoBehaviour //BaseScene
{
    SuberunkerSceneHomeScene Scene => Managers.Scene.CurrentScene as SuberunkerSceneHomeScene;

    ReactiveProperty<bool> loadingComplete = null;
    public void OnKeyAction()
    {
        // // 치트키 1
        // if(Input.GetKeyDown(KeyCode.A))
        // {
        //     Debug.Log("hihihihihihihiihi");
        //     SecurePlayerPrefs.SetString("MyValue", "MyValue1");
        // }


        // if(Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.X))
        // {
        //     UI_ToastPopup.Show("create", UI_ToastPopup.Type.Debug);
        // }
        // if(Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.C))
        // {
        //     UI_ToastPopup.Show("create", UI_ToastPopup.Type.Error);
        // }
        // if(Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.V))
        // {

        //     int reward = Scene.GetRandomReward();
        //     UI_RewardAcquiredPopup popup = Managers.UI.ShowPopupUI<UI_RewardAcquiredPopup>();
        //     popup.SetInfo(reward);

        // }
        // if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.D))
        // {
        //     Managers.IAP.Buy("CASH_1000");
        // }
        // if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.W))
        // {
        //     Systems.GoogleLoginWebView.ShowUrl();
        // }


        // if (Input.GetKeyDown(KeyCode.D))
        // {
        //     loadingComplete = UI_LoadingPopup.Show();
        // }

        // if (Input.GetKeyDown(KeyCode.F))
        // {
        //     if (loadingComplete != null && loadingComplete.HasValue)
        //     {
        //         loadingComplete.Value = true;
        //     }
        // }

        // if (Input.GetKeyDown(KeyCode.S))
        // {           
        //     //var test = SecurePlayerPrefs.GetString("MyValue", "test");
        //     UI_ToastPopup.ShowCritical(Managers.Error.GetError(EErrorCode.ERR_NetworkSettlementErrorResend));
        // }

    }

}
