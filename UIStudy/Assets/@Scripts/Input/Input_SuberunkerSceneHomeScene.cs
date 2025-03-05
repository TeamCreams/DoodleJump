using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class Input_SuberunkerSceneHomeScene : MonoBehaviour //BaseScene
{
    SuberunkerSceneHomeScene Scene => Managers.Scene.CurrentScene as SuberunkerSceneHomeScene;

    public void OnKeyAction()
    {
        // 치트키 1
        if(Input.GetKeyDown(KeyCode.A))
        {
            SecurePlayerPrefs.SetString("MyValue", "MyValue1");
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            var test = SecurePlayerPrefs.GetString("MyValue", "test");
            var toast = Managers.UI.ShowPopupUI<UI_ToastPopup>();
            //toast.Show(UI_ToastPopup.Type.Debug, test);
            Managers.Event.TriggerEvent(EEventType.ToastPopupNotice, this, test);
        }
    }
}
