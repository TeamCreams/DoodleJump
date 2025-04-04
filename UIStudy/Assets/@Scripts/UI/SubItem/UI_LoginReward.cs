using System;
using System.Collections;
using GameApi.Dtos;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static Define;

public class UI_LoginReward : UI_Base
{
    private enum Texts
    {
        RewardResetTimer_Text,
        Price_Text,
        Title_Text
    }
    private enum Images
    {
        UI_Reward
    }
    private ScrollRect _parentScrollRect = null;
    private DateTime _nextRewardTime;
    private TimeSpan _chargeTime;
    public override bool Init()
    {
        if (base.Init() == false)
        {
            return false;
        }
        BindTexts(typeof(Texts));
        BindImages(typeof(Images));

        GetImage((int)Images.UI_Reward).gameObject.BindEvent(GetReward, EUIEvent.Click);
        this.gameObject.BindEvent(OnBeginDrag, EUIEvent.BeginDrag);
        this.gameObject.BindEvent(OnDrag, EUIEvent.Drag);
        this.gameObject.BindEvent(OnEndDrag, EUIEvent.EndDrag);
        
        Managers.SignalR.OnChangedHeartBeat -= CheckServerTime; // 구독 해제
        Managers.SignalR.OnChangedHeartBeat += CheckServerTime; // 이벤트 구독

        return true;
    }

    private void OnDestroy()
    {
        Managers.SignalR.OnChangedHeartBeat -= CheckServerTime; // 구독 해제
    }
    public void SetInfo()
    {
        GetText((int)Texts.RewardResetTimer_Text).text = "시간 계산 중";
        _parentScrollRect = this.transform.GetComponentInParent<ScrollRect>();
    }
    private void GetReward(PointerEventData eventData)
    {
        if(_chargeTime == null || 0 < _chargeTime.TotalSeconds)
        {
            return;
        }
        var loadingComplete = UI_LoadingPopup.Show();
        Managers.WebContents.UpdateRewardClaim(new ReqDtoUpdateRewardClaim()
        {
            UserAccountId = Managers.Game.UserInfo.UserAccountId,
            Gold = 200 // 임시 가격
        },
        (response) =>
        {
            loadingComplete.Value = true;
            Managers.Game.UserInfo.Gold = response.Gold;
            Managers.Game.UserInfo.LastRewardClaimTime = response.LastRewardClaimTime;
            Managers.Event.TriggerEvent(EEventType.UpdateGold);
        },
       (errorCode) =>
        {   
            loadingComplete.Value = true;
            UI_ErrorButtonPopup.ShowErrorButton(Managers.Error.GetError(Define.EErrorCode.ERR_NetworkSettlementErrorResend));
        });
    }
    public void CheckServerTime(DateTime newHeartBeat)
    {
        // 24시간이 지나야만 리워드 획득
        _nextRewardTime = Managers.Game.UserInfo.LastRewardClaimTime.AddHours(24);
        _chargeTime = _nextRewardTime - newHeartBeat;
        if (0 < _chargeTime.TotalSeconds)
        {
            GetText((int)Texts.RewardResetTimer_Text).text = $"{_chargeTime.Hours} : {_chargeTime.Minutes}";
        }
        else
        {
            GetText((int)Texts.RewardResetTimer_Text).text = "리워드 획득 가능";
        }
    }
    private void OnBeginDrag(PointerEventData eventData)
	{
        _parentScrollRect.OnBeginDrag(eventData); // 부모한테 이벤트 전달
    }
    private void OnDrag(PointerEventData eventData)
    {
        _parentScrollRect.OnDrag(eventData);
    }
    private void OnEndDrag(PointerEventData eventData)
    {
        _parentScrollRect.OnEndDrag(eventData);
    }
}
