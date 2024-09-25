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

    public override bool Init()
    {
        if (base.Init() == false)
        {
            return false;
        }
        BindTexts(typeof(Texts));
        _layoutElement = GetText((int)Texts.TextInBox).GetOrAddComponent<LayoutElement>();


        //_layoutElement.GetComponent<ContentSizeFitter>().SetLayoutHorizontal();
        //_layoutElement.GetComponent<ContentSizeFitter>().SetLayoutVertical();
        //Canvas.ForceUpdateCanvases();  // *
        //LayoutRebuilder.ForceRebuildLayoutImmediate(_layoutElement.GetComponent<RectTransform>());


        //SetChattingBubbleSize(GetText((int)Texts.TextInBox).preferredWidth);
        StartCoroutine(ForceUpdate());
        return true;
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
