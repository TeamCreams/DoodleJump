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

    private string _tempString = "";
    public override bool Init()
    {
        if (base.Init() == false)
        {
            return false;
        }
        BindInputFields(typeof(InputFields));
        BindButtons(typeof(Buttons));
        GetInputField((int)InputFields.Name_InputField).gameObject.BindEvent(OnClick_InputName, Define.EUIEvent.Click);
        GetButton((int)Buttons.InspectName_Button).gameObject.BindEvent(OnClick_InspectName, Define.EUIEvent.Click);
        return true;
    }


    private void OnClick_InspectName(PointerEventData eventData)
    {
        if(CheckCorrectNickname(_tempString))
        {
            Managers.Game.ChracterStyleInfo.PlayerName = _tempString;
            Managers.Scene.LoadScene(Define.EScene.SuberunkerScene);
        }
    }

    private void OnClick_InputName(PointerEventData eventData)
    {
        _tempString = GetInputField((int)InputFields.Name_InputField).text;
        Debug.Log($"Name_InputField : {GetInputField((int)InputFields.Name_InputField).text}");
    }

    private bool CheckCorrectNickname(string name)
    {
        if (0 <= name[0] && name[0] <= 9)
        {
            _tempString = "";
            return false;
        }

        if(18 < name.Length || name.Length < 0)
        {
            _tempString = "";
            return false;
        }
        return true;
    }

    //ㄹㅐㄴ덤 이름 함        Managers.Scene.LoadScene(Define.EScene.SuberunkerTimelineScene);

 }
