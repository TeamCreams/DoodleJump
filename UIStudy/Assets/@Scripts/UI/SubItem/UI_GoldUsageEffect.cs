using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using static Define;

public class UI_GoldUsageEffect : UI_Base
{
    private enum Texts
    {
        TotalGold_Text,
    }
    public override bool Init()
    {
        if (base.Init() == false)
        {
            return false;
        }

        BindTexts(typeof(Texts));
        Managers.Event.AddEvent(EEventType.UpdateGold, OnEvent_ChangedGold);

        return true;
    }

	private void OnDestroy()
    {
        Managers.Event.RemoveEvent(EEventType.UpdateGold, OnEvent_ChangedGold);
    }
    void OnEvent_ChangedGold(Component sender, object param)
    {
        Managers.Sound.Play(ESound.Effect, "CoinSound");
        StartCoroutine(UpdateGold());
    }
    IEnumerator UpdateGold()
    {
        int toGold = Managers.Game.RemainingChange;
        int fromGold = Managers.Game.UserInfo.Gold;
        float maxDuration = 0.15f;
        float duration = maxDuration;

        while (0 < duration)
        {
            float nextValue = Mathf.Lerp(fromGold, toGold, 1 - duration / maxDuration);
            int tempNextValue = (int)nextValue;
            GetText((int)Texts.TotalGold_Text).text = tempNextValue.ToString();

            duration -= UnityEngine.Time.deltaTime;
            yield return null;
        }
        GetText((int)Texts.TotalGold_Text).text = Managers.Game.RemainingChange.ToString();
        Managers.Game.UserInfo.Gold = Managers.Game.RemainingChange;
    }

}
