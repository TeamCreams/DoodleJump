using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager
{
    GameObject _root = null;

    GameObject Root
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

    public T MakeSubItem<T>(string name = null, Transform parent = null) where T : UI_Base
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

        var go = Managers.Resource.Instantiate(name, parent);
        if (go == null)
        {
            Debug.Log($"resource not found [{name}]");
        }

        var rv = go.GetOrAddComponent<T>();

        return rv;
    }

}

