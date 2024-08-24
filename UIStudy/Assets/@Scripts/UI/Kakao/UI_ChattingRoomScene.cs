using System.Collections;
using TMPro;
using UnityEditor.VersionControl;
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
        ChatBubble_GO
    }

    enum Buttons
    {
        Button_Send
    }

    enum InputFields
    {
        InputMessage_IF
    }

    private GameObject _chattingBubbleRoot = null;
    private TMP_InputField _inputMessage = null;
    private Messages _messages = null;
    private GameObject _chatMe = null;
    private GameObject _chatYou = null;

    protected override void Init()
    {
        base.Init();

        BindObjects(typeof(GameObjects));
        BindButtons(typeof(Buttons));
        BindInputFields(typeof(InputFields));

        _chattingBubbleRoot = GetObject((int)GameObjects.ChatBubble_GO);
        _inputMessage = GetInputField((int)InputFields.InputMessage_IF);

        // StartLoadAssets을 해줘야 함
        _chatMe = Managers.Resource.Load<GameObject>("Chat_ME"); // null
        Debug.Log(_chatMe);
        _chatYou = Managers.Resource.Load<GameObject>("Chat_YOU"); // null
        Debug.Log(_chatYou);
        _messages = Managers.Message.ReadTextFile();
        foreach (var message in _messages.Chatting)
        {
            if(message.name == "Me")
            {
                this.SendBubble(_chatMe, message.message);
            }
            else
            {
                this.SendBubble(_chatYou, message.message);
            }
        }

        this.Get<Button>((int)Buttons.Button_Send).gameObject.BindEvent((evt) =>
        {
            this.SendBubble(_chatMe, _inputMessage.text, true);
        }, Define.EUIEvent.Click);
    }

    private void SendBubble(GameObject prefab, string text, bool input = false)
    {
        GameObject.Instantiate(prefab, _chattingBubbleRoot.transform);
        //var go = GameObject.Instantiate(prefab, _chattingBubbleRoot.transform) as GameObject;
        BindTexts(typeof(Texts)); // 맞는 지 모르겠음
        GetText((int)Texts.ChattingTMP).text = text; // ChattingTMP가 clone으로 여러개 생성될 건데 접근 어케할지..
        //go.GetComponentInChildren<TMP_Text>().text = text; // Clone으로 생성되는 자식에게 어케 접근하는지.

        if (input)
        {
            _inputMessage.text = "";
        }
        StartCoroutine(ForceUpdate()); //Invoke(nameof(ForceUpdate1), 1.0f);
    }

    void ForceUpdate1()
    {
        LayoutRebuilder.ForceRebuildLayoutImmediate(_chattingBubbleRoot.GetComponent<RectTransform>());
    }

    IEnumerator ForceUpdate()
    {
        yield return new WaitForSeconds(1.0f);
        LayoutRebuilder.ForceRebuildLayoutImmediate(_chattingBubbleRoot.GetComponent<RectTransform>());
    }
}
