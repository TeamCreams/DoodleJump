﻿using System.Collections;
using System.Collections.Generic;
using GameApi.Dtos;
using UnityEngine;
using static Define;

public class UI_MainPanel : UI_Base
{
    private enum GameObjects
    {
        RankingRoot,
    }

    private enum Texts
    {
        Best_Text,
        Current_Text,
    }

    private List<GameObject> _itemList = new List<GameObject>();
    private Transform _rankingRoot = null;
    List<ResDtoGetUserAccountListElement> _userList = null;
    private int _rank = 1; 

    private string _minutesString = "분";
    private string _secondsString = "초";
    private string _bestRecord = "최고 기록";
    private string _recentRecord = "최근 기록";
    private int _recordMinutes;
    private float _recordSeconds;
    private int _minutes;
    private float _seconds;

     public override bool Init()
    {
        if (base.Init() == false)
        {
            return false;
        }
        BindObjects(typeof(GameObjects));
        BindTexts(typeof(Texts));
        _rankingRoot = GetObject((int)GameObjects.RankingRoot).transform;
        Managers.Event.AddEvent(EEventType.GetUserScoreList, SetUserScoreList);
        Managers.Event.AddEvent(EEventType.GetMyScore, SetMyScore);
        Managers.Event.AddEvent(EEventType.SetLanguage, OnEvent_SetLanguage);
        Managers.Event.TriggerEvent(EEventType.SetLanguage);


        SetMyScore();
        SetUserScoreList();
        return true;
    }

    private void AllPush()
    {
        foreach(var _item in _itemList)
        {
            Managers.Resource.Destroy(_item.gameObject);
        }
        _itemList.Clear();
    }

    private void SetUserScoreList(Component sender = null, object param = null)
    {
        AllPush();
        Managers.Resource.Instantiate("UI_Loading", this.transform);
        Managers.Event.TriggerEvent(EEventType.StartLoading);
        Managers.WebContents.ReqGetUserAccountList(null,
       (response) =>
       {
            _userList = response.List;
            foreach (var user in _userList)
            { 
                SpawnRankingItem(user, _rank);
                _rank++;
            }
            _rank = 1;
            Managers.Event.TriggerEvent(EEventType.StopLoading);
       },
       (errorCode) =>
       {
            Managers.UI.ShowPopupUI<UI_ToastPopup>();
            Managers.Event.TriggerEvent(EEventType.ToastPopupNotice, this, "The settlement could not be processed due to poor network conditions.");
            StartCoroutine(LoadScene_Co());
       });
    }

    private void SetMyScore(Component sender = null, object param = null)
    {
        _recordMinutes = Managers.Game.UserInfo.RecordScore / 60;
        _recordSeconds = Managers.Game.UserInfo.RecordScore % 60;
        GetText((int)Texts.Best_Text).text = $"{_bestRecord} : {_recordMinutes}{_minutesString} {_recordSeconds}{_secondsString}";

        _minutes = Managers.Game.UserInfo.LatelyScore / 60;
        _seconds = Managers.Game.UserInfo.LatelyScore % 60;
        GetText((int)Texts.Current_Text).text = $"{_recentRecord} : {_minutes}{_minutesString} {_seconds}{_secondsString}";
    }
    private void SpawnRankingItem(ResDtoGetUserAccountListElement element, int rank)
    {
        var item = Managers.UI.MakeSubItem<UI_RankingItem>(parent: _rankingRoot, pooling: true);
        item.SetInfo(element, rank);
        _itemList.Add(item.gameObject);
    }

    IEnumerator LoadScene_Co()
    {
        yield return new WaitForSeconds(2.5f);
        Managers.Scene.LoadScene(EScene.SuberunkerSceneHomeScene);
    }

    void OnEvent_SetLanguage(Component sender, object param)
    {
        _bestRecord = Managers.Language.LocalizedString(91001);
        _recentRecord = Managers.Language.LocalizedString(91002);
        _minutesString = Managers.Language.LocalizedString(91004);
        _secondsString = Managers.Language.LocalizedString(91005);
        GetText((int)Texts.Best_Text).text = $"{_bestRecord} : {_recordMinutes}{_minutesString} {_recordSeconds}{_secondsString}";
        GetText((int)Texts.Current_Text).text = $"{_recentRecord} : {_minutes}{_minutesString} {_seconds}{_secondsString}";
    }
}