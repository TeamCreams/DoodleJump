using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class UI_LevelUpPopup : UI_Popup
{

    enum Texts
    {
        Level_Text,
        LevelUp_Text
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

        Managers.Event.AddEvent(EEventType.SetLanguage, OnEvent_SetLanguage);
        Managers.Event.TriggerEvent(EEventType.SetLanguage);

        GetText((int)Texts.Level_Text).text = Managers.Game.DifficultySettingsInfo.StageLevel.ToString();
        GetButton((int)Buttons.Cancle_Button).gameObject.BindEvent((evt) =>
        {
            Managers.UI.ClosePopupUI(this);
            Time.timeScale = 1;
        }, Define.EUIEvent.Click);

        Time.timeScale = 0;

        return true;
    }
    void OnEvent_SetLanguage(Component sender, object param)
    {
        GetText((int)Texts.LevelUp_Text).text = this.LocalizedString(ELocalizableTerms.LevelUp);
    }

    public string LocalizedString(ELocalizableTerms eLocalizableTerm)
    {
        int stringId = 0;

        foreach (var gameLanguageData in Managers.Data.GameLanguageDataDic)
        {
            if (gameLanguageData.Value.LocalizableTerm == eLocalizableTerm)
            {
                stringId = gameLanguageData.Value.Id;
                break;
            }
        }

        var content = Managers.Data.GameLanguageDataDic[stringId];

        switch (Managers.Game.ELanguageInfo)
        {
            case ELanguage.Kr:
                return content.KrText;

            case ELanguage.En:
                return content.EnText;
        }

        return "";
    }
}
