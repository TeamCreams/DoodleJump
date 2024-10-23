using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;
using static UI_InputNameScene;
using UnityEngine.EventSystems;

public class UI_SuberunkerSceneHomeScene : UI_Scene
{

    enum GameObjects
    {
        Ranking,
        MyScore
    }

    enum Texts
    {
        Best_Text,
        Current_Text
    }

    enum Buttons
    {
        Home_Button,
        Mission_Button,
        ChooseCharacter_Button,
        Start_Button
    }

    enum Images
    {
        MyScore_Button,
        Ranking_Button
    }

    enum Toggles
    {
        Language_En,
        Language_Kr
    }
    public override bool Init()
    {
        if (base.Init() == false)
        {
            return false;
        }
        BindObjects(typeof(GameObjects));
        BindTexts(typeof(Texts));
        BindButtons(typeof(Buttons));
        BindImages(typeof(Images));
        BindToggles(typeof(Toggles));

        GetButton((int)Buttons.ChooseCharacter_Button).gameObject.BindEvent((evt) =>
        {
            Managers.Scene.LoadScene(Define.EScene.ChooseCharacterScene);
        }, Define.EUIEvent.Click);

        GetButton((int)Buttons.Start_Button).gameObject.BindEvent((evt) =>
        {
            Managers.Scene.LoadScene(Define.EScene.SuberunkerTimelineScene);
        }, Define.EUIEvent.Click);

        GetObject((int)GameObjects.MyScore).SetActive(false);

        GetImage((int)Images.MyScore_Button).gameObject.BindEvent(OnClick_ShowMyScore, Define.EUIEvent.Click);
        GetImage((int)Images.Ranking_Button).gameObject.BindEvent(OnClick_ShowRanking, Define.EUIEvent.Click);
        GetToggle((int)Toggles.Language_En).gameObject.BindEvent(OnClick_SetLanguage, Define.EUIEvent.Click);
        GetToggle((int)Toggles.Language_Kr).gameObject.BindEvent(OnClick_SetLanguage, Define.EUIEvent.Click);


        return true;
    }

    private void OnClick_ShowMyScore(PointerEventData eventData)
    {
        GetObject((int)GameObjects.MyScore).SetActive(true);
        GetObject((int)GameObjects.Ranking).SetActive(false);
        
        int recordMinutes = Mathf.FloorToInt(Managers.Game.PlayTimeRecord / 60);
        float recordSeconds = Managers.Game.PlayTimeRecord % 60;
        GetText((int)Texts.Best_Text).text = $"최고 기록 : {recordMinutes}분 {recordSeconds}초";

        int minutes = Mathf.FloorToInt(Managers.Game.PlayTime / 60);
        float seconds = Managers.Game.PlayTime % 60;
        GetText((int)Texts.Current_Text).text = $"최근 기록 : {minutes}분 {seconds}초";
    }

    private void OnClick_ShowRanking(PointerEventData eventData)
    {
        GetObject((int)GameObjects.MyScore).SetActive(false);
        GetObject((int)GameObjects.Ranking).SetActive(true);
    }

    private void OnClick_SetLanguage(PointerEventData eventData)
    {
        if(GetToggle((int)Toggles.Language_En).isOn == true)
        {
            Managers.Game.ELanguageInfo = ELanguage.En;
        }
        else
        {
            Managers.Game.ELanguageInfo = ELanguage.Kr;
        }

        Debug.Log($"language : {Managers.Game.ELanguageInfo}");
    }
}
