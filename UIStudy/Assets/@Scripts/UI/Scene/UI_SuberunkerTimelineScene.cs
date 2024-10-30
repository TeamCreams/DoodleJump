using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class UI_SuberunkerTimelineScene : UI_Scene
{

    enum GameObjects
    {

    }

    enum Texts
    {
        Skip_Text
    }

    enum Buttons
    {
        Skip
    }

    public override bool Init()
    {
        if (base.Init() == false)
        {
            return false;
        }
        BindButtons(typeof(Buttons));
        GetButton((int)Buttons.Skip).gameObject.BindEvent((evt) =>
        {
            Managers.Scene.LoadScene(EScene.SuberunkerScene);
        }, EUIEvent.Click);
        Managers.Event.AddEvent(EEventType.SetLanguage, OnEvent_SetLanguage);
        Managers.Event.TriggerEvent(EEventType.SetLanguage);

        return true;
    }
    void OnEvent_SetLanguage(Component sender, object param)
    {
        GetText((int)Texts.Skip_Text).text = Managers.Language.LocalizedString(91016);
    }

}
