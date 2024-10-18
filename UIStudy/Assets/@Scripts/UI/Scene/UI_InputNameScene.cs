using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static Define;

public class UI_InputNameScene : UI_Scene
{
    public enum InputFields
    {
        Name_InputField,
    }

    public enum Buttons
    {
        InspectName_Button,
    }

    public enum Texts
    {
        Warning_Text,
    }
    public override bool Init()
    {
        if (base.Init() == false)
        {
            return false;
        }
        BindInputFields(typeof(InputFields));
        BindButtons(typeof(Buttons));
        BindTexts(typeof(Texts));

        GetButton((int)Buttons.InspectName_Button).gameObject.BindEvent(OnClick_InspectName, Define.EUIEvent.Click);
        GetText((int)Texts.Warning_Text).text = "";
        return true;
    }


    private void OnClick_InspectName(PointerEventData eventData)
    {
        EErrorCode errCode = CheckCorrectNickname(GetInputField((int)InputFields.Name_InputField).text);

        if (errCode != EErrorCode.ERR_OK)
        {
            //Localization 세계화 번역작업
            //Managers.Data.Localization[][ko]
            GetText((int)Texts.Warning_Text).text = "사용할 수 없는 닉네임입니다.";
            return;
        }
        
        Managers.Game.ChracterStyleInfo.PlayerName = GetInputField((int)InputFields.Name_InputField).text;
        Managers.Scene.LoadScene(Define.EScene.SuberunkerTimelineScene);
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


    //ㄹㅐㄴ덤 이름 함        Managers.Scene.LoadScene(Define.EScene.SuberunkerTimelineScene);

}
