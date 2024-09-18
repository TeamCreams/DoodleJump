using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using static Define;

public class UI_PlayerDesign : UI_Base
{

    enum Images
    {
        Hair,
        Eyes,
        Eyebrows,
    }

    protected override void Init()
    {
        base.Init();
        BindImages(typeof(Images));
        Managers.Event.AddEvent(EEventType.SetStyle_Player, OnEvent_SetStyle);
    }
    private void OnDestroy()
    {
        Managers.Event.RemoveEvent(EEventType.SetStyle_Player, OnEvent_SetStyle);
    }
    public void OnEvent_SetStyle(Component sender, object param)
    {
        GetImage((int)Images.Hair).sprite = Managers.Resource.Load<Sprite>($"{Managers.Game.ChracterStyleInfo.Hair}.sprite");
        GetImage((int)Images.Eyebrows).sprite = Managers.Resource.Load<Sprite>($"{Managers.Game.ChracterStyleInfo.Eyebrows}.sprite");
        GetImage((int)Images.Eyes).sprite = Managers.Resource.Load<Sprite>($"{Managers.Game.ChracterStyleInfo.Eyes}.sprite");
    }

}
