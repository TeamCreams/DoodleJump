using System.Collections;
using UniRx;
using UnityEngine;
using static Define;

public class UI_LoadingPopup : UI_Popup
{
    public override bool Init()
    {
        if (base.Init() == false)
        {
            return false;
        }

        return true;
    }

    public static ReactiveProperty<bool> Show()
    {
        UI_LoadingPopup indicator = Managers.UI.ShowPopupUI<UI_LoadingPopup>();

        ReactiveProperty<bool> condition = new ReactiveProperty<bool>(false);
        condition.DistinctUntilChanged(c => c == true)
            .Where(c => c)
            .Subscribe(_ =>
            {
                Managers.UI.ClosePopupUI(indicator);
            });

        return condition;
    }
}
