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
        //_animator.SetTrigger("CompleteMission");
        Managers.Event.TriggerEvent(EEventType.OnUpdateMission);
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
    public void SetInfo(int missionId, int missionStatus)
    {
        //_animator.SetTrigger("NewMission");
        Debug.Log($"missionId : {missionId}");
        _missionId = missionId;
        MissionData missionData = Managers.Data.MissionDataDic[_missionId];
        GetText((int)Texts.Title_Text).text = missionData.Title;
        GetText((int)Texts.Explanation_Text).text = missionData.Explanation;
        
        int missionValue = missionData.MissionType.GetMissionValueByType();
        float value = (float)missionValue / (float)missionData.Param1;
        Debug.Log($"---------------------------------------------≈value : {missionValue} / {missionData.Param1} = {(float)missionValue / (float)missionData.Param1}");
        if(value < 1.0f)
        {
            GetText((int)Texts.ProgressPercent).text = $"{missionValue}/{missionData.Param1}";
            GetSlider((int)Sliders.Progress).value = value;
            SetActiveProgressState();
        }
        else if(1.0f <= value)
        {
            GetSlider((int)Sliders.Progress).value = 1;
            GetText((int)Texts.ProgressPercent).text = $"{missionValue}/{missionData.Param1}";
            SetActiveCompleteButton();
            // GetText((int)Texts.ProgressPercent).text = "달성";
            // 레벨업 조건 달성 
            // 레벨업은 어디서 관리하는지 
        }
    }

    void OnEvent_SetLanguage(Component sender, object param)
    {
        //GetText((int)Texts.Title_Text).text = Managers.Language.LocalizedString();
        //GetText((int)Texts.Explanation_Text).text = Managers.Language.LocalizedString();
        //GetText((int)Texts.ProgressPercent).text = Managers.Language.LocalizedString();
        //GetText((int)Texts.Complete_Text).text = Managers.Language.LocalizedString();
    }
}