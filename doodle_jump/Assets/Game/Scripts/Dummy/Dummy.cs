using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dummy : MonoBehaviour
{
    float time = 3.0f;
    float moveTo = 10;

    // Update is called once per frame
    void OnEnable()
    {
        var tween = this.transform.DOLocalMoveX(moveTo, time).SetEase(Ease.Linear);
        tween.onComplete += ResetUnlock;
    }

    private void ResetUnlock()
    {
        moveTo = -moveTo;
        var tween = this.transform.DOLocalMoveX(moveTo, time).SetEase(Ease.Linear);
        tween.onComplete += ResetUnlock;
    }
}
