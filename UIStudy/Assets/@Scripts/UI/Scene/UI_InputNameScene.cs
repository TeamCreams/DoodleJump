using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static Define;

public class UI_InputNameScene : UI_Scene
{

    private enum InputFields
    {
        Name_InputField,
    }

    private enum Buttons
    {
        InspectName_Button,
    }

    private enum Texts
    {
        Warning_Text,
        Name_Text,
        Placeholder_Nickname_Text
    }

    private string _nicknameUnavailable = "사용할 수 없는 닉네임입니다.";

    public override bool Init()
    {
        if (base.Init() == false)
        {
            return false;
        }
        BindInputFields(typeof(InputFields));
        BindButtons(typeof(Buttons));
        BindTexts(typeof(Texts));

        GetButton((int)Buttons.InspectName_Button).gameObject.BindEvent(OnClick_InspectName, EUIEvent.Click);
        GetText((int)Texts.Warning_Text).text = "";

        Managers.Event.AddEvent(EEventType.SetLanguage, OnEvent_SetLanguage);
        Managers.Event.TriggerEvent(EEventType.SetLanguage);

        return true;
    }


    private void OnClick_InspectName(PointerEventData eventData)
    {
        EErrorCode errCode = CheckCorrectNickname(GetInputField((int)InputFields.Name_InputField).text);

        if (errCode != EErrorCode.ERR_OK)
        {
            //Localization 세계화 번역작업
            //Managers.Data.Localization[][ko]
            GetText((int)Texts.Warning_Text).text = _nicknameUnavailable;
            return;
        }
        
        Managers.Game.ChracterStyleInfo.PlayerName = GetInputField((int)InputFields.Name_InputField).text;
        Managers.Scene.LoadScene(EScene.SuberunkerTimelineScene);
    }

    private EErrorCode CheckCorrectNickname(string name)
    {
        if (string.IsNullOrEmpty(name) || char.IsDigit(name[0]))
        {
            return EErrorCode.ERR_ValidationNickname;
        }
        if (18 <  name.Length)
        {
            return EErrorCode.ERR_ValidationNickname;
        }
        return EErrorCode.ERR_OK;
    }


    void OnEvent_SetLanguage(Component sender, object param)
    {
        GetText((int)Texts.Name_Text).text = Managers.Language.LocalizedString(91013);
        GetText((int)Texts.Placeholder_Nickname_Text).text = Managers.Language.LocalizedString(91014);
        _nicknameUnavailable = Managers.Language.LocalizedString(91015);
    }
}
