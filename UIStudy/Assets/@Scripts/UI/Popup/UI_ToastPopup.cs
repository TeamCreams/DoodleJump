using Data;
using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using DG.Tweening;
using static Define;

//SYSTEM : 곧 점검이 시작됩니다. (INFO)
//SYSTEM : 돈이 부족합니다. (WARNING)
//SYSTEM : 네트워크가 불안정합니다. (ERROR)

public class UI_ToastPopup : UI_Popup
{
    public enum Type
    {
        Debug,
        Info,
        Warning,
        Error,
        Critical
    }
    private enum Images
    {
        Background_Image,
    }
    private enum Texts
    {
        Notice_Text,
    }
    //private string _notice;
    private Type _type;
    private float _time;
    private static int _order = 1000;
    public static Queue<UI_ToastPopup> _queue = new();
    Sequence _showSeq;
    public override bool Init()
    {
        if (base.Init() == false)
        {
            return false;
        }
        BindImages(typeof(Images));
        BindTexts(typeof(Texts));

        Vector3 originalScale = this.transform.localScale;
        float scaleSpeed = 0.1f;
        _showSeq = DOTween.Sequence()
            .Append(GetImage((int)Images.Background_Image).transform.DOScale(new Vector3(originalScale.x + 0.5f, originalScale.y + 0.5f, originalScale.z + 0.5f), scaleSpeed).SetEase(Ease.Linear))
            .Append(GetImage((int)Images.Background_Image).transform.DOScale(originalScale, scaleSpeed).SetEase(Ease.Linear));


        return true;
    }

    private void SetInfo(string notice, Type type = Type.Info, float time = 2f, Action onCompleteCallback = null)
    {
        GetText((int)Texts.Notice_Text).text = notice;
        _type = type;
        _time = time;
        SetBackgroundColor();
        StartCoroutine(ToastPopup_Co(onCompleteCallback));
    }
    private void SetBackgroundColor()
    {
        Color color = new();
        switch (_type)
        {
            case Type.Debug:
                color = ToastPopupColor.DebugColor;
                break;
            case Type.Info:
                color = ToastPopupColor.InfoColor;
                break;
            case Type.Warning:
                color = ToastPopupColor.WarningColor;
                break;
            case Type.Error:
                color = ToastPopupColor.ErrorColor;
                break;
            case Type.Critical:
                color = ToastPopupColor.CriticalColor;
                break;

            default:
                color = ToastPopupColor.InfoColor;
                break;
        }
        GetImage((int)Images.Background_Image).color = color;
    }

    public static void ShowInfo(NoticeInfo noticeInfo, float time = 2f, Action onCompleteCallback = null)
    {
        UI_ToastPopup toast = ShowToastPopupUI();
        toast.SetInfo(noticeInfo.Notice, UI_ToastPopup.Type.Info, time, onCompleteCallback);
    }
    public static void ShowWarning(NoticeInfo noticeInfo, float time = 2f, Action onCompleteCallback = null)
    {
        UI_ToastPopup toast = ShowToastPopupUI();
        toast.SetInfo(noticeInfo.Notice, UI_ToastPopup.Type.Warning, time, onCompleteCallback);
    }
    public static void ShowError(NoticeInfo noticeInfo, float time = 2f, Action onCompleteCallback = null)
    {
        UI_ToastPopup toast = ShowToastPopupUI();
        toast.SetInfo(noticeInfo.Notice, UI_ToastPopup.Type.Error, time, onCompleteCallback);
    }
    public static void ShowCritical(NoticeInfo noticeInfo, float time = 2f, Action onCompleteCallback = null)
    {
        UI_ToastPopup toast = ShowToastPopupUI();
        toast.SetInfo(noticeInfo.Notice, UI_ToastPopup.Type.Critical, time, onCompleteCallback);
    }
    
    public static void Show(NoticeInfo noticeInfo, float time = 2f, Action onCompleteCallback = null)
    {
#if !UNITY_EDITOR
        if(noticeInfo.Type == NoticeInfo.EType.Debug)
            return;
#endif

        UI_ToastPopup toast = ShowToastPopupUI();

        switch (noticeInfo.Type)
        {
            case NoticeInfo.EType.Debug:
                toast.SetInfo(noticeInfo.Notice, Type.Debug, time, onCompleteCallback);
                break;
            case NoticeInfo.EType.Info:
                toast.SetInfo(noticeInfo.Notice, Type.Info, time, onCompleteCallback);
                break;
            case NoticeInfo.EType.Warning:
                toast.SetInfo(noticeInfo.Notice, Type.Warning, time, onCompleteCallback);
                break;
            case NoticeInfo.EType.Error:
                toast.SetInfo(noticeInfo.Notice, Type.Error, time, onCompleteCallback);
                break;
            case NoticeInfo.EType.Critical:
                toast.SetInfo(noticeInfo.Notice, Type.Critical, time, onCompleteCallback);
                break;
            default:
                toast.SetInfo(noticeInfo.Notice, Type.Info, time, onCompleteCallback);
                break;
        }
    }

    public static void Show(string message, Type type, float time = 2f, Action onCompleteCallback = null)
    {
#if !UNITY_EDITOR
        if(type == Type.Debug)
            return;
#endif

#if UNITY_EDITOR
        if (type == Type.Debug)
        {
            message = $"[DEBUG] {message}";
        }
#endif
        UI_ToastPopup toast = ShowToastPopupUI();
        toast.SetInfo(message, type, time, onCompleteCallback);
    }

    public class ToastPopupColor
    {
        public static readonly Color DebugColor = new Color(0, 1, 0, 1f);                                       // Green
        public static readonly Color InfoColor = new Color(255f / 255f, 246f / 255f, 225f / 255f, 1f);          // White
        public static readonly Color WarningColor = new Color(255f / 255f, 186f / 255f, 28f / 255f, 1f);        // Yellow
        public static readonly Color ErrorColor = new Color(159f / 255f, 159f / 255f, 159f / 255f, 1f);         // Gray
        public static readonly Color CriticalColor = new Color(255f / 255f, 177f / 255f, 177f / 255f, 1f);      // Red
    }
    public static UI_ToastPopup ShowToastPopupUI()
    {
        var go = Managers.Resource.Instantiate("UI_ToastPopup", Managers.UI.Root.transform);
        if (go == null)
        {
            Debug.Log($"resource not found UI_ToastPopup]");
        }
        var rv = go.GetComponent<UI_ToastPopup>();
        rv.SetOrder(_order--);
        _queue.Enqueue(rv);
        return rv;
    }
    public void CloseToastPopupUI()
    {
        if (_queue.Count == 0)
        {
            return;
        }

        var peek = _queue.Peek();
        if (peek != this)
        {
            Debug.Log("삭제할 수 없습니다.");
            return;
        }
        _queue.Dequeue();
        if (_queue.Count == 0)
        {
            _order = 1000;
        }
        GameObject.Destroy(this.gameObject);
    }
    public IEnumerator ToastPopup_Co(Action onCompleteCallback = null)
    {
        this.Hide();
        while (0 < _queue.Count)
        {
            var toast = _queue.Peek(); 
            
            if (toast != null && toast == this)
            {
                Show();
                yield return new WaitForSeconds(_time);
                CloseToastPopupUI();
                onCompleteCallback?.Invoke();
            }
            yield return new WaitUntil(() => _queue.Count == 0 || _queue.Peek() == this);
        }
    }
    public override void SetOrder(int sortOrder)
    {
        this.GetComponent<Canvas>().sortingOrder = sortOrder;
    }

    Canvas _rootCanvas = null;
    public void Show()
    {
        _rootCanvas = this.GetComponent<Canvas>();
        _rootCanvas.enabled = true;

        _showSeq.PlayForward();
    }

    public void Hide()
    {
        _rootCanvas = this.GetComponent<Canvas>();
        _rootCanvas.enabled = false;
    }
}
