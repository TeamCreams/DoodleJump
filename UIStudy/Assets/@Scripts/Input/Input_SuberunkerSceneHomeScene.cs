using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using static Define;

public class Input_SuberunkerSceneHomeScene : MonoBehaviour //BaseScene
{
    SuberunkerSceneHomeScene Scene => Managers.Scene.CurrentScene as SuberunkerSceneHomeScene;

    ReactiveProperty<bool> loadingComplete = null;
    public void OnKeyAction()
    {
        // 치트키 1
        if(Input.GetKeyDown(KeyCode.A))
        {
            Debug.Log("hihihihihihihiihi");
            SecurePlayerPrefs.SetString("MyValue", "MyValue1");
        }


        if (Input.GetKeyDown(KeyCode.D))
        {
            loadingComplete = UI_LoadingPopup.Show();
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            if (loadingComplete != null && loadingComplete.HasValue)
            {
                loadingComplete.Value = true;
            }
        }


        // if (Input.GetKeyDown(KeyCode.S))
        // {           
        //     var test = SecurePlayerPrefs.GetString("MyValue", "test");
        //     var toast = Managers.UI.ShowPopupUI<UI_ToastPopup>();
        //     //toast.Show(UI_ToastPopup.Type.Debug, test);
        //     toast.SetInfo(test);
        // }

    }
}
