﻿using System.Collections;
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
        //Managers.Event.AddEvent(EEventType.StartLoading, OnEvent_StartLoading);
        //Managers.Event.AddEvent(EEventType.StopLoading, OnEvent_StopLoading);

        return true;
    }

    private void OnDestroy()
    {
        //Managers.Event.RemoveEvent(EEventType.StartLoading, OnEvent_StartLoading);
        //Managers.Event.RemoveEvent(EEventType.StopLoading, OnEvent_StopLoading);
    }
    private void OnEvent_StartLoading(Component sender, object param)
    {
        StartCoroutine(Loading_Co());
    }
    private void OnEvent_StopLoading(Component sender, object param)
    {
        StopCoroutine(Loading_Co());
        Managers.Resource.Destroy(this.gameObject);
    }
    private IEnumerator Loading_Co()
    {
        yield return new WaitForSeconds(100);     
        Managers.Resource.Destroy(this.gameObject);
    }
}
