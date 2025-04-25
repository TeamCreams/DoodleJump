using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class UI_LevelUpPopup : UI_Popup
{

    private enum Texts
    {
        Level_Text,
        LevelUp_Text
    }

    private enum Images
    {
        Close,
    }

    public override bool Init()
    {
        if (base.Init() == false)
        {
            return false;
        }
        BindTexts(typeof(Texts));
        BindImages(typeof(Images));

        Managers.Event.AddEvent(EEventType.SetLanguage, OnEvent_SetLanguage);
        OnEvent_SetLanguage(null, null);

        GetText((int)Texts.Level_Text).text = Managers.Game.DifficultySettingsInfo.StageLevel.ToString();

        GetImage((int)Images.Close).gameObject.BindEvent((evt) =>
        {
            Managers.UI.ClosePopupUI(this);
            Time.timeScale = 1;
        }, EUIEvent.Click);

        Time.timeScale = 0;

        return true;
    }
    private void OnDestroy()
    {
        Managers.Event.RemoveEvent(EEventType.SetLanguage, OnEvent_SetLanguage);
    }
    void OnEvent_SetLanguage(Component sender, object param)
    {
        GetText((int)Texts.LevelUp_Text).text = Managers.Language.LocalizedString(91019);
    }

}
