using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class UIManager
{
    GameObject _root = null;

    public GameObject Root
    {
        get
        {
            if(_root == null)
            {
                _root = GameObject.Find("@UI_Root");
                if (_root == null)
                {
                    _root = new GameObject("@UI_Root");
                }
            }
            return _root;
        }
    }
    // toast Popup
    // private int _toastOrder = 1000;
    // public Queue<UI_ToastPopup> _toastQueue = new();
    //
    private int _popupOrder = 100;
    private Stack<UI_Popup> _popupStacks = new Stack<UI_Popup>();

    public T ShowSceneUI<T>(string name = null) where T : UI_Scene
    {
        if (string.IsNullOrEmpty(name))
        {
            name = typeof(T).ToString();
        }
        //string resourcePath = $"UI/Scene/{name}";


        var go = Managers.Resource.Instantiate(name, Root.transform);
        if (go == null)
        {
            Debug.Log($"resource not found [{name}]");
        }

        var rv = go.GetOrAddComponent<T>();

        return rv;
    }

    public T ShowUIBase<T>(string name = null) where T : UI_Base
	{
        if (string.IsNullOrEmpty(name))
        {
            name = typeof(T).ToString();
        }
        //string resourcePath = $"UI/Scene/{name}";


        var go = Managers.Resource.Instantiate(name, Root.transform);
        if (go == null)
        {
            Debug.Log($"resource not found [{name}]");
        }

        var rv = go.GetOrAddComponent<T>();

        return rv;
    }

    public T ShowPopupUI<T>(string name = null) where T : UI_Popup
    {
        if(string.IsNullOrEmpty(name))
        {
            name = typeof(T).ToString();
        }
        //string resourcePath = $"UI/Popup/{name}";

        var go = Managers.Resource.Instantiate(name, Root.transform);
        if (go == null)
        {
            Debug.Log($"resource not found [{name}]");
        }

        var rv = go.GetOrAddComponent<T>();
        Debug.Log($"name : {rv.name}");
        rv.SetOrder(_popupOrder++);

        _popupStacks.Push(rv);

        return rv;
    }

    public void ClosePopupUI()
    {
        if(_popupStacks.Count == 0)
        {
            return;
        }

        var popup = _popupStacks.Peek();
        GameObject.Destroy(popup);
        _popupOrder--;
    }

    public void ClosePopupUI(UI_Popup popup)
    {
        if (_popupStacks.Count == 0)
        {
            return;
        }

        var peek = _popupStacks.Peek();
        if (peek != popup)
        {
            Debug.Log("삭제할 수 없습니다.");
            return;
        }
        _popupStacks.Pop();
        GameObject.Destroy(popup.gameObject);
        _popupOrder--;
    }

    public T MakeSubItem<T>(string name = null, Transform parent = null, bool pooling = false) where T : UI_Base
    {
        if (string.IsNullOrEmpty(name))
        {
            name = typeof(T).ToString();
        }
        string resourcePath = $"UI/SubItem/{name}";

        if(parent == null)
        {
            parent = Root.transform;
        }

        var go = Managers.Resource.Instantiate(name, parent, pooling);
        if (go == null)
        {
            Debug.Log($"resource not found [{name}]");
        }

        var rv = go.GetOrAddComponent<T>();

        return rv;
    }

    public T GetSceneUI<T>() where T : UI_Scene
    {
        var rv = Util.FindChild<T>(Root);
        return rv;
    }

    // public UI_ToastPopup ShowToastPopupUI<T>(string name = null) where T : UI_Popup
    // {
    //     if(string.IsNullOrEmpty(name))
    //     {
    //         name = typeof(T).ToString();
    //     }

    //     var go = Managers.Resource.Instantiate(name, Root.transform);
    //     if (go == null)
    //     {
    //         Debug.Log($"resource not found [{name}]");
    //     }

    //     var rv = go.GetOrAddComponent<UI_ToastPopup>();
    //     rv.SetOrder(_toastOrder--);
    //     _toastQueue.Enqueue(rv);
    //     return rv;
    // }
    // public void CloseToastPopupUI(UI_ToastPopup popup)
    // {
    //     if (_toastQueue.Count == 0)
    //     {
    //         return;
    //     }

    //     var peek = _toastQueue.Peek();
    //     if (peek != popup)
    //     {
    //         Debug.Log("삭제할 수 없습니다.");
    //         return;
    //     }
    //     _toastQueue.Dequeue();
    //     if (_toastQueue.Count == 0)
    //     {
    //         _toastOrder = 1000;
    //     }
    //     GameObject.Destroy(popup.gameObject);
    // }
    // public IEnumerator ToastPopup_Co(UI_ToastPopup popup, Action onCompleteCallback = null)
    // {
    //     while (0 < _toastQueue.Count)
    //     {
    //         var toast = _toastQueue.Peek(); 
            
    //         if (toast != null && toast == popup)
    //         {
    //             yield return new WaitForSeconds(popup.Time);
    //             CloseToastPopupUI(popup);
    //             onCompleteCallback?.Invoke();
    //         }
    //         yield return new WaitUntil(() => _toastQueue.Count == 0 || _toastQueue.Peek() == popup);
    //     }
    // }
}

