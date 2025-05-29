using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Input_SuberunkerScene : MonoBehaviour //BaseScene
{
    SuberunkerScene Scene => Managers.Scene.CurrentScene as SuberunkerScene;

    public void OnKeyAction()
    {
        // if(Input.GetKeyDown(KeyCode.C))
        // {
        //     Managers.Camera.Shake(1.0f, 0.2f);
        // }

        // if(Input.GetKeyDown(KeyCode.K))
        // {
        //     Managers.Event.TriggerEvent(Define.EEventType.LevelStageUp);
        // }

        if (Input.GetKeyDown(KeyCode.A))
        {
            Managers.UI.ShowPopupUI<UI_ContinuePopup>().SetInfo();
        }
    }
}
