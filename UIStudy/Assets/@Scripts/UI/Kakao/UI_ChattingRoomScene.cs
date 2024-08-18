using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_ChattingRoomScene : UI_Scene
{
    public enum Texts
    {
        ChattingTMP
    }
    public enum GameObjects
    {
        ChatBubble,
        InputMessage
    }

    enum Buttons
    {
        Button_Send
    }

    private GameObject _chattingBubbleRoot = null;
    private TMP_InputField _inputMessage = null;

    protected override void Init()
    {
        base.Init();
        BindTexts(typeof(Texts));
        BindObjects(typeof(GameObjects));
        BindButtons(typeof(Buttons));
        _chattingBubbleRoot = GetObject((int)GameObjects.ChatBubble);
        _inputMessage = GetObject((int)GameObjects.InputMessage).gameObject.GetOrAddComponent<TMP_InputField>();

        
        this.Get<Button>((int)Buttons.Button_Send).gameObject.BindEvent((evt) =>
        {
            this.SendBubble();
        }, Define.EUIEvent.Click);
    }

    private void SendBubble()
    {
        var go = GameObject.Instantiate(Resources.Load("Kakao/UI/Chat_ME"), _chattingBubbleRoot.transform) as GameObject;
        //_inputMessage = GetObject((int)GameObjects.InputMessage).gameObject.GetOrAddComponent<TMP_InputField>();
        Debug.Log(_inputMessage.text);
        go.GetComponentInChildren<TMP_Text>().text = _inputMessage.text; // children¸»°í µý°É·Î ¹Ù²ã¾ßÇÒ µí.
        _inputMessage.text = "";
    }
}
