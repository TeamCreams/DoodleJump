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

    void Update()
    {
        this.SetChattingBubbleSize(GetText((int)Texts.ChattingTMP).preferredWidth);
        // 이게 업데이트에 있을 필요는..
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
