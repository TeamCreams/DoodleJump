using System;
using System.Collections;
using GameApi.Dtos;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;
using static Define;

public class UI_HomeTopBar : UI_Base
{
    private enum Texts
    {
        TotalGold_Text,
        Energy_Text,
        EnergyTimer_Text,
    }
    private enum Buttons
    {
        Setting_Button,

        EnergyAdd_Button,
        GoldAdd_Button
    }
    private const int ENERGY_RECHARGE_SECONDS = 300; // 5분
    System.IDisposable _rechargeTimer;
    private int _displayTime = 0;
    private int _calculateTime = 0;
    private DateTime _serverTime = new DateTime();
    private DateTime _startTime = new DateTime();
    private bool _isRunningTimer = false;
    private bool _isSettingComplete = false;
    private Coroutine _tickCo;
    public override bool Init()
    {
        if (base.Init() == false)
        {
            return false;
        }

        BindTexts(typeof(Texts));
        BindButtons(typeof(Buttons));
        
        // Bind Event
        GetButton((int)Buttons.Setting_Button).gameObject.BindEvent(OnClick_SettingButton, EUIEvent.Click);
        GetButton((int)Buttons.EnergyAdd_Button).gameObject.BindEvent(OnClick_EnergyAddButton, EUIEvent.Click);
        GetButton((int)Buttons.GoldAdd_Button).gameObject.BindEvent(OnClick_GoldAddButton, EUIEvent.Click);

        Managers.Event.AddEvent(EEventType.UIRefresh, OnEvent_Refresh);

        _startTime = Managers.Game.UserInfo.LatelyEnergy;
        OnEvent_Refresh(null, null);

        Managers.SignalR.OnChangedHeartBeat -= CheckServerTime; // 구독 해제
        Managers.SignalR.OnChangedHeartBeat += CheckServerTime; // 이벤트 구독
        return true;
    }

    private void OnDestroy()
    {
        Managers.Event.RemoveEvent(EEventType.UIRefresh, OnEvent_Refresh);
        Managers.SignalR.OnChangedHeartBeat -= CheckServerTime; // 구독 해제
    }
    private void OnClick_EnergyAddButton(PointerEventData eventData)
    {
        // 판넬 추가
        var ui = Managers.UI.ShowPopupUI<UI_EnergyShopPanel>();
        ui.SetInfo();
    }
    private void OnClick_GoldAddButton(PointerEventData eventData)
    {
        Managers.UI.ShowPopupUI<UI_ShopPanel>();
        Managers.Event.TriggerEvent(EEventType.EnterShop);
    }
    private void OnClick_SettingButton(PointerEventData eventData)
    {
        UI_SettingPopup settingPopup = Managers.UI.ShowPopupUI<UI_SettingPopup>();
        settingPopup.ActiveInfo();
    }
    private void OnEvent_Refresh(Component sender, object param)
    {
        Debug.Log("OnEvent_Refresh");
        GetText((int)Texts.Energy_Text).text = $"{Managers.Game.UserInfo.Energy} / 10";
        GetText((int)Texts.TotalGold_Text).text = Managers.Game.UserInfo.Gold.ToString();
        if (10 <= Managers.Game.UserInfo.Energy)
        {
            GetText((int)Texts.EnergyTimer_Text).text = "05:00";
            StopEnergyTimer();        
        }
    }
    private void StopEnergyTimer()
    {
        _isRunningTimer = false;
        _isSettingComplete = false;
        
        if(_tickCo != null)
        {
            StopCoroutine(_tickCo);
            _tickCo = null;
            _rechargeTimer?.Dispose();
        }
    }
    private void EnergyTimer()
    {
        // 5초마다 서버에서 시간을 받아옴.
        // 300초가 되면 updateEnergy 요청

        // 게임어플을 아예 꺼버렸을 때 laytelyTime을 저장할 방법은?
        if (10 <= Managers.Game.UserInfo.Energy)
        {
            StopEnergyTimer();
            return;
        }

        if (_isRunningTimer)
        {
            return;
        }
        _isRunningTimer = true;
        _calculateTime = (int)(_serverTime - _startTime).TotalSeconds;
        _isSettingComplete = true;
        EnergyRecharge();
    }
    public void CheckServerTime(DateTime newHeartBeat)
    {
        _serverTime = newHeartBeat; // 5초마다 웹소켓에서 전해준 값으로 서버시간 업데이트. 
        EnergyTimer(); //.ConfigureAwait(false); // 비동기로 실행. 끝나고 메인스레드로 돌아가지 않음.
    }
    private void EnergyRecharge()
    {
        // 코루틴은 메인스레드에서 진행 됨.
        // 서버시간을 받으면서 백그라운드 스레드로 진행되기 때문에 메인스레드로 전환이 필요.
        if(_tickCo == null)
        {
            _tickCo = StartCoroutine(EnergyRechargeCoroutine());
        }
    }
    
    private IEnumerator EnergyRechargeCoroutine()
    {
        yield return new WaitWhile(() => _isSettingComplete == false);
        _rechargeTimer?.Dispose();
        _rechargeTimer = Observable.Interval(new TimeSpan(0, 0, 1))
            .Subscribe(_ =>
            {
                _calculateTime++;

                //에너지 충전 완료 체크
                if(ENERGY_RECHARGE_SECONDS <= _calculateTime)
                {
                    _startTime = _serverTime;
                    _calculateTime = 0;
                    Managers.Event.TriggerEvent(EEventType.UpdateEnergy, this);
                }

                //남은 시간 계산
                int remainingSeconds = ENERGY_RECHARGE_SECONDS - (_calculateTime % ENERGY_RECHARGE_SECONDS);
                remainingSeconds = Mathf.Max(0, remainingSeconds); // 0 미만 방지

                int minutes = Mathf.Clamp(remainingSeconds / 60, 0, 5);
                int seconds = Mathf.Clamp(remainingSeconds % 60, 0, 59);

                GetText((int)Texts.EnergyTimer_Text).text = $"{minutes:D1}:{seconds:D2}";
            }).AddTo(this.gameObject);
    }
}
