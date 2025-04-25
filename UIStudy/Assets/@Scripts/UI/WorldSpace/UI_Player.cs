using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using TMPro;
using UnityEngine;
using static Define;

public class UI_Player : UI_Base
{

    private enum Texts
    {
        Nickname_Text,
        Content_Text
    }

    private enum GameObjects
    {
        ThoughtBubble
    }

    Canvas _worldCanvas;
    public GameObject _thoughtBubble;
    private TMP_Text _content;
    private string _tempContent;
    private string _textId;

    public override bool Init()
    {
        if( base.Init() == false)
        {
            return false;
        }

        _worldCanvas = this.GetComponent<Canvas>();
        _worldCanvas.worldCamera = Camera.main;

        BindTexts(typeof(Texts));
        BindObjects(typeof(GameObjects));

        GetText((int)Texts.Nickname_Text).text = Managers.Game.UserInfo.UserNickname;
        _content = GetText((int)Texts.Content_Text);
        _thoughtBubble = GetObject((int)GameObjects.ThoughtBubble);
        _thoughtBubble.SetActive(false);

        Managers.Event.AddEvent(EEventType.ThoughtBubble, OnEvent_ThoughtBubble);
        Managers.Event.AddEvent(EEventType.CancelThoughtBubble, OnEvent_CancelThoughtBubble);

        return true;
    }

    private void OnDestroy()
    {
        Managers.Event.RemoveEvent(EEventType.ThoughtBubble, OnEvent_ThoughtBubble);
        Managers.Event.RemoveEvent(EEventType.CancelThoughtBubble, OnEvent_CancelThoughtBubble);
    }
    private void OnEvent_ThoughtBubble(Component sender, object param)
    {
       int doOrNot = Random.Range(0, 10);

        if(doOrNot < 6)
        {
            ThoughtBubble((EBehavior)(int)param);
        }
        ThoughtBubble((EBehavior)(int)param);
    }

    private void OnEvent_CancelThoughtBubble(Component sender, object param)
    {
        StopAllCoroutines();
        _content.text = "";
        _thoughtBubble.SetActive(false);
    }

    private void ThoughtBubble(EBehavior eBehavior)
    {
        StopAllCoroutines();

        var groupTextIds = Managers.Data.ThoughtBubbleDataDic
            .Where(selectBehavior => selectBehavior.Value.Behavior == eBehavior)
            .Select(selectTextId => selectTextId.Value.TextId)
            .ToList();

        int random = Random.Range(0, groupTextIds.Count);
        _textId = groupTextIds[random];

        foreach (var content in Managers.Data.ThoughtBubbleLanguageDataDic)
        {
            if (content.Value.TextId.Equals(_textId) == true)
            {
                switch (Managers.Language.ELanguageInfo)
                {
                    case ELanguage.Kr:
                        _tempContent = content.Value.KrText;
                        break;

                    case ELanguage.En:
                        _tempContent = content.Value.EnText;
                        break;
                }
            }
        }
        StartCoroutine(ThoughtBubbleText());
    }

    IEnumerator ThoughtBubbleText()
    {
        _thoughtBubble.SetActive(true);

        int count = 0;
        _content.text = "";

        while (count < _tempContent.Length)
        {
            _content.text += _tempContent[count].ToString();
            count++;
            yield return new WaitForSeconds(0.12f);
        }
        StartCoroutine(ThoughtBubbleActive());
    }

    IEnumerator ThoughtBubbleActive()
    {
        yield return new WaitForSeconds(2f);
        _thoughtBubble.SetActive(false);
    }
}
