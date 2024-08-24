using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UI_TinyFarmScene : UI_Scene
{

    enum GameObjects
    {
        Missions,
        Content
    }

    enum Buttons
    {
        Shop_Button,
        Quest_Button,
        Event_Button,
        Dongeon_Button,
        Setup_Button,
        Back_Button
    }


    private GameObject _root = null;
    private GameObject _missions = null;
    

    protected override void Init()
    {
        base.Init();
        BindObjects(typeof(GameObjects));
        BindButtons(typeof(Buttons));
        _root = GetObject((int)GameObjects.Content);
        _missions = GetObject((int)GameObjects.Missions);
        StartLoadAssets();

        this.Get<Button>((int)Buttons.Quest_Button).gameObject.BindEvent((evt) =>
        {
            _missions.SetActive(true);
            //_missions.SetActive(!_missions.activeSelf);
        }, Define.EUIEvent.Click);

        this.Get<Button>((int)Buttons.Back_Button).gameObject.BindEvent((evt) =>
        {
            _missions.SetActive(false);
        }, Define.EUIEvent.Click);

    }

    void StartLoadAssets()
    {
        Managers.Resource.LoadAllAsync<UnityEngine.Object>("PreLoad", (key, count, totalCount) =>
        {
            Debug.Log($"{key} {count}/{totalCount}");

            if (count == totalCount)
            {
                Debug.Log("Load Complete");
                Managers.Data.Init();
                Managers.UI.MakeSubItem<UI_TinyFarmMission>("UI_Mission", _root.transform);
            }
        });
    }
}
