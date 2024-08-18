using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class InputText : UI_Base
{
    public enum Texts
    {
        ChattingTMP
    }
    public enum GameObjects
    {
        ChatBubble,
        SendButton,
        InputMessage
    }

    private GameObject _chattingBubbleRoot = null;
    private TMP_InputField _inputMessage = null;
    private Button _sendButton = null;
    protected override void Init()
    {
        base.Init();
        BindTexts(typeof(Texts));
        BindObjects(typeof(GameObjects));
        _chattingBubbleRoot = GetObject((int)GameObjects.ChatBubble);
        _sendButton = GetObject((int)GameObjects.SendButton).gameObject.GetOrAddComponent<Button>();
        _sendButton.onClick.AddListener(SendBubble);
    }

    private void SendBubble()
    {
        var go = GameObject.Instantiate(Resources.Load("Kakao/UI/Chat_ME"), _chattingBubbleRoot.transform) as GameObject;
        _inputMessage = GetObject((int)GameObjects.InputMessage).gameObject.GetOrAddComponent<TMP_InputField>();
        Debug.Log(_inputMessage.text);
        go.GetComponentInChildren<TMP_Text>().text = _inputMessage.text; // children¸»°í µý°É·Î ¹Ù²ã¾ßÇÒ µí.
        _inputMessage.text = "";
    }
}
