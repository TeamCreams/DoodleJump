using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static Define;

public class UI_ClickAnimator : UI_Base
{
    private Vector3 _scale;
    private Vector3 _nextScale;
    private Coroutine _coroutine = null;
    //private Button _button;
    public override bool Init()
    {
        if (base.Init() == false)
        {
            return false;
        }
        //_button = this.gameObject.GetOrAddComponent<Button>();

        _scale = this.gameObject.transform.localScale;
        _nextScale = new Vector3(_scale.x * 0.9f, _scale.y * 0.9f, _scale.z * 0.9f);
        this.gameObject.BindEvent(OnPointerDown_Button, EUIEvent.PointerDown);
        this.gameObject.BindEvent(OnPointerUp_Button, EUIEvent.PointerUp);
        this.gameObject.BindEvent(OnPointerExit_Button, EUIEvent.PointerExit);
        return true;
    }
    private void OnPointerDown_Button(PointerEventData eventData)
    {
        Debug.Log("OnPointerDown_Button");
        if(_coroutine != null)
        {
            StopCoroutine(_coroutine);
            _coroutine = null;
        }

        if(_coroutine == null)
        {
            _coroutine = StartCoroutine(ChangedScale(_scale, _nextScale));
        }
    }
    public void OnPointerUp_Button(PointerEventData eventData)
    {
        if(_coroutine != null)
        {
            StopCoroutine(_coroutine);
            _coroutine = null;
        }

        this.gameObject.transform.localScale = _scale;
    }
    public void OnPointerExit_Button(PointerEventData eventData)
    {
        if(_coroutine != null)
        {
            StopCoroutine(_coroutine);
            _coroutine = null;
        }

        this.gameObject.transform.localScale = _scale;
    }


    // public void OnEvent_PoiterUp(Component sender, object param)
    // {
    //     // 모든 버튼들의 이벤트가 호출되는데 이게 맞나
    //     // 그리고 버튼 클릭을 중간에 끊어도 문제..
    //     Debug.Log("OnEvent_PoiterUp");
    //     if(_coroutine != null)
    //     {
    //         StopCoroutine(_coroutine);
    //         _coroutine = null;
    //     }

    //     this.gameObject.transform.localScale = _scale;
    // }

 
    private IEnumerator ChangedScale(Vector3 startScale, Vector3 endScale)
    {
        float duration = 0.1f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            this.gameObject.transform.localScale = Vector3.Lerp(startScale, endScale, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
    }
}
