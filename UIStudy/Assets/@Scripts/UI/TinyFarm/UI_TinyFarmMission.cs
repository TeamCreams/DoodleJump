using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_TinyFarmMission : UI_Base
{
    enum GameObjects
    {
        MissionsRoot
    }

    private GameObject _root = null;

    protected override void Init()
    {
        base.Init();
        BindObjects(typeof(GameObjects));

        _root = GetObject((int)GameObjects.MissionsRoot);
        int count = Managers.Data.TinyFarmDic.Count;

        foreach(var tinyFarmMissionData in Managers.Data.TinyFarmDic)
        {
            UI_TinyFarmMissionSlot slot3 = Managers.UI.MakeSubItem<UI_TinyFarmMissionSlot>(parent:_root.transform);
            slot3.SetInfo(tinyFarmMissionData.Value);
        }

        //for (int i = 1; i <= count; i++)
        //{
        //    //var slot1 = GameObject.Instantiate(_missionUI, _UIRoot.transform); // X
        //    //var slot2 = Managers.UI.MakeSubItem<UI_TinyFarmMissionSlot>("UI_MissionSlot", _root.transform);
        //    //slot2.SetInfo(Managers.Data.TinyFarmDic[i]);
        //    var slot3 = Managers.Resource.Instantiate("UI_MissionSlot", _root.transform);
        //    slot3.GetOrAddComponent<UI_TinyFarmMissionSlot>().SetInfo(Managers.Data.TinyFarmDic[i]);
            
        //}
    }

}
