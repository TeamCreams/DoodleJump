using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using GameApi.Dtos;
using static Define;

public class UI_RewardAcquiredPopup : UI_Popup
{
    private enum Texts
    {
        Gold_Text,
    }
    private enum Images
    {
        ClickWindow,
    }
    private enum GameObjects
    {
        Group_Object
    }

    Sequence _showSeq;
    private int _reward;
    private bool _isSuccess = false;
    public override bool Init()
    {
        if (base.Init() == false)
        {
            return false;
        }
        BindTexts(typeof(Texts));
        BindImages(typeof(Images));
        BindObjects(typeof(GameObjects));

        GetImage((int)Images.ClickWindow).gameObject.BindEvent(OnClick_Close, EUIEvent.Click);

        Vector3 originalScale = this.transform.localScale;
        float scaleSpeed = 0.1f;
        _showSeq = DOTween.Sequence()
            .Append(GetObject((int)GameObjects.Group_Object).transform.DOScale(new Vector3(originalScale.x + 0.5f, originalScale.y + 0.5f, originalScale.z + 0.5f), scaleSpeed).SetEase(Ease.Linear))
            .Append(GetObject((int)GameObjects.Group_Object).transform.DOScale(originalScale, scaleSpeed).SetEase(Ease.Linear)); //여기가 실행 안됨.

        return true;
    }
    public void SetInfo(int gold)
    {
        GetText((int)Texts.Gold_Text).text = gold.ToString();
        _reward = gold;
    }

    private void OnClick_Close(PointerEventData eventData)
    {
        GetReward();
        StartCoroutine(Close());
    }

    private IEnumerator Close()
    {
        yield return new WaitWhile(() => _isSuccess == false);
        Managers.UI.ClosePopupUI(this);
    }

    private void GetReward()
    {
        var loadingComplete = UI_LoadingPopup.Show();

        Managers.WebContents.UpdateRewardClaim(new ReqDtoUpdateRewardClaim()
        {
            UserAccountId = Managers.Game.UserInfo.UserAccountId,
            Gold = _reward
        },
        (response) =>
        {
            loadingComplete.Value = true;
            Managers.Game.GoldTochange = response.Gold;
            Managers.Game.UserInfo.LastRewardClaimTime = response.LastRewardClaimTime;
            _isSuccess = true;
            Managers.Event.TriggerEvent(EEventType.AddGold);
        },
       (errorCode) =>
        {   
            loadingComplete.Value = true;
            _isSuccess = true;
            UI_ErrorButtonPopup.ShowErrorButton(Managers.Error.GetError(Define.EErrorCode.ERR_NetworkSettlementErrorResend));
        });
    }

}
