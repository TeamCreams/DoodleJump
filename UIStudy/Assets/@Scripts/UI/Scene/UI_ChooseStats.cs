using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI_ChooseStats : UI_Scene
{
    enum Images
    {
    }

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

    enum GameObjects
    {
        UI_ColorPicker
    }

    public UI_ColorPicker ColorPicker { get; private set; }

    private int _playerDataLastId = 0;
    private int _playerDataId = 20001;
    public int PlayerDataId
    {
        get => _playerDataId;
        private set
        {
            _playerDataId = value;
        }
    }
    public override bool Init()
    {
        if (base.Init() == false)
        {
            return false;
        }

        BindObjects(typeof(GameObjects));
        BindButtons(typeof(Buttons));
        BindTexts(typeof(Texts));
        /*
        ColorPicker = Get<GameObject>((int)GameObjects.UI_ColorPicker).GetComponent<UI_ColorPicker>();
        Debug.Assert(ColorPicker != null, $"{nameof(UI_ColorPicker)} is null---------------");
        Debug.Log("0987654321234567890ChooseStat");
        */
        GetButton((int)Buttons.Back).gameObject.BindEvent(OnClick_BackButton, Define.EUIEvent.Click);
        GetButton((int)Buttons.Next).gameObject.BindEvent(OnClick_NextButton, Define.EUIEvent.Click);

        //_playerDataLastId = 20000 + Managers.Data.PlayerDic.Count;
        return true;
    }

    private void OnClick_BackButton(PointerEventData eventData)
    {
        PlayerDataId--;
        if (PlayerDataId < 20001)
        {
            PlayerDataId = 20000 + Managers.Data.PlayerDic.Count;
        }
        this.DisplayInfo(PlayerDataId);
    }
    private void OnClick_NextButton(PointerEventData eventData)
    {
        PlayerDataId++;
        if(20000 + Managers.Data.PlayerDic.Count < PlayerDataId)
        {
            PlayerDataId = 20001;
        }
        this.DisplayInfo(PlayerDataId);
    }

    private void DisplayInfo(int templateId)
    {
        Managers.Game.ChracterStyleInfo.CharacterId = PlayerDataId;
        GetText((int)Texts.Stat_Text).text = $"{Managers.Data.PlayerDic[templateId].Name}";
        GetText((int)Texts.Speed_Text).text = $"Default Speed : {Managers.Data.PlayerDic[templateId].Speed}";
        GetText((int)Texts.Life_Text).text = $"Default Life : {Managers.Data.PlayerDic[templateId].Life}";
        GetText((int)Texts.Luck_Text).text = $"Default Luck : {Managers.Data.PlayerDic[templateId].Luck}";
    }

}