using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TextController : UI_Base
{
    private const int MAX_WIDTH = 250;
    private LayoutElement _layoutElement = null;
    public enum Texts
    {
        ChattingTMP
    }

    protected override void Init()
    {
        base.Init();
        BindTexts(typeof(Texts));
        _layoutElement = GetText((int)Texts.ChattingTMP).GetOrAddComponent<LayoutElement>();
    }

    private void Start()
    {
        // 데이터를 받아올 거니까 여기 있어야함.
        //this.SetChattingBubbleSize(GetText((int)Texts.ChattingTMP).preferredWidth);
    }
    void Update()
    {
        this.SetChattingBubbleSize(GetText((int)Texts.ChattingTMP).preferredWidth);
    }

    private void SetChattingBubbleSize(float width)
    {
        if (MAX_WIDTH <= width)
        {
            if (_layoutElement.enabled == false)
            {

                _layoutElement.enabled = true;
            }
        }
        else
        {
            if (_layoutElement.enabled == true)
            {

                _layoutElement.enabled = false;
            }
        }
    }
}
