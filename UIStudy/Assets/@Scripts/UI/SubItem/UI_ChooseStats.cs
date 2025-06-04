using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using static Define;

public class UI_ChooseStats : UI_Base
{

    private enum Buttons
    {
        Back,
        Next
    }

    private enum Texts
    {
        Speed_Text,
        Life_Text,
        Luck_Text,
        Stat_Text
    }


    private int _playerDataId = 20001;
    private string _defaultSpeed = "기본 속도";
    private string _defaultHp = "기본 Hp";
    private string _defaultLuck = "기본 행운";


    public override bool Init()
    {
        if (base.Init() == false)
        {
            return false;
        }

        BindButtons(typeof(Buttons));
        BindTexts(typeof(Texts));
        
        GetButton((int)Buttons.Back).gameObject.BindEvent(OnClick_BackButton, EUIEvent.Click);
        GetButton((int)Buttons.Next).gameObject.BindEvent(OnClick_NextButton, EUIEvent.Click);
        Managers.Event.AddEvent(EEventType.SetLanguage, OnEvent_SetLanguage);
        OnEvent_SetLanguage(null, null);
        _playerDataId = Managers.Game.ChracterStyleInfo.CharacterId;
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

    public void DisplayInfo()
    {
        Managers.Game.ChracterStyleInfo.TempCharacterId = _playerDataId;
        GetText((int)Texts.Stat_Text).text = $"{Managers.Data.PlayerDic[_playerDataId].Name}";
        GetText((int)Texts.Speed_Text).text = $"{_defaultSpeed} : {Managers.Data.PlayerDic[_playerDataId].Speed}";
        GetText((int)Texts.Life_Text).text = $"{_defaultHp} : {Managers.Data.PlayerDic[_playerDataId].Hp}";
        GetText((int)Texts.Luck_Text).text = $"{_defaultLuck} : {Managers.Data.PlayerDic[_playerDataId].Luck}";
    }

    void OnEvent_SetLanguage(Component sender, object param)
    {
        _defaultSpeed = Managers.Language.LocalizedString(91010);
        _defaultHp = Managers.Language.LocalizedString(91011);
        _defaultLuck = Managers.Language.LocalizedString(91012);

        GetText((int)Texts.Speed_Text).text = $"{_defaultSpeed} : {Managers.Data.PlayerDic[_playerDataId].Speed}";
        GetText((int)Texts.Life_Text).text = $"{_defaultHp} : {Managers.Data.PlayerDic[_playerDataId].Hp}";
        GetText((int)Texts.Luck_Text).text = $"{_defaultLuck} : {Managers.Data.PlayerDic[_playerDataId].Luck}";
    }

}