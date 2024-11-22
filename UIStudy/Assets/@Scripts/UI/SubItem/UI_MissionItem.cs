using UnityEngine;
using UnityEngine.UI;

public class UI_MissionItem : UI_Base
{
 
    private enum Texts
    {
        Title_Text,
        Explanation_Text,
        ProgressPercent
    }

    private enum Sliders
    {
        Progress
    }
    public override bool Init()
    {
        if (base.Init() == false)
        {
            return false;
        }
        BindTexts(typeof(Texts));
        BindSliders(typeof(Sliders));
        return true;
    }

    public void SetInfo(int missionId, int missionStatus, int param1)
    {
        Debug.Log($"missionId : {missionId}");
        GetText((int)Texts.Title_Text).text = Managers.Data.MissionDataDic[missionId].Title;
        GetText((int)Texts.Explanation_Text).text = Managers.Data.MissionDataDic[missionId].Explanation;
        
        if((float)((float)param1 / Managers.Data.MissionDataDic[missionId].Param1) < 1.0f)
        {
            GetText((int)Texts.ProgressPercent).text = $"{param1}/{Managers.Data.MissionDataDic[missionId].Param1}";
            GetSlider((int)Sliders.Progress).value = (float)((float)param1 / Managers.Data.MissionDataDic[missionId].Param1);
        }
        else if(1.0f <= (float)((float)param1 / Managers.Data.MissionDataDic[missionId].Param1))
        {
            GetText((int)Texts.ProgressPercent).text = "달성";
            GetSlider((int)Sliders.Progress).value  = 1;
            // 레벨업 조건 달성 
            // 레벨업은 어디서 관리하는지 
        }
    }
}
