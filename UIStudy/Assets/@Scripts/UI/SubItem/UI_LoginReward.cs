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
    private SuberunkerSceneHomeScene _scene;
    private bool _isTimeCalculated = false;
    private bool _isAccept = false;
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

        _scene = Managers.Scene.CurrentScene as SuberunkerSceneHomeScene;

        // hierarchy 순서 변경
        this.transform.SetAsFirstSibling();
        return true;
    }
    private void OnDestroy()
    {
        Managers.SignalR.OnChangedHeartBeat -= CheckServerTime; // 구독 해제
    }
    private void OnDisable()
    {
        _isTimeCalculated = false;
    }
    public void SetInfo()
    {
        GetText((int)Texts.RewardResetTimer_Text).text = "시간 계산 중";
        _parentScrollRect = this.transform.GetComponentInParent<ScrollRect>();
    }
    private void GetReward(PointerEventData eventData)
    {
        if (!_isTimeCalculated || 0 < _chargeTime.TotalSeconds || _isAccept)
        {
            return;
        }

        GetText((int)Texts.RewardResetTimer_Text).text = "리워드 획득 완료";
        _isAccept = true;

        int gold = _scene.GetRandomReward();
        UI_RewardAcquiredPopup popup = Managers.UI.ShowPopupUI<UI_RewardAcquiredPopup>();
        popup.SetInfo(gold);
    }
    public void CheckServerTime(DateTime newHeartBeat)
    {
        // 24시간이 지나야만 리워드 획득
        _nextRewardTime = Managers.Game.UserInfo.LastRewardClaimTime.AddHours(24);
        _chargeTime = _nextRewardTime - newHeartBeat;

        _isTimeCalculated = true;

        if (0 < _chargeTime.TotalSeconds)
        {
            GetText((int)Texts.RewardResetTimer_Text).text = $"{_chargeTime.Hours} h {_chargeTime.Minutes} m";
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
