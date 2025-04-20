using UnityEngine;
using System.Collections;
using System;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.Playables;

public class UI_Base : InitBase
{

    protected Dictionary<Type, UnityEngine.Object[]> _objects = new Dictionary<Type, UnityEngine.Object[]>();

    protected void Awake()
    {
        this.Init();
    }

    public override bool Init()
    {
        if (base.Init() == false)
        {
            return false;
        }

        return true;
    }

    protected void Bind<T>(Type type) where T : UnityEngine.Object
    {
        string[] names = Enum.GetNames(type);
        UnityEngine.Object[] objects = new UnityEngine.Object[names.Length];
        _objects.Add(typeof(T), objects);

        for (int i = 0; i < names.Length; i++)
        {
            if (typeof(T) == typeof(GameObject))
                objects[i] = Util.FindChild(gameObject, names[i].Trim(), true);
            else
                objects[i] = Util.FindChild<T>(gameObject, names[i].Trim(), true);

            if (objects[i] == null)
                Debug.Log($"Failed to bind({names[i].Trim()})");
        }
    }

    protected void BindObjects(Type type) { Bind<GameObject>(type); }
    protected void BindImages(Type type) { Bind<Image>(type); }
    protected void BindTexts(Type type) { Bind<TMP_Text>(type); }
    protected void BindButtons(Type type) { Bind<Button>(type); }
    protected void BindToggles(Type type) { Bind<Toggle>(type); }
    protected void BindInputFields(Type type) { Bind<TMP_InputField>(type); }
    protected void BindSliders(Type type) { Bind<Slider>(type); }
    protected void BindLegacyTexts(Type type) { Bind<Text>(type); }
    protected void BindLegacyInputFields(Type type) { Bind<InputField>(type); }
    protected void BindPlayableDirector(Type type) { Bind<PlayableDirector>(type); }
    

    protected T Get<T>(int idx) where T : UnityEngine.Object
    {
        UnityEngine.Object[] objects = null;
        if (_objects.TryGetValue(typeof(T), out objects) == false)
            return null;

        return objects[idx] as T;
    }

    protected GameObject GetObject(int idx) { return Get<GameObject>(idx); }
    protected TMP_Text GetText(int idx) { return Get<TMP_Text>(idx); }
    protected Button GetButton(int idx) { return Get<Button>(idx); }
    protected Image GetImage(int idx) { return Get<Image>(idx); }
    protected Toggle GetToggle(int idx) { return Get<Toggle>(idx); }
    protected TMP_InputField GetInputField(int idx) { return Get<TMP_InputField>(idx); }
    protected Slider GetSlider(int idx) { return Get<Slider>(idx); }

    // FIX : 현재 ChooseStats에서 사용되는 Text와 InputField는 Legacy(옛날거) 라서 이거로 사용해야함.
    protected Text GetLegacyText(int idx) { return Get<Text>(idx); }
    protected InputField GetLegacyInputField(int idx) { return Get<InputField>(idx); }
    protected PlayableDirector GetPlayableDirector(int idx) { return Get<PlayableDirector>(idx); }

    public static void BindEvent(GameObject go, Action<PointerEventData> action = null, Define.EUIEvent type = Define.EUIEvent.Click)
    {
        UI_EventHandler evt = Util.GetOrAddComponent<UI_EventHandler>(go);

        switch (type)
        {
            case Define.EUIEvent.PointerUp:
                evt.OnPointerUpHandler -= action;
                evt.OnPointerUpHandler += action;
                break;
            case Define.EUIEvent.PointerDown:
                evt.OnPointerDownHandler -= action;
                evt.OnPointerDownHandler += action;
                break;
            case Define.EUIEvent.Click:
                evt.OnClickHandler -= action;
                evt.OnClickHandler += action;
                break;
            case Define.EUIEvent.BeginDrag:
                evt.OnBeginDragHandler -= action;
                evt.OnBeginDragHandler += action;
                break;
            case Define.EUIEvent.Drag:
                evt.OnDragHandler -= action;
                evt.OnDragHandler += action;
                break;
            case Define.EUIEvent.EndDrag:
                evt.OnEndDragHandler -= action;
                evt.OnEndDragHandler += action;
                break;
            case Define.EUIEvent.PointerEnter:
                evt.OnEndDragHandler -= action;
                evt.OnEndDragHandler += action;
                break;
            case Define.EUIEvent.PointerExit:
                evt.OnEndDragHandler -= action;
                evt.OnEndDragHandler += action;
                break;
        }
    }

    public static void ClearEvent(GameObject go, Action<PointerEventData> action = null, Define.EUIEvent type = Define.EUIEvent.Click)
    {
        UI_EventHandler evt = Util.GetOrAddComponent<UI_EventHandler>(go);

        switch (type)
        {
            case Define.EUIEvent.PointerUp:
                evt.OnPointerUpHandler = null;
                break;
            case Define.EUIEvent.PointerDown:
                evt.OnPointerDownHandler = null;
                break;
            case Define.EUIEvent.Click:
                evt.OnClickHandler = null;
                break;
            case Define.EUIEvent.BeginDrag:
                evt.OnBeginDragHandler = null;
                break;
            case Define.EUIEvent.Drag:
                evt.OnDragHandler = null;
                break;
            case Define.EUIEvent.EndDrag:
                evt.OnEndDragHandler = null;
                break;
            case Define.EUIEvent.PointerEnter:
                evt.OnEndDragHandler = null;
                break;
            case Define.EUIEvent.PointerExit:
                evt.OnEndDragHandler = null;
                break;
        }
    }

}

