using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_SuberunkerButtons : UI_Base
{
    public enum Buttons
    {
        Retry_Button,
    }

    public override bool Init()
    {
        if (base.Init() == false)
        {
            return false;
        }
        BindButtons(typeof(Buttons));

        this.Get<Button>((int)Buttons.Retry_Button).gameObject.BindEvent((evt) =>
        {
            Managers.Scene.LoadScene(Define.EScene.SuberunkerScene);
        }, Define.EUIEvent.Click);
        return true;
    }
}
