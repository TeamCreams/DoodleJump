using System.Collections;
using DG.Tweening;
using UnityEngine;

public class UI_RevivalPrepPopup : UI_Popup
{
    private enum Texts
    {
        Countdown_Text,
    }
    float _fromSize;
    private Coroutine _countDownCoroutine;
    public override bool Init()
    {
        if (base.Init() == false)
        {
            return false;
        }

        BindTexts(typeof(Texts));
        _fromSize = GetText((int)Texts.Countdown_Text).fontSize;
        Time.timeScale = 0f;
        return true;
    }

    public void BeginCountdown()
    {
        if(_countDownCoroutine != null)
        {
            StopCoroutine(_countDownCoroutine);
        }
        _countDownCoroutine = StartCoroutine(CountDown());
    }
    
    IEnumerator CountDown()
    // 카운트다운 시작 코루틴
    {
        for (int number = 3; 0 < number; number--)
        {
            yield return AnimateNumber(number);
        }
        Managers.UI.ClosePopupUI(this);
        Time.timeScale = 1;
    }

    IEnumerator AnimateNumber(int number) 
    // 개별 숫자 애니메이션 실행 코루틴
    {
        GetText((int)Texts.Countdown_Text).text = $"{number}";
        
        float fromSize = _fromSize;
        float toSize = 1f;        
        GetText((int)Texts.Countdown_Text).fontSize = fromSize;
        
        float totalDuration = 1.0f;
        float currentDuration = totalDuration;
        while (0 <= currentDuration)
        {
            GetText((int)Texts.Countdown_Text).fontSize = Mathf.Lerp(fromSize, toSize, 1f - currentDuration / totalDuration);
            currentDuration -= UnityEngine.Time.unscaledDeltaTime;
            yield return null;
        }
        GetText((int)Texts.Countdown_Text).fontSize = toSize;
    }
}
