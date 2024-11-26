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
                return Managers.Game.UserInfo.TotalScore;
            case EMissionType.SurviveToLevel:
                return 1;
            case EMissionType.AvoidRocksCount:
                return 1;
            case EMissionType.AchieveScoreInGame:
                return Managers.Game.UserInfo.LatelyScore;
            case EMissionType.Style:
                return 1;
        }
        return 1;
    }
}
