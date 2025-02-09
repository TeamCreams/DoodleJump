﻿using GameApi.Dtos;
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
    public readonly static Func<ReqDtoGetOrAddUserAccount, string> GetOrAddUserAccount = (dto) => $"{BaseUrl}User/GetOrAddUserAccount?UserName={dto.UserName}";
    public readonly static Func<ReqDtoGetUserAccountList, string> GetUserAccountList = (dto) => $"{BaseUrl}User/GetUserAccountList"; //얘는 param값이 없음

    //public readonly static string InsertUserMissionList= $"{BaseUrl}User/InsertUserMissionList";
    public readonly static Func<ReqDtoInsertUserMissionList, string> InsertUserMissionList= (dto) => $"{BaseUrl}User/InsertUserMissionList?UserAccountId={dto.UserAccountId}&List={dto.List}";

    public readonly static Func<ReqDtoUpdateUserMissionList, string> UpdateUserMissionList = (dto) => $"{BaseUrl}User/UpdateUserMissionList?UserAccountId={dto.UserAccountId}&List={dto.List}";
    public readonly static string CompleteUserMission = $"{BaseUrl}User/CompleteUserMission";
    public readonly static Func<ReqDtoGetUserMissionList, string> GetUserMissionList = (dto) => $"{BaseUrl}User/GetUserMissionList?UserAccountId={dto.UserAccountId}";
    public readonly static string InsertMissionCompensation = $"{BaseUrl}User/InsertMissionCompensation";
    public readonly static string UpdateUserStyle = $"{BaseUrl}User/UpdateUserStyle";
    public readonly static string UpdateUserGold = $"{BaseUrl}User/UpdateUserGold";

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



    public void ReqInsertUserMission(ReqDtoInsertUserMissionList requestDto, Action<ResDtoInsertUserMissionList> onSuccess = null, Action<EStatusCode> onFailed = null)
    {
        Managers.Web.SendGetRequest(WebRoute.InsertUserMissionList(requestDto), (response) =>
        {
            CommonResult<ResDtoInsertUserMissionList> rv = JsonConvert.DeserializeObject<CommonResult<ResDtoInsertUserMissionList>>(response);

            if (rv == null)
            {
                onFailed.Invoke(EStatusCode.UnknownError);
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
    public void CompleteUserMission(ReqDtoCompleteUserMission requestDto, Action<ResDtoCompleteUserMission> onSuccess = null, Action<EStatusCode> onFailed = null)
    {
        string body = JsonConvert.SerializeObject(requestDto, Formatting.Indented);

        Managers.Web.SendPostRequest(WebRoute.CompleteUserMission, body , (response) =>
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
    public void UpdateUserMissionList(ReqDtoUpdateUserMissionList requestDto, Action<ResDtoUpdateUserMissionList> onSuccess = null, Action<EStatusCode> onFailed = null)
    {
        Managers.Web.SendGetRequest(WebRoute.UpdateUserMissionList(requestDto) , (response) =>
        {
            CommonResult<ResDtoUpdateUserMissionList> rv = JsonConvert.DeserializeObject<CommonResult<ResDtoUpdateUserMissionList>>(response);

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
        string body = JsonConvert.SerializeObject(requestDto, Formatting.Indented);
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
                    // 여기로 들어와 짐
                    onFailed.Invoke(rv.StatusCode);
                }
                else
                {
                    onSuccess.Invoke(rv.Data);
                }
            }
        });
    }

    public void ReqDtoUpdateUserStyle(ReqDtoUpdateUserStyle requestDto, Action<ResDtoUpdateUserStyle> onSuccess = null, Action<EStatusCode> onFailed = null)
    {
        string body = JsonConvert.SerializeObject(requestDto, Formatting.Indented);

        Managers.Web.SendPostRequest(WebRoute.UpdateUserStyle, body , (response) =>
        {
            CommonResult<ResDtoUpdateUserStyle> rv = JsonConvert.DeserializeObject<CommonResult<ResDtoUpdateUserStyle>>(response);

            if (rv == null)
            {
                onFailed.Invoke(EStatusCode.UnknownError);
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
                    Managers.Scene.LoadScene(EScene.SuberunkerSceneHomeScene);
                }
            }
        });
    }

    public void ReqDtoUpdateUserGold(ReqDtoUpdateUserGold requestDto, Action<ResDtoUpdateUserGold> onSuccess = null, Action<EStatusCode> onFailed = null)
    {
        string body = JsonConvert.SerializeObject(requestDto, Formatting.Indented);

        Managers.Web.SendPostRequest(WebRoute.UpdateUserGold, body , (response) =>
        {
            CommonResult<ResDtoUpdateUserGold> rv = JsonConvert.DeserializeObject<CommonResult<ResDtoUpdateUserGold>>(response);

            if (rv == null)
            {
                onFailed.Invoke(EStatusCode.UnknownError);
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
//     public void ReqDtoInsertMissionCompensation(ReqDtoInsertMissionCompensation requestDto, Action<ResDtoInsertMissionCompensation> onSuccess = null, Action<EStatusCode> onFailed = null)
//     {
//         string body = JsonConvert.SerializeObject(requestDto, Formatting.Indented);

//         Managers.Web.SendPostRequest(WebRoute.InsertMissionCompensation, body , (response) =>
//         {
//             CommonResult<ResDtoInsertMissionCompensation> rv = JsonConvert.DeserializeObject<CommonResult<ResDtoInsertMissionCompensation>>(response);

//             if (rv == null || false == rv.IsSuccess)
//             {
//                 onFailed.Invoke(EStatusCode.ServerException);
//             }
//             else
//             {
//                 if (rv.StatusCode != EStatusCode.OK)
//                 {
//                     onFailed.Invoke(rv.StatusCode);
//                 }
//                 else
//                 {                    
//                     onSuccess.Invoke(rv.Data);
//                 }
//             }
//         });
//     }
}