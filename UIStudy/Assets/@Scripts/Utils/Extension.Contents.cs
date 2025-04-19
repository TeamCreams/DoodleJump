using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using static Define;

public static partial class Extension
{
    public static int GetMissionValueByType(this EMissionType type)
    {
        switch (type)
        {
            case EMissionType.Time:
                return Managers.Game.UserInfo.PlayTime;
            case EMissionType.SurviveToLevel:
                return Managers.Game.UserInfo.StageLevel; // 이거 서버에 추가하기. 게임 score에 추가하기.
            case EMissionType.AvoidRocksCount:
                return Managers.Game.UserInfo.AccumulatedStone;
            // case EMissionType.AchieveScoreInGame:
            //     return 0;
            case EMissionType.Style:
                {
                    int value = Managers.Game.ChracterStyleInfo.IsChangedStyle; // 0 = false, 1 = true
                    Managers.Game.ChracterStyleInfo.IsChangedStyle = 0;
                    return value;
                }
            case EMissionType.RecordScore:
                return Managers.Game.UserInfo.RecordScore;
            case EMissionType.Evolution:
                return Managers.Game.UserInfo.EvolutionSetLevel;
        }
        return 1;
    }

    public static Dictionary<int, T> ListToDict<T>(List<T> list)
    {
        return list.Select((value, index) => new { Key = index, Value = value })
                   .ToDictionary(x => x.Key, x => x.Value);
    }

    
}
