using Data;
using GameApi.Dtos;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class UI_MissionPanel : UI_Base
{
    private enum GameObjects
    {
        MissionRoot,

    }
    private enum Buttons
    {
        Cancle_Button,
    }
    private Transform _missionRoot = null;
    private List<GameObject> _itemList = new List<GameObject>();
    private List<ResDtoGetUserMissionListElement> _userMissionList = null;

    public override bool Init()
    {
        if (base.Init() == false)
        {
            return false;
        }
        BindObjects(typeof(GameObjects));
        BindButtons(typeof(Buttons));
        GetButton((int)Buttons.Cancle_Button).gameObject.BindEvent((evt) =>
        {
            this.gameObject.SetActive(false);
        }, Define.EUIEvent.Click);
        _missionRoot = GetObject((int)GameObjects.MissionRoot).transform;
        Managers.Event.AddEvent(Define.EEventType.Mission, SetMissionList);
        return true;
    }
    private void AllPush()
    {
        foreach(var item in _itemList)
        {
            Managers.Resource.Destroy(item.gameObject);
        }
        _itemList.Clear();
    }
    private void SetMissionList(Component sender = null, object param = null)
    {
        AllPush();


        Managers.WebContents.ReqDtoGetUserMissionList(new ReqDtoGetUserMissionList()
        {
            UserAccountId = Managers.Game.UserInfo.UserAccountId
        },
       (response) =>
       {    
            if(response != null)
            {
                _userMissionList = response.List;
                foreach(var mission in _userMissionList)
                {
                    SpawnMissionItem(mission.MissionId, mission.MissionStatus, mission.Param1);
                }
                Debug.Log("is success");
            }
       },
       (errorCode) =>
       {
            Managers.UI.ShowPopupUI<UI_ToastPopup>();
            Managers.Event.TriggerEvent(EEventType.ToastPopupNotice, this, "The settlement could not be processed due to poor network conditions.");
       });
        // 미션 진행을 저장하는 변수가 있어야하는가?
        // 미션을 분리해서 놓고 싶음. enum, level에 따른 미션, 메인미션 분배하는 법
    }

    private void SpawnMissionItem(int missionId, int missionStatus, int param1)
    {
        // 스폰 조건이 안되면 스폰안되도록 세팅
        MissionData missionData = Managers.Data.MissionDataDic[missionId];
        if (missionData.PrevMissionId != 0)
        {
            // 내가가지고있는 미션들중에 그 아이디가 없으면
            return;
        }

        var item = Managers.UI.MakeSubItem<UI_MissionItem>(parent: _missionRoot, pooling: true);
        item.SetInfo(missionId, missionStatus, param1);
        _itemList.Add(item.gameObject);
    }
}
