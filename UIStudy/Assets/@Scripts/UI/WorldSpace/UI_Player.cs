using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using TMPro;
using UnityEngine;
using static Define;

public class UI_Player : UI_Base
{
    Canvas _worldCanvas;

    private enum Texts
    {
        Nickname_Text,
        Content_Text
    }

    private enum GameObjects
    {
        ThoughtBubble
    }

    public GameObject _thoughtBubble;
    private TMP_Text _content;
    private string _tempContent;

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

        GetText((int)Texts.Nickname_Text).text = Managers.Game.ChracterStyleInfo.PlayerName;
        _content = GetText((int)Texts.Content_Text);
        _thoughtBubble = GetObject((int)GameObjects.ThoughtBubble);
        _thoughtBubble.SetActive(false);

        Managers.Event.AddEvent(EEventType.ThoughtBubble, OnEvent_ThoughtBubble);

        return true;
    }

    private void OnEvent_ThoughtBubble(Component sender, object param)
    {
       int doOrNot = Random.Range(0, 10);

        if(doOrNot < 6)
        {
            StopAllCoroutines();
            EBehavior eBehavior = (EBehavior)(int)param;

            var groupTexts = Managers.Data.ThoughtBubbleDataDic
                .Where(selectBehavior => selectBehavior.Value.Behavior == eBehavior)
                .Select(selectText => selectText.Value.Text)
                .ToList();

            int random = Random.Range(0, groupTexts.Count);
            _content.text = "";
            _tempContent = groupTexts[random];
            StartCoroutine(ThoughtBubbleText());
        }
    }

    IEnumerator ThoughtBubbleText()
    {
        _thoughtBubble.SetActive(true);

        int count = 0;

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
