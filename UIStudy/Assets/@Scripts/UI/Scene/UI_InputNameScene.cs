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
    private string _tempString = "";
    public override bool Init()
    {
        if (base.Init() == false)
        {
            return false;
        }
        BindInputFields(typeof(InputFields));
        BindButtons(typeof(Buttons));
        BindTexts(typeof(Texts));

        GetInputField((int)InputFields.Name_InputField).gameObject.BindEvent(OnClick_InputName, Define.EUIEvent.Click);
        GetButton((int)Buttons.InspectName_Button).gameObject.BindEvent(OnClick_InspectName, Define.EUIEvent.Click);
        GetText((int)Texts.Warning_Text).text = "";
        return true;
    }


    private void OnClick_InspectName(PointerEventData eventData)
    {
        if(CheckCorrectNickname(_tempString) == true)
        {
            _tempString = GetInputField((int)InputFields.Name_InputField).text;
            Managers.Game.ChracterStyleInfo.PlayerName = _tempString;
            Managers.Scene.LoadScene(Define.EScene.SuberunkerTimelineScene);
        }
        else
        {
            _tempString = "";
            GetText((int)Texts.Warning_Text).text = "사용할 수 없는 닉네임입니다.";
        }
    }

    private void OnClick_InputName(PointerEventData eventData)
    {
        GetText((int)Texts.Warning_Text).text = "";
        _tempString = GetInputField((int)InputFields.Name_InputField).text;
        //Debug.Log($"Name_InputField : {GetInputField((int)InputFields.Name_InputField).text}");
    }

    private bool CheckCorrectNickname(string name)
    {
        if (string.IsNullOrEmpty(name) || char.IsDigit(name[0]))
        {
            return false;
        }
        if (18 <  name.Length)
        {
            return false;
        }
        return true;
    }


    //ㄹㅐㄴ덤 이름 함        Managers.Scene.LoadScene(Define.EScene.SuberunkerTimelineScene);

}
