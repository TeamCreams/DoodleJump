using System;
using System.Collections;
using System.Net.Http;
using System.Security.Policy;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;
using static Define;
using WebApi.Models.Dto;
using GameApi.Dtos;

public class ScoreManagerSlave : InitBase
{
    private CommonResult<ResDtoGetUserAccount> _rv = null;

    public override bool Init()
    {
        if (false == base.Init())
        {
            return false;
        }
        Managers.Event.AddEvent(EEventType.SignIn, OnEvent_GetRv);

        DontDestroyOnLoad(this.gameObject);
        Debug.Log("ScoreManagerSlave");
        return true;
    }

    public int GetScore(string userName, EScoreType scoreType)//, Action<int> callback)
    {
        switch(scoreType)
        {
            case EScoreType.LatelyScore:
                return _rv.Data.LatelyScore;//callback(_rv.Data.LatelyScore);
            case EScoreType.RecordScore:
                return _rv.Data.HighScore;//callback(_rv.Data.HighScore);
        }
        return 0;
        //StartCoroutine(ReturnRv(requestDto, scoreType, callback));
    }

    public void SetScore(string userName, int score)
    {
        _rv.Data.LatelyScore = score;
        //StartCoroutine(ReturnRv(requestDto, EScoreType.LatelyScore, null, score));
    }

    private IEnumerator ReturnRv(ReqDtoGetUserAccount requestDto, EScoreType scoreType, Action<int> callback, int score = -1)
    {
        if(_rv == null)
        {
            Managers.Web.SendGetRequest(WebRoute.GetUserAccount(requestDto), (response) =>        
            {
                Debug.Log("Response: " + response);
                _rv = JsonConvert.DeserializeObject<CommonResult<ResDtoGetUserAccount>>(response);
            });
        }
        yield return new WaitUntil(() => _rv != null);

        if(score != -1)
        {
            if (_rv.IsSuccess)
            {
                Debug.Log("Set Score");

                switch(scoreType)
                {
                    case EScoreType.LatelyScore:
                            _rv.Data.LatelyScore = score;
                    break;
                }
            }
            else
            {
                Debug.Log("fail");   
            }
        }
        else
        {
            if (_rv.IsSuccess)
            {
                Debug.Log("Get Score");

                switch(scoreType)
                {
                    case EScoreType.LatelyScore:
                            callback(_rv.Data.LatelyScore);
                    break;
                    case EScoreType.RecordScore:
                            callback(_rv.Data.HighScore);
                    break;
                }
            }
            else
            {
                Debug.Log("fail");
                callback(0);        
            }
        }
    }

    void OnEvent_GetRv(Component sender, object param)
    {
        ReqDtoGetUserAccount requestDto = new ReqDtoGetUserAccount();
        requestDto.UserName = "Orange";
        Debug.Log("Orange" + " hihihihihihihihihi");
        Managers.Web.SendGetRequest(WebRoute.GetUserAccount(requestDto), (response) =>        
        {
            Debug.Log("Response: " + response);
            _rv = JsonConvert.DeserializeObject<CommonResult<ResDtoGetUserAccount>>(response);
        });
    }

}