using System.Collections;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class UI_ChattingRoomScene : UI_Scene
{
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

    public override bool Init()
    {
        if (base.Init() == false)
        {
            return false;
        }


        StartLoadAssets(); // 그냥 기본 class를 만들어서 쓰는 것도 ㄱㅊ을 듯
        return true;
    }

    private void SendBubble(string name, string text, bool input = false)
    {
        var bubble = Managers.Resource.Instantiate(name, _chattingBubbleRoot.transform);
        bubble.GetOrAddComponent<UI_ChattingTest>().SetText(text);

		//for (int i = 0; i < 5; i++)
		//{
		//    var a = bubble.transform.GetChild(0).GetComponent<HorizontalLayoutGroup>();

		//    a.SetLayoutHorizontal();
		//    a.CalculateLayoutInputHorizontal();

		//    LayoutRebuilder.ForceRebuildLayoutImmediate(bubble.transform.GetChild(0).GetComponent<RectTransform>());
		//    var csf = bubble.GetComponent<ContentSizeFitter>();
		//    csf.SetLayoutVertical();
		//    csf.SetLayoutHorizontal();


		//    a.SetLayoutHorizontal();
		//    a.CalculateLayoutInputHorizontal();

		//    LayoutRebuilder.ForceRebuildLayoutImmediate(bubble.GetComponent<RectTransform>());
		//    LayoutRebuilder.ForceRebuildLayoutImmediate(_chattingBubbleRoot.GetComponent<RectTransform>());


		//}


		Observable.Timer(new System.TimeSpan(0, 0, 0, 0, 200))
           .Subscribe(_ =>
           {
	           var a = bubble.transform.GetChild(0).GetComponent<HorizontalLayoutGroup>();

	           a.SetLayoutHorizontal();
	           a.CalculateLayoutInputHorizontal();

	           var csf = bubble.GetComponent<ContentSizeFitter>();
	           csf.SetLayoutVertical();

	           LayoutRebuilder.ForceRebuildLayoutImmediate(bubble.transform.GetChild(0).GetComponent<RectTransform>());
	           LayoutRebuilder.ForceRebuildLayoutImmediate(bubble.GetComponent<RectTransform>());
	           LayoutRebuilder.ForceRebuildLayoutImmediate(_chattingBubbleRoot.GetComponent<RectTransform>());
           });
		if (input)
        {
            _inputMessage.text = "";
        }
        //StartCoroutine(ForceUpdate()); //Invoke(nameof(ForceUpdate1), 1.0f);
    }

    void ForceUpdate1()
    {
        LayoutRebuilder.ForceRebuildLayoutImmediate(_chattingBubbleRoot.GetComponent<RectTransform>());
    }

    IEnumerator ForceUpdate()
    {
        yield return new WaitForSeconds(0.3f); // 문장이 길어지면 1초여도 안고쳐짐
        LayoutRebuilder.ForceRebuildLayoutImmediate(_chattingBubbleRoot.GetComponent<RectTransform>());
    }
    void StartLoadAssets()
    {
        Managers.Resource.LoadAllAsync<UnityEngine.Object>("PreLoad", (key, count, totalCount) =>
        {
            Debug.Log($"{key} {count}/{totalCount}");

            if (count == totalCount)
            {
                Debug.Log("Load Complete");
                Managers.Data.Init();

                // 여기 아래로 넣으면 됨. 
                // Init에 넣으면 안 됨.
                BindObjects(typeof(GameObjects));
                BindButtons(typeof(Buttons));
                BindInputFields(typeof(InputFields));

                _chattingBubbleRoot = GetObject((int)GameObjects.ChatBubble_GO);
                _inputMessage = GetInputField((int)InputFields.InputMessage_IF);

                _messages = Managers.Message.ReadTextFile();
                foreach (var message in _messages.Chatting)
                {
                    if (message.name == "Me")
                    {
                        this.SendBubble("Chat_ME", message.message);
                    }
                    else
                    {
                        this.SendBubble("Chat_YOU", message.message);
                    }
                }

                this.Get<Button>((int)Buttons.Button_Send).gameObject.BindEvent((evt) =>
                {
                    this.SendBubble("Chat_ME", _inputMessage.text, true);
                }, Define.EUIEvent.Click);
            }
        });
    }
}
