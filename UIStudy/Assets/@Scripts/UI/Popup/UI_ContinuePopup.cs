using System.Collections;
using System.Collections.Generic;
using GameApi.Dtos;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static Define;
public class UI_ContinuePopup : UI_PurchasePopupBase
{
    private enum Texts
    {
        Score_Text,
        RecordScore_Text,
        Close_Text,
        Continue_Text,
        Ads_Text
    }

    private enum Images
    {
        Close_Button,
        Continue_Button,
        Ads_Button,
        Circle_Image
    }

    private string _bestRecord = "최고 기록";
    private string _recentRecord = "최근 기록";
    private Coroutine timerCoroutine;
    public override bool Init()
    {
        if (base.Init() == false)
        {
            return false;
        }
        BindTexts(typeof(Texts));
        BindImages(typeof(Images));
        Managers.Event.AddEvent(EEventType.SetLanguage, OnEvent_SetLanguage);

        OnEvent_SetLanguage(null, null);
        GetImage((int)Images.Close_Button).gameObject.BindEvent(OnClick_ClosePopup, EUIEvent.Click);
        GetImage((int)Images.Continue_Button).gameObject.BindEvent(OnEvent_ClickOk, EUIEvent.Click);

        GetText((int)Texts.RecordScore_Text).text = $"{_bestRecord} : {Managers.Game.UserInfo.RecordScore:N0}";
        GetText((int)Texts.Score_Text).text = $"{_recentRecord} : {Managers.Game.UserInfo.LatelyScore:N0}";

        Time.timeScale = 0f;
        FilledImageTimer();
        _gold = HardCoding.ContinueGameGold;
        return true;
    }
    protected override void OnDestroy()
    {
        Managers.Event.RemoveEvent(EEventType.SetLanguage, OnEvent_SetLanguage);
    }
    protected override void OnClick_ClosePopup(PointerEventData eventData)
    {      
        Managers.Event.TriggerEvent(EEventType.OnPlayerDead, this, 0);  
        Managers.UI.ClosePopupUI(this);
        Time.timeScale = 1;
    }
    protected override void OnEvent_ClickOk(PointerEventData eventData)
    {
        var loadingComplete = UI_LoadingPopup.Show();

        //돈 소모
        Managers.Game.GoldTochange = 0;
        int remainingChange = Managers.Game.UserInfo.Gold - _gold;
        if(0 <= remainingChange)
        {
            Managers.Game.GoldTochange = remainingChange;
            UpdateUserGold();
        }
        else
        {
            ShowGoldInsufficientError();
        }
    }

    private void OnClick_ClickAds(PointerEventData eventData)
    {        
        // Advertisement
    }

    private void FilledImageTimer()
    {
        if (timerCoroutine != null)
        {
            StopCoroutine(timerCoroutine);
        }
        timerCoroutine = StartCoroutine(Timer());
    }

    IEnumerator Timer()
    {
        float fromFillAmount = 1f;
        float toFillAmount = 0f;
        float totalDuration = 3f;
        float currentDuration = totalDuration; // 현재 남은 시간 (totalDuration에서 시작)

        GetImage((int)Images.Circle_Image).fillAmount = fromFillAmount;

        while (0f < currentDuration)
        {
            GetImage((int)Images.Circle_Image).fillAmount = Mathf.Lerp(fromFillAmount, toFillAmount, 1f - currentDuration / totalDuration);
            currentDuration -= UnityEngine.Time.deltaTime;
            yield return null;
        }

        GetImage((int)Images.Circle_Image).fillAmount = toFillAmount; // 0으로 설정
    }

    protected override void AfterPurchaseProcess()
    {
        Time.timeScale = 1;
        Managers.Scene.LoadScene(EScene.SuberunkerScene);
    }

    void OnEvent_SetLanguage(Component sender, object param)
    {
        _bestRecord = Managers.Language.LocalizedString(91001);
        _recentRecord = Managers.Language.LocalizedString(91002);
        GetText((int)Texts.Close_Text).text = Managers.Language.LocalizedString(91017);
        GetText((int)Texts.Continue_Text).text = Managers.Language.LocalizedString(91017);
        GetText((int)Texts.Ads_Text).text = Managers.Language.LocalizedString(91018);
    }

}
