using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI_ChooseStats : UI_Base
{

    enum Buttons
    {
        Back,
        Next
    }

    enum Texts
    {
        Speed_Text,
        Life_Text,
        Luck_Text,
        Stat_Text
    }


    private int _playerDataLastId = 0;
    private int _playerDataId = 20001;

    public override bool Init()
    {
        if (base.Init() == false)
        {
            return false;
        }

        BindButtons(typeof(Buttons));
        BindTexts(typeof(Texts));
        
        GetButton((int)Buttons.Back).gameObject.BindEvent(OnClick_BackButton, Define.EUIEvent.Click);
        GetButton((int)Buttons.Next).gameObject.BindEvent(OnClick_NextButton, Define.EUIEvent.Click);

        //_playerDataLastId = 20000 + Managers.Data.PlayerDic.Count;
        DisplayInfo();
        return true;
    }

    private void OnClick_BackButton(PointerEventData eventData)
    {
        _playerDataId--;
        if (_playerDataId < 20001)
        {
            _playerDataId = 20000 + Managers.Data.PlayerDic.Count;
        }
        this.DisplayInfo();
    }
    private void OnClick_NextButton(PointerEventData eventData)
    {
        _playerDataId++;
        if(20000 + Managers.Data.PlayerDic.Count < _playerDataId)
        {
            _playerDataId = 20001;
        }
        this.DisplayInfo();
    }

    private void DisplayInfo()
    {
        Managers.Game.ChracterStyleInfo.CharacterId = _playerDataId;
        GetText((int)Texts.Stat_Text).text = $"{Managers.Data.PlayerDic[_playerDataId].Name}";
        GetText((int)Texts.Speed_Text).text = $"Default Speed : {Managers.Data.PlayerDic[_playerDataId].Speed}";
        GetText((int)Texts.Life_Text).text = $"Default Life : {Managers.Data.PlayerDic[_playerDataId].Life}";
        GetText((int)Texts.Luck_Text).text = $"Default Luck : {Managers.Data.PlayerDic[_playerDataId].Luck}";
    }

}