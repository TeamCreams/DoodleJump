using System;
using System.Collections;
using System.Collections.Generic;
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
                return 0;
            case EMissionType.AvoidRocksCount:
                return Managers.Game.UserInfo.AccumulatedStone;
            case EMissionType.AchieveScoreInGame:
                return Managers.Game.UserInfo.LatelyScore;
            case EMissionType.Style:
                return 0;
            case EMissionType.RecordScore:
                return 0;
        }
        return 1;
    }
}
