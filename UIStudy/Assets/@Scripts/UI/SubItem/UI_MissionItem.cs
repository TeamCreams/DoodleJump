using Assets.HeroEditor.Common.Scripts.Common;
using Data;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static Define;

public class UI_MissionItem : UI_Base
{
    private enum Texts
    {
        Title_Text,
        Explanation_Text,
        ProgressPercent,
        Complete_Text
    }

    private enum Sliders
    {
        Progress
    }

    private enum Buttons
    {
        Complete_Button
    }

    private int _missionId = 0;
    private Animator _animator = null;
    public override bool Init()
    {
        if (base.Init() == false)
        {
            return false;
        }
        BindTexts(typeof(Texts));
        BindSliders(typeof(Sliders));
        BindButtons(typeof(Buttons));
        Managers.Event.AddEvent(EEventType.SetLanguage, OnEvent_SetLanguage);
        //OnEvent_SetLanguage(null, null);
        GetButton((int)Buttons.Complete_Button).gameObject.BindEvent(OnClick_CompleteButton, EUIEvent.Click);

        _animator = this.GetOrAddComponent<Animator>();
        return true;
    }
    private void OnDestroy()
    {
        Managers.Event.RemoveEvent(EEventType.SetLanguage, OnEvent_SetLanguage);
    }
    private void OnClick_CompleteButton(PointerEventData eventData)
    {        
        //보상
        Debug.Log($"보상 지급");
        Managers.Event.TriggerEvent(EEventType.OnMissionComplete, this, _missionId);
        // refresh
        //_animator.SetTrigger("CompleteMission");
        //Managers.Event.TriggerEvent(EEventType.OnUpdateMission);
        //Managers.Event.TriggerEvent(EEventType.Mission);
    }
    private void SetActiveProgressState()
    {
        GetButton((int)Buttons.Complete_Button).SetActive(false);
        GetSlider((int)Sliders.Progress).SetActive(true);
    }
    private void SetActiveCompleteButton()
    {
        GetButton((int)Buttons.Complete_Button).SetActive(true);
        GetSlider((int)Sliders.Progress).SetActive(false);
    }
    public void SetInfo(int missionId)
    {
        //_animator.SetTrigger("NewMission");
        //Debug.Log($"missionId : {missionId}");
        _missionId = missionId;        
        MissionData missionData = Managers.Data.MissionDataDic[_missionId];
        EMission type = missionData.Type;
        SetLanguage(missionData);
        
        float param1 = Managers.Mission.Dicts[_missionId].Param1;
        float value = param1 / (float)missionData.Param1;
        switch (type)
        {
            case EMission.Level:
                {
                    param1 = Managers.Mission.Dicts[_missionId].Param1;
                    value = param1 / (float)missionData.Param1;
                }
            break;
            case EMission.Shop:
                {
                    param1 = Managers.Data.EvolutionDataDic[Managers.Game.UserInfo.EvolutionId].BuyCount;
                    float remain = param1 % 3;
                    value = 0 < param1 && remain == 0 ? 1 : remain / (float)missionData.Param1;
                    param1 = remain;
                }
            break;
        }

        if(value < 1.0f)
        {
            GetText((int)Texts.ProgressPercent).text = $"{param1}/{missionData.Param1}";
            Debug.Log($"missionData : {missionData.Id}");
            GetSlider((int)Sliders.Progress).value = value;
            SetActiveProgressState();
        }
        else if(1.0f <= value)
        {
            GetSlider((int)Sliders.Progress).value = 1;
            GetText((int)Texts.ProgressPercent).text = $"{param1}/{missionData.Param1}";
            SetActiveCompleteButton();
            // 레벨업 조건 달성 
            // 레벨업은 어디서 관리하는지 
        }
    }

    // 처음 언어 설정을 할 때
    void SetLanguage(MissionData missionData)
    {
        MissionLanguageData missionLanguageData = Managers.Data.MissionLanguageDataDic[missionData.LanguageId];

        switch(Managers.Language.ELanguageInfo)
        {
            case ELanguage.Kr:
            {
                GetText((int)Texts.Title_Text).text = missionLanguageData.KrTitle;
                GetText((int)Texts.Explanation_Text).text = missionLanguageData.KrExplanation;
            }
            break;
            case ELanguage.En:
            {
                GetText((int)Texts.Title_Text).text = missionLanguageData.EnTitle;
                GetText((int)Texts.Explanation_Text).text = missionLanguageData.EnExplanation;
            }
            break;
            default:
            break;
        }
    }

    // 나중에 언어가 바뀔 때
    void OnEvent_SetLanguage(Component sender, object param)
    {
        MissionData missionData = Managers.Data.MissionDataDic[_missionId];
        MissionLanguageData missionLanguageData = Managers.Data.MissionLanguageDataDic[missionData.LanguageId];

        switch(Managers.Language.ELanguageInfo)
        {
            case ELanguage.Kr:
            {
                GetText((int)Texts.Title_Text).text = missionLanguageData.KrTitle;
                GetText((int)Texts.Explanation_Text).text = missionLanguageData.KrExplanation;
            }
            break;
            case ELanguage.En:
            {
                GetText((int)Texts.Title_Text).text = missionLanguageData.EnTitle;
                GetText((int)Texts.Explanation_Text).text = missionLanguageData.EnExplanation;
            }
            break;
            default:
            break;
        }
        GetText((int)Texts.Complete_Text).text = Managers.Language.LocalizedString(91042);
    }
}