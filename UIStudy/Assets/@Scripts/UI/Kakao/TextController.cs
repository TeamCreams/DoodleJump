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
        TextInBox
    }

    protected override void Init()
    {
        base.Init();
        BindTexts(typeof(Texts));
        _layoutElement = GetText((int)Texts.TextInBox).GetOrAddComponent<LayoutElement>();
        StartCoroutine(ForceUpdate());
    }

    IEnumerator ForceUpdate()
    {
        yield return new WaitForSeconds(0.1f);
        this.SetChattingBubbleSize(GetText((int)Texts.TextInBox).preferredWidth);
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
