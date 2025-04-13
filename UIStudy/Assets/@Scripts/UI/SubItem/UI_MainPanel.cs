using System.Collections;
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

    private string _bestRecord = "최고 기록";
    private string _recentRecord = "최근 기록";

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
        OnEvent_SetLanguage(null, null);


        SetMyScore();
        SetUserScoreList();
        return true;
    }
    private void OnDestroy()
    {
        Managers.Event.RemoveEvent(EEventType.GetUserScoreList, SetUserScoreList);
        Managers.Event.RemoveEvent(EEventType.GetMyScore, SetMyScore);
        Managers.Event.RemoveEvent(EEventType.SetLanguage, OnEvent_SetLanguage);
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
        Debug.Log("SetUserScoreList");
        AllPush();
        var loadingComplete = UI_LoadingPopup.Show();

        Managers.WebContents.GetUserAccountList(null,
       (response) =>
       {
            _userList = response.List;
            foreach (var user in _userList)
            { 
                SpawnRankingItem(user, _rank);
                _rank++;
            }
            _rank = 1;        
            loadingComplete.Value = true;
       },
       (errorCode) =>
       {
            loadingComplete.Value = true;

            UI_ToastPopup.ShowError(Managers.Error.GetError(EErrorCode.ERR_NetworkSettlementError));
            StartCoroutine(LoadScene_Co());
       });
    }

    private void SetMyScore(Component sender = null, object param = null)
    {
        GetText((int)Texts.Best_Text).text = $"{_bestRecord} : {Managers.Game.UserInfo.RecordScore:N0}";
        GetText((int)Texts.Current_Text).text = $"{_recentRecord} : {Managers.Game.UserInfo.LatelyScore:N0}";
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
        GetText((int)Texts.Best_Text).text = $"{_bestRecord} : {Managers.Game.UserInfo.RecordScore:N0}";
        GetText((int)Texts.Current_Text).text = $"{_recentRecord} : {Managers.Game.UserInfo.LatelyScore:N0}";
    }
}
