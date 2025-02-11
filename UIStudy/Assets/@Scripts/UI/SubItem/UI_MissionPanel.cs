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
    private Dictionary<int, int> _missionDic = new Dictionary<int, int>();
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
        }, EUIEvent.Click);
        _missionRoot = GetObject((int)GameObjects.MissionRoot).transform;
        Managers.Event.AddEvent(EEventType.Mission, SetMissionList);
        return true;
    }
    private void OnDestroy()
    {
        Managers.Event.RemoveEvent(EEventType.Mission, SetMissionList);
    }
    private void AllPush()
    {
        foreach(var item in _itemList)
        {
            Managers.Resource.Destroy(item.gameObject);
        }
        _itemList.Clear();
        //_missionDic.Clear();
    }
    private void SetMissionList(Component sender = null, object param = null)
    {
        AllPush();

        foreach (var missionKeyValue in Managers.Mission.Dicts)
        {
            int missionId = missionKeyValue.Key;
            ResDtoGetUserMissionListElement mission = missionKeyValue.Value;

            //TODO : 이부분 수정필요
            // 퀘스트 완료와 동시에 신규퀘스트를 수락하도록.
            // prevId가 업데이트가 안 되어있음.
            if (_missionDic.ContainsKey(mission.MissionId))
            {
                _missionDic[mission.MissionId] = mission.MissionStatus; // 상태 업데이트
            }
            else
            {
                _missionDic.Add(mission.MissionId, mission.MissionStatus); // 새 미션 추가
            }
            SpawnMissionItem(mission.MissionId, mission.MissionStatus);
        }

        // 미션 진행을 저장하는 변수가 있어야하는가?
        // 미션을 분리해서 놓고 싶음. enum, level에 따른 미션, 메인미션 분배하는 법
    }
    private void SpawnMissionItem(int missionId, int missionStatus)
    {   
        // 스폰 조건이 안되면 스폰안되도록 세팅
        MissionData missionData = Managers.Data.MissionDataDic[missionId];

        if(missionStatus == (int)EMissionStatus.Complete) // 2 == MissionComplete
        {
            // 미션이 이미 완료 상태라면
            return;
        }
        foreach(var prevId in missionData.PrevMissionId)
        {
            if (prevId != 0 && !_missionDic.ContainsKey(prevId))
            {
                // 이전 미션 ID가 Dictionary에 없으면 처리
                return;
            }
            if (prevId != 0 && _missionDic[prevId] != (int)EMissionStatus.Complete)
            {
                // 이전 미션이 완료되지 않았다면 처리
                return;
            }
        }
        var item = Managers.UI.MakeSubItem<UI_MissionItem>(parent: _missionRoot, pooling: true);
        item.SetInfo(missionId, missionStatus);
        _itemList.Add(item.gameObject);
    }
}