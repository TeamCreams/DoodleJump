using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UI_ChooseStats : UI_Scene
{
    enum Images
    {
    }

    enum Buttons
    {
    }

    enum Texts
    {
    }

    enum GameObjects
    {
        UI_ColorPicker
    }

    public UI_ColorPicker ColorPicker { get; private set; }

    protected override void Init()
    {
        base.Init();

        BindObjects(typeof(GameObjects));

        ColorPicker = Get<GameObject>((int)GameObjects.UI_ColorPicker).GetComponent<UI_ColorPicker>();
        Debug.Assert(ColorPicker != null, $"{nameof(UI_ColorPicker)} is null");
    }

}

