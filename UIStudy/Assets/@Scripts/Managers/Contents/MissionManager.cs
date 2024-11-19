using UnityEngine;
using static Define;
public class MissionManager
{
    public void Init()
    {
        
    }

    public void KindOfMission(Define.EMissionType missionType, int param1 = 0, string param2 = null)
    {
        switch(missionType)
        {
            case EMissionType.Time:
            break;
            case EMissionType.AvoidRocksForMinutes:
            break;
            case EMissionType.SurviveToLevel:
            break;
            case EMissionType.CollectAllThoughtBubbles:
            break;
            case EMissionType.StayStillForMinutes:
            break;
            case EMissionType.AchieveLevelWithoutCollectingItems:
            break;
            case EMissionType.AvoidRocksCount:
            break;
            case EMissionType.MoveOneWayForMinutes:
            break;
            case EMissionType.AchieveScoreInGame:
            break;
            case EMissionType.ChangeBaseStats:
            break;
            case EMissionType.CollectGoldInGame:
            break;
            case EMissionType.AchieveLuckInGame:
            break;
            case EMissionType.TeleportAccumulatedNTimes:
            break;
            case EMissionType.PlayerToLevel24:
            break;
        }
    }

    private void OnEvent_Time(Component sender, object param)
    {
        
    }
    private void OnEvent_AvoidRocksForMinutes(Component sender, object param)
    {

    }
    private void OnEvent_SurviveToLevel(Component sender, object param)
    {

    }
    private void OnEvent_CollectAllThoughtBubbles(Component sender, object param)
    {

    }
    private void OnEvent_StayStillForMinutes(Component sender, object param)
    {

    }
    private void OnEvent_AchieveLevelWithoutCollectingItems(Component sender, object param)
    {

    }
    private void OnEvent_AvoidRocksCount(Component sender, object param)
    {

    }
    private void OnEvent_MoveOneWayForMinutes(Component sender, object param)
    {

    }
    private void OnEvent_AchieveScoreInGame(Component sender, object param)
    {

    }
    private void OnEvent_ChangeBaseStats(Component sender, object param)
    {

    }
    private void OnEvent_CollectGoldInGame(Component sender, object param)
    {

    }
    private void OnEvent_AchieveLuckInGame(Component sender, object param)
    {

    }
    private void OnEvent_TeleportAccumulatedNTimes(Component sender, object param)
    {

    }
    private void OnEvent_PlayerToLevel24(Component sender, object param)
    {

    }

}
