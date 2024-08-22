using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UI_KakaoScene : UI_Scene
{
    public enum Buttons
    {
        Button_Home,
        Button_Chatting
    }
    
    public enum GameObjects
    {
        FriendsPage,
        ChattingPage
    }// 추가될 예정
    // 채팅방은 GameObjects인가?

    protected override void Init()
    {
        base.Init();
        BindButtons(typeof(Buttons));
        BindObjects(typeof(GameObjects));

        Get<GameObject>((int)GameObjects.ChattingPage).SetActive(false);

        this.Get<Button>((int)Buttons.Button_Home).gameObject.BindEvent((evt) =>
        {
            foreach(GameObjects page in Enum.GetValues(typeof(GameObjects)))
            {
                Get<GameObject>((int)page).SetActive(false);
            }
            Get<GameObject>((int)GameObjects.FriendsPage).SetActive(true);
        }, Define.EUIEvent.Click);

        this.Get<Button>((int)Buttons.Button_Chatting).gameObject.BindEvent((evt) =>
        {
            foreach (GameObjects page in Enum.GetValues(typeof(GameObjects)))
            {
                Get<GameObject>((int)page).SetActive(false);
            }
            Get<GameObject>((int)GameObjects.ChattingPage).SetActive(true);

        }, Define.EUIEvent.Click);


    }


}
