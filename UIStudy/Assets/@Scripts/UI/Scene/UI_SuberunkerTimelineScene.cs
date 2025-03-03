using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using static Define;

public class UI_SuberunkerTimelineScene : UI_Scene
{

    private enum GameObjects
    {

    }

    private enum Texts
    {
        Skip_Text
    }

    private enum Buttons
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
        BindTexts(typeof(Texts));
        GetButton((int)Buttons.Skip).gameObject.BindEvent(OnClick_Skip, EUIEvent.Click);
        Managers.Event.AddEvent(EEventType.SetLanguage, OnEvent_SetLanguage);
        OnEvent_SetLanguage(null, null);

        return true;
    }
    private void OnClick_Skip(PointerEventData eventData)
    {
        Managers.Scene.LoadScene(EScene.SuberunkerScene);
    }
    void OnEvent_SetLanguage(Component sender, object param)
    {
        GetText((int)Texts.Skip_Text).text = Managers.Language.LocalizedString(91016);
    }

}
