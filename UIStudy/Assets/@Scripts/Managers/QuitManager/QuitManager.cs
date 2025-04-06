using System;
using System.Collections;
using System.Net.Http;
using System.Text;
using UnityEngine;

public class QuitManager : InitBase
{
    bool _isPaused = false; 
    public override bool Init()
    {
        if (false == base.Init())
        {
            return false;
        }

        DontDestroyOnLoad(this.gameObject);

        return true;
    }

    void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            _isPaused = true;
            Managers.Event.TriggerEvent(Define.EEventType.UpdateEnergy, this);

            /* 앱이 비활성화 되었을 때 처리 */    
        }

        else
        {
            if (_isPaused)
            {
                _isPaused = false;
                Managers.Event.TriggerEvent(Define.EEventType.UpdateEnergy, this);
                /* 앱이 활성화 되었을 때 처리 */    
            }
        }
    }

    void OnApplicationQuit()
    {

    /* 앱이 종료 될 때 처리 */   
        Managers.Event.TriggerEvent(Define.EEventType.UpdateEnergy, this);

    }
}
    