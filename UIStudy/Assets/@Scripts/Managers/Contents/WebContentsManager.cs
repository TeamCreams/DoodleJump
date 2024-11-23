using GameApi.Dtos;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using WebApi.Models.Dto;
using static Define;
public class WebRoute
{
    private readonly static string BaseUrl = $"https://dev-single-api.snapism.net:8082/";
    public readonly static Func<ReqDtoGetUserAccount, string> GetUserAccount = (dto) => $"{BaseUrl}User/GetUserAccount?UserName={dto.UserName}";
    public readonly static Func<ReqDtoGetValidateUserAccountId, string> GetValidateUserAccountId = (dto) => $"{BaseUrl}User/GetValidateUserAccountId?UserName={dto.UserName}";
    public readonly static string InsertUserAccount = $"{BaseUrl}User/InsertUser";
    public readonly static string InsertUserAccountScore = $"{BaseUrl}User/InsertUserAccountScore";
    public readonly static string InsertUserAccountNickname = $"{BaseUrl}User/InsertUserAccountNickname";
    //public readonly static Func<ReqInsertUserAccountScore, string> InsertUserAccountScore = (dto) => $"{BaseUrl}User/InsertUserAccountScore?UserName={dto.UserName}&Score{dto.Score}";
    public readonly static Func<ReqDtoGetOrAddUserAccount, string> GetOrAddUserAccount = (dto) => $"{BaseUrl}User/GetOrAddUserAccount";
    public readonly static Func<ReqDtoGetUserAccountList, string> GetUserAccountList = (dto) => $"{BaseUrl}User/GetUserAccountList";

    public readonly static string InsertUserMission= $"{BaseUrl}User/InsertUserMissions";
    public readonly static string UpdateUserMission = $"{BaseUrl}User/UpdateUserMission";
    public readonly static string CompleteUserMission = $"{BaseUrl}User/CompleteUserMission";
    public readonly static Func<ReqDtoGetUserMissionList, string> GetUserMissionList = (dto) => $"{BaseUrl}User/GetUserMissionList";
}

public class WebContentsManager
{
    public void ReqGetUserAccount(ReqDtoGetUserAccount requestDto, Action<ResDtoGetUserAccount> onSuccess = null, Action<EStatusCode> onFailed = null)
    {
        Managers.Web.SendGetRequest(WebRoute.GetUserAccount(requestDto), (response) =>
        {
            CommonResult<ResDtoGetUserAccount> rv = JsonConvert.DeserializeObject<CommonResult<ResDtoGetUserAccount>>(response);
            
            if(rv == null || false == rv.IsSuccess)
            {
                onFailed.Invoke(EStatusCode.ServerException);
            }
            else
            {
                if(rv.StatusCode != EStatusCode.OK)
                {
                    onFailed.Invoke(rv.StatusCode);
                }
                else
                {
                    onSuccess.Invoke(rv.Data);
                }
            }
        });
    }


    public void ReqGetValidateUserAccountId(ReqDtoGetValidateUserAccountId requestDto, Action<ResDtoGetValidateUserAccountId> onSuccess = null, Action<EStatusCode> onFailed = null)
    {
        Managers.Web.SendGetRequest(WebRoute.GetValidateUserAccountId(requestDto), (response) =>
        {
            CommonResult<ResDtoGetValidateUserAccountId> rv = JsonConvert.DeserializeObject<CommonResult<ResDtoGetValidateUserAccountId>>(response);

            if (rv == null || false == rv.IsSuccess)
            {
                onFailed.Invoke(EStatusCode.ServerException);
            }
            else
            {
                if (rv.StatusCode != EStatusCode.OK)
                {
                    onFailed.Invoke(rv.StatusCode);
                }
                else
                {
                    onSuccess.Invoke(rv.Data);
                }
            }
        });
    }
    public void ReqInsertUserAccount(ReqDtoInsertUserAccount requestDto, Action<ResDtoInsertUserAccount> onSuccess = null, Action<EStatusCode> onFailed = null)
    {
        string body = JsonConvert.SerializeObject(requestDto, Formatting.Indented);

        Managers.Web.SendPostRequest(WebRoute.InsertUserAccount, body , (response) =>
        {
            CommonResult<ResDtoInsertUserAccount> rv = JsonConvert.DeserializeObject<CommonResult<ResDtoInsertUserAccount>>(response);

            if (rv == null || false == rv.IsSuccess)
            {
                onFailed.Invoke(EStatusCode.ServerException);
            }
            else
            {
                if (rv.StatusCode != EStatusCode.OK)
                {
                    onFailed.Invoke(rv.StatusCode);
                }
                else
                {
                    onSuccess.Invoke(rv.Data);
                }
            }
        });
    }
    public void ReqInsertUserAccountScore(ReqDtoInsertUserAccountScore requestDto, Action<ResDtoInsertUserAccountScore> onSuccess = null, Action<EStatusCode> onFailed = null)
    {
        string body = JsonConvert.SerializeObject(requestDto, Formatting.Indented);

        Managers.Web.SendPostRequest(WebRoute.InsertUserAccountScore, body , (response) =>
        {
            CommonResult<ResDtoInsertUserAccountScore> rv = JsonConvert.DeserializeObject<CommonResult<ResDtoInsertUserAccountScore>>(response); 

            if (rv == null || false == rv.IsSuccess)
            {
                onFailed.Invoke(EStatusCode.ServerException);
            }
            else
            {
                if (rv.StatusCode != EStatusCode.OK)
                {
                    onFailed.Invoke(rv.StatusCode);
                }
                else
                {
                    onSuccess.Invoke(rv.Data);
                }
            }
        });
    }
    public void ReqInsertUserAccountNickname(ReqDtoInsertUserAccountNickname requestDto, Action<ResDtoInsertUserAccountNickname> onSuccess = null, Action<EStatusCode> onFailed = null)
    {
        string body = JsonConvert.SerializeObject(requestDto, Formatting.Indented);

        Managers.Web.SendPostRequest(WebRoute.InsertUserAccountNickname, body , (response) =>
        {
            CommonResult<ResDtoInsertUserAccountNickname> rv = JsonConvert.DeserializeObject<CommonResult<ResDtoInsertUserAccountNickname>>(response);

            if (rv == null || false == rv.IsSuccess)
            {
                onFailed.Invoke(EStatusCode.ServerException);
            }
            else
            {
                if (rv.StatusCode != EStatusCode.OK)
                {
                    onFailed.Invoke(rv.StatusCode);
                }
                else
                {
                    onSuccess.Invoke(rv.Data);
                }
            }
        });
    }
    public void ReqGetOrAddUserAccount(ReqDtoGetOrAddUserAccount requestDto, Action<ResDtoGetOrAddUserAccount> onSuccess = null, Action<EStatusCode> onFailed = null)
    {
        Managers.Web.SendGetRequest(WebRoute.GetOrAddUserAccount(requestDto), (response) =>
        {
            CommonResult<ResDtoGetOrAddUserAccount> rv = JsonConvert.DeserializeObject<CommonResult<ResDtoGetOrAddUserAccount>>(response);
            
            if(rv == null || false == rv.IsSuccess)
            {
                onFailed.Invoke(EStatusCode.ServerException);
            }
            else
            {
                if(rv.StatusCode != EStatusCode.OK)
                {
                    onFailed.Invoke(rv.StatusCode);
                }
                else
                {
                    Debug.Log($"{rv.Data.UserName ?? "null"}, {rv.Data.Nickname ?? "null"}");
                    Managers.Game.UserInfo.UserId = rv.Data.UserName;
                    Managers.Game.UserInfo.UserNickname = rv.Data.Nickname;
                    onSuccess.Invoke(rv.Data);
                }
            }
        });
    }
    public void ReqGetUserAccountList(ReqDtoGetUserAccountList requestDto, Action<ResDtoGetUserAccountList> onSuccess = null, Action<EStatusCode> onFailed = null)
    {
        Managers.Web.SendGetRequest(WebRoute.GetUserAccountList(requestDto), (response) =>
        {
            CommonResult<ResDtoGetUserAccountList> rv = JsonConvert.DeserializeObject<CommonResult<ResDtoGetUserAccountList>>(response);
            
            if(rv == null || false == rv.IsSuccess)
            {
                Debug.Log("ReqGetUserAccountList fail");
                onFailed.Invoke(EStatusCode.ServerException);
            }
            else
            {
                if(rv.StatusCode != EStatusCode.OK)
                {                    
                    Debug.Log("ReqGetUserAccountList fail");
                    onFailed.Invoke(rv.StatusCode);
                }
                else
                {
                    Debug.Log("ReqGetUserAccountList success");
                    onSuccess.Invoke(rv.Data);
                }
            }
        });
    }



    public void ReqInsertUserMission(ReqDtoInsertUserMission requestDto, Action<ResDtoInsertUserMission> onSuccess = null, Action<EStatusCode> onFailed = null)
    {
        string body = JsonConvert.SerializeObject(requestDto, Formatting.Indented);

        Managers.Web.SendPostRequest(WebRoute.InsertUserMission, body , (response) =>
        {
            CommonResult<ResDtoInsertUserMission> rv = JsonConvert.DeserializeObject<CommonResult<ResDtoInsertUserMission>>(response);

            if (rv == null || false == rv.IsSuccess)
            {
                Debug.Log("ReqInsertUserMission false == rv.IsSuccess "+ rv.Message);
                onFailed.Invoke(EStatusCode.ServerException);
            }
            else
            {
                if (rv.StatusCode != EStatusCode.OK)
                {
                    Debug.Log(rv.Message);
                    onFailed.Invoke(rv.StatusCode);
                }
                else
                {
                    onSuccess.Invoke(rv.Data);
                }
            }
        });
    }

    public void CompleteUserMission(ReqDtoCompleteUserMission requestDto, Action<ResDtoCompleteUserMission> onSuccess = null, Action<EStatusCode> onFailed = null)
    {
        string body = JsonConvert.SerializeObject(requestDto, Formatting.Indented);

        Managers.Web.SendPostRequest(WebRoute.InsertUserMission, body , (response) =>
        {
            CommonResult<ResDtoCompleteUserMission> rv = JsonConvert.DeserializeObject<CommonResult<ResDtoCompleteUserMission>>(response);

            if (rv == null || false == rv.IsSuccess)
            {
                onFailed.Invoke(EStatusCode.ServerException);
            }
            else
            {
                if (rv.StatusCode != EStatusCode.OK)
                {
                    onFailed.Invoke(rv.StatusCode);
                }
                else
                {
                    onSuccess.Invoke(rv.Data);
                }
            }
        });
    }

    public void UpdateUserMission(ReqDtoUpdateUserMission requestDto, Action<ResDtoUpdateUserMission> onSuccess = null, Action<EStatusCode> onFailed = null)
    {
        string body = JsonConvert.SerializeObject(requestDto, Formatting.Indented);

        Managers.Web.SendPostRequest(WebRoute.InsertUserMission, body , (response) =>
        {
            CommonResult<ResDtoUpdateUserMission> rv = JsonConvert.DeserializeObject<CommonResult<ResDtoUpdateUserMission>>(response);

            if (rv == null || false == rv.IsSuccess)
            {
                onFailed.Invoke(EStatusCode.ServerException);
            }
            else
            {
                if (rv.StatusCode != EStatusCode.OK)
                {
                    onFailed.Invoke(rv.StatusCode);
                }
                else
                {
                    onSuccess.Invoke(rv.Data);
                }
            }
        });
    }

    public void ReqDtoGetUserMissionList(ReqDtoGetUserMissionList requestDto, Action<ResDtoGetUserMissionList> onSuccess = null, Action<EStatusCode> onFailed = null)
    {
        Managers.Web.SendGetRequest(WebRoute.GetUserMissionList(requestDto), (response) =>
        {
            CommonResult<ResDtoGetUserMissionList> rv = JsonConvert.DeserializeObject<CommonResult<ResDtoGetUserMissionList>>(response);
            
            if(rv == null || false == rv.IsSuccess)
            {
                onFailed.Invoke(EStatusCode.ServerException);
            }
            else
            {
                if(rv.StatusCode != EStatusCode.OK)
                {                    
                    onFailed.Invoke(rv.StatusCode);
                }
                else
                {
                    onSuccess.Invoke(rv.Data);
                }
            }
        });
    }
}