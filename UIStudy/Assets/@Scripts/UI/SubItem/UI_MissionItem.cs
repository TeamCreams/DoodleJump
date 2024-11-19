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

    private enum Images
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
        BindImages(typeof(Images));
        return true;
    }

    public void SetInfo(int id)
    {
        GetText((int)Texts.Title_Text).text = Managers.Data.MissionDataDic[id].Title;
        GetText((int)Texts.Explanation_Text).text = Managers.Data.MissionDataDic[id].Explanation;
        
        if((float)(1.0f / Managers.Data.MissionDataDic[id].Param1) < 1.0f)
        {
            GetText((int)Texts.ProgressPercent).text = $"{0}/{Managers.Data.MissionDataDic[id].Param1}";
            GetImage((int)Images.Progress).fillAmount = (float)(1.0f / Managers.Data.MissionDataDic[id].Param1);
        }
        else if(1.0f <= (float)(1.0f / Managers.Data.MissionDataDic[id].Param1))
        {
            GetText((int)Texts.ProgressPercent).text = "달성";
            GetImage((int)Images.Progress).fillAmount  = 1;
            // 레벨업 조건 달성 
            // 레벨업은 어디서 관리하는지 
        }
    }
}
