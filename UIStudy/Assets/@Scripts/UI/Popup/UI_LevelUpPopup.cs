using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_LevelUpPopup : UI_Popup
{

    enum Texts
    {
        Level_Text
    }

    enum Buttons
    {
        Cancle_Button,
    }


    public override bool Init()
    {
        if (base.Init() == false)
        {
            return false;
        }
        BindTexts(typeof(Texts));
        BindButtons(typeof(Buttons));

        GetText((int)Texts.Level_Text).text = Managers.Game.DifficultySettingsInfo.StageLevel.ToString();
        GetButton((int)Buttons.Cancle_Button).gameObject.BindEvent((evt) =>
        {
            Managers.UI.ClosePopupUI(this);
            Time.timeScale = 1;
        }, Define.EUIEvent.Click);

        Time.timeScale = 0;
        return true;
    }

}
