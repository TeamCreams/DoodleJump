using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_SuberunkerTimelineScene : UI_Scene
{

    enum GameObjects
    {

    }

    enum Texts
    {

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
            Managers.Scene.LoadScene(Define.EScene.SuberunkerScene);
        }, Define.EUIEvent.Click);
        return true;
    }


}
