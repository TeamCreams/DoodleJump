using GameApi.Dtos;
using GameApiDto.Dtos;
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
    private readonly static string BaseUrl = $"https://dd37927.store/";
    public readonly static Func<ReqDtoGetUserAccount, string> GetUserAccount = (dto) => $"{BaseUrl}User/GetUserAccount?UserName={dto.UserName}&Password={dto.Password}";
                                                                            //https://dd37927.store/User/GetUserAccount?UserName=test3&Password=12345678
    public readonly static Func<ReqDtoGetUserAccountByGoogle, string> GetUserAccountByGoogle = (dto) => $"{BaseUrl}User/GetUserAccountByGoogle?GoogleAccount={dto.GoogleAccount}";

    public readonly static string CheckUserAccountUserNameExists = $"{BaseUrl}User/CheckUserAccountUserNameExists";
    public readonly static string CheckUserAccountNicknameExists = $"{BaseUrl}User/CheckUserAccountNicknameExists";
    public readonly static string CheckGoogleAccountExists = $"{BaseUrl}User/CheckGoogleAccountExists";
    
    public readonly static Func<ReqDtoGetUserAccountPassword, string> GetUserAccountPassword = (dto) => $"{BaseUrl}User/GetUserAccountPassword?UserName={dto.UserName}";

    public readonly static string InsertUserAccount = $"{BaseUrl}User/InsertUser";
    public readonly static string InsertUserAccountScore = $"{BaseUrl}User/InsertUserAccountScore";
    public readonly static string InsertUserAccountNickname = $"{BaseUrl}User/InsertUserAccountNickname";
    public readonly static string InsertGoogleAccount = $"{BaseUrl}User/InsertGoogleAccount";
    public readonly static string BindUserAccountToGoogle = $"{BaseUrl}User/BindUserAccountToGoogle";

    //public readonly static Func<ReqInsertUserAccountScore, string> InsertUserAccountScore = (dto) => $"{BaseUrl}User/InsertUserAccountScore?UserName={dto.UserName}&Score={dto.Score}";
    public readonly static Func<ReqDtoGetOrAddUserAccount, string> GetOrAddUserAccount = (dto) => $"{BaseUrl}User/GetOrAddUserAccount?UserName={dto.UserName}";
    public readonly static Func<ReqDtoGetUserAccountList, string> GetUserAccountList = (dto) => $"{BaseUrl}User/GetUserAccountList"; //얘는 param값이 없음

    public readonly static string InsertUserMissionList= $"{BaseUrl}User/InsertUserMissionList";
    //public readonly static Func<ReqDtoInsertUserMissionList, string> InsertUserMissionList= (dto) => $"{BaseUrl}User/InsertUserMissionList?UserAccountId={dto.UserAccountId}&List={dto.List}";
    public readonly static string UpdateUserMissionList = $"{BaseUrl}User/UpdateUserMissionList";
    //public readonly static Func<ReqDtoUpdateUserMissionList, string> UpdateUserMissionList = (dto) => $"{BaseUrl}User/UpdateUserMissionList?UserAccountId={dto.UserAccountId}&List={dto.List}";
    public readonly static string CompleteUserMission = $"{BaseUrl}User/CompleteUserMission";
    //public readonly static Func<ReqDtoCompleteUserMission, string> CompleteUserMission = (dto) => $"{BaseUrl}User/CompleteUserMission?UserAccountId={dto.UserAccountId}&MissionId={dto.MissionId}&Param1={dto.Param1}&Gold={dto.Gold}";
    public readonly static Func<ReqDtoGetUserMissionList, string> GetUserMissionList = (dto) => $"{BaseUrl}User/GetUserMissionList?UserAccountId={dto.UserAccountId}";
    public readonly static string InsertMissionCompensation = $"{BaseUrl}User/InsertMissionCompensation";
    
    public readonly static string UpdateUserStyle = $"{BaseUrl}User/UpdateUserStyle";
    public readonly static string UpdateUserGold = $"{BaseUrl}User/UpdateUserGold";

    public readonly static string HeartBeat = $"{BaseUrl}User/HeartBeat";
    public readonly static string GameStart = $"{BaseUrl}User/GameStart";
    public readonly static string UpdateEnergy = $"{BaseUrl}User/UpdateEnergy";
    public readonly static Func<ReqDtoInsertEnergy, string> InsertEnergy = (dto) => $"{BaseUrl}User/InsertEnergy?UserAccountId={dto.UserAccountId}&Energy={dto.Energy}";

    public readonly static string UpdateRewardClaim = $"{BaseUrl}User/UpdateRewardClaim";

    //==================================================
    //     캐시 관련
    //==================================================
    public readonly static string PurchaseCashProduct = $"{BaseUrl}Cash/PurchaseCashProduct";
    public readonly static Func<ReqDtoGetCashProductList, string> GetCashProductList = (dto) => $"{BaseUrl}Cash/GetCashProductList";

}

public class WebContentsManager
{
    public void GetUserAccount(ReqDtoGetUserAccount requestDto, Action<ResDtoGetUserAccount> onSuccess = null, Action<EStatusCode> onFailed = null)
    {
        Managers.Web.SendGetRequest(WebRoute.GetUserAccount(requestDto), (response) =>
        {
            CommonResult<ResDtoGetUserAccount> rv = JsonConvert.DeserializeObject<CommonResult<ResDtoGetUserAccount>>(response);
            
            if (rv == null)
            {
                Debug.Log("GetUserAccount: 응답 파싱 실패 (null)");
                onFailed?.Invoke(EStatusCode.ServerException);
            }
            else
            {
                Debug.Log($"응답 상태: IsSuccess={rv.IsSuccess}, StatusCode={rv.StatusCode}, Data={rv.Data != null}");
                
                if (rv.IsSuccess && rv.StatusCode == EStatusCode.OK && rv.Data != null)
                {
                    Debug.Log("성공 콜백 호출");
                    onSuccess?.Invoke(rv.Data);
                }
                else
                {
                    Debug.Log($"실패 콜백 호출: {rv.StatusCode}");
                    onFailed?.Invoke(rv.StatusCode);
                }
            }
        });
    }
    public void GetUserAccountByGoogle(ReqDtoGetUserAccountByGoogle requestDto, Action<ResDtoGetUserAccountByGoogle> onSuccess = null, Action<EStatusCode> onFailed = null)
    {
        Managers.Web.SendGetRequest(WebRoute.GetUserAccountByGoogle(requestDto), (response) =>
        {
            CommonResult<ResDtoGetUserAccountByGoogle> rv = JsonConvert.DeserializeObject<CommonResult<ResDtoGetUserAccountByGoogle>>(response);
            
            if (rv == null)
            {
                Debug.Log("GetUserAccount: 응답 파싱 실패 (null)");
                onFailed?.Invoke(EStatusCode.ServerException);
            }
            else
            {
                Debug.Log($"응답 상태: IsSuccess={rv.IsSuccess}, StatusCode={rv.StatusCode}, Data={rv.Data != null}");
                
                if (rv.IsSuccess && rv.StatusCode == EStatusCode.OK && rv.Data != null)
                {
                    Debug.Log("성공 콜백 호출");
                    onSuccess?.Invoke(rv.Data);
                }
                else
                {
                    Debug.Log($"실패 콜백 호출: {rv.StatusCode}");
                    onFailed?.Invoke(rv.StatusCode);
                }
            }
        });
    }
    public void CheckUserAccountUserNameExists(ReqDtoUserAccountUserName requestDto, Action<ResDtoUserAccountUserName> onSuccess = null, Action<EStatusCode> onFailed = null)
    {
        string body = JsonConvert.SerializeObject(requestDto, Formatting.Indented);

        Managers.Web.SendPostRequest(WebRoute.CheckUserAccountUserNameExists, body , (response) =>
        {
            CommonResult<ResDtoUserAccountUserName> rv = JsonConvert.DeserializeObject<CommonResult<ResDtoUserAccountUserName>>(response);

            if (rv == null)
            {
                onFailed?.Invoke(EStatusCode.ServerException);
            }
            else if (!rv.IsSuccess || rv.StatusCode != EStatusCode.OK)
            {
                if(rv.StatusCode == EStatusCode.NameAlreadyExists) 
                {
                    onFailed.Invoke(rv.StatusCode);
                }
                if (rv.StatusCode != EStatusCode.OK) 
                {
                    onFailed.Invoke(rv.StatusCode);
                }
            }
            else
            {
                onSuccess?.Invoke(rv.Data);
            }

            // if (rv == null || false == rv.IsSuccess)
            // {
            //     onFailed.Invoke(EStatusCode.ServerException);
            // }
            // else
            // {
            //     Debug.Log("ReqGetValidateUserAccountUserName");

            //     if(rv.StatusCode == EStatusCode.NameAlreadyExists) // 안들어와짐
            //     {
            //         onFailed.Invoke(rv.StatusCode);
            //         //Debug.Log("ReqGetValidateUserAccountUserName");
            //     }
            //     if (rv.StatusCode != EStatusCode.OK) // 안들어와짐
            //     {
            //         onFailed.Invoke(rv.StatusCode);
            //         Managers.Scene.LoadScene(EScene.SignInScene);
            //     }
            //     else
            //     {
            //         onSuccess.Invoke(rv.Data);
            //     }
            // }
        });
    }
    public void CheckUserAccountNicknameExists(ReqDtoUserAccountNickname requestDto, Action<ResDtoUserAccountNickname> onSuccess = null, Action<EStatusCode> onFailed = null)
    {
        string body = JsonConvert.SerializeObject(requestDto, Formatting.Indented);

        Managers.Web.SendPostRequest(WebRoute.CheckUserAccountNicknameExists, body , (response) =>
        {
            CommonResult<ResDtoUserAccountNickname> rv = JsonConvert.DeserializeObject<CommonResult<ResDtoUserAccountNickname>>(response);

            if (rv == null)
            {
                onFailed?.Invoke(EStatusCode.ServerException);
            }
            else if (!rv.IsSuccess || rv.StatusCode != EStatusCode.OK)
            {
                if(rv.StatusCode == EStatusCode.NameAlreadyExists)
                {
                    onFailed.Invoke(rv.StatusCode);
                }
                if (rv.StatusCode != EStatusCode.OK) 
                {
                    onFailed.Invoke(rv.StatusCode);
                }
            }
            else
            {
                onSuccess?.Invoke(rv.Data);
            }
        });
    }
    public void CheckGoogleAccountExists(ReqDtoGoogleAccount requestDto, Action<ResDtoGoogleAccount> onSuccess = null, Action<EStatusCode> onFailed = null)
    {
        string body = JsonConvert.SerializeObject(requestDto, Formatting.Indented);

        Managers.Web.SendPostRequest(WebRoute.CheckGoogleAccountExists, body, (response) =>
        {
            CommonResult<ResDtoGoogleAccount> rv = JsonConvert.DeserializeObject<CommonResult<ResDtoGoogleAccount>>(response);

            //if (rv == null || false == rv.IsSuccess) // false면 무조건 serverException이 되어서 여태 StatusCode를 사용할 수 없었음.
            if (rv == null)
            {
                onFailed?.Invoke(EStatusCode.ServerException);
            }
            else if (!rv.IsSuccess || rv.StatusCode != EStatusCode.OK)
            {
                // IsSuccess가 false이거나 StatusCode가 OK가 아닌 경우 서버에서 보낸 StatusCode를 그대로 사용
                onFailed?.Invoke(rv.StatusCode);
            }
            else
            {
                onSuccess?.Invoke(rv.Data);
            }
        });
    }
    public void InsertUserAccount(ReqDtoInsertUserAccount requestDto, Action<ResDtoInsertUserAccount> onSuccess = null, Action<EStatusCode> onFailed = null)
    {
        string body = JsonConvert.SerializeObject(requestDto, Formatting.Indented);

        Managers.Web.SendPostRequest(WebRoute.InsertUserAccount, body , (response) =>
        {
            CommonResult<ResDtoInsertUserAccount> rv = JsonConvert.DeserializeObject<CommonResult<ResDtoInsertUserAccount>>(response);

            if (rv == null)
            {
                onFailed?.Invoke(EStatusCode.ServerException);
            }
            else if (!rv.IsSuccess || rv.StatusCode != EStatusCode.OK)
            {
                // IsSuccess가 false이거나 StatusCode가 OK가 아닌 경우 서버에서 보낸 StatusCode를 그대로 사용
                onFailed?.Invoke(rv.StatusCode);
            }
            else
            {
                onSuccess?.Invoke(rv.Data);
            }
        });
    }
    public void InsertUserAccountScore(ReqDtoInsertUserAccountScore requestDto, Action<ResDtoInsertUserAccountScore> onSuccess = null, Action<EStatusCode> onFailed = null)
    {
        string body = JsonConvert.SerializeObject(requestDto, Formatting.Indented);

        Managers.Web.SendPostRequest(WebRoute.InsertUserAccountScore, body , (response) =>
        {
            CommonResult<ResDtoInsertUserAccountScore> rv = JsonConvert.DeserializeObject<CommonResult<ResDtoInsertUserAccountScore>>(response); 

            if (rv == null)
            {
                onFailed?.Invoke(EStatusCode.ServerException);
            }
            else 
            {
                // 상태 로그 추가
                Debug.Log($"[InsertUserAccountScore] IsSuccess: {rv.IsSuccess}, StatusCode: {rv.StatusCode}");
                
                if (!rv.IsSuccess || rv.StatusCode != EStatusCode.OK)
                {
                    onFailed?.Invoke(rv.StatusCode);
                }
                else
                {
                    onSuccess?.Invoke(rv.Data);
                }
            }
        });
    }
    // public void InsertUserAccountNickname(ReqDtoInsertUserAccountNickname requestDto, Action<ResDtoInsertUserAccountNickname> onSuccess = null, Action<EStatusCode> onFailed = null)
    // {
    //     string body = JsonConvert.SerializeObject(requestDto, Formatting.Indented);

    //     Managers.Web.SendPostRequest(WebRoute.InsertUserAccountNickname, body , (response) =>
    //     {
    //         CommonResult<ResDtoInsertUserAccountNickname> rv = JsonConvert.DeserializeObject<CommonResult<ResDtoInsertUserAccountNickname>>(response);

    //         if (rv == null || false == rv.IsSuccess)
    //         {
    //             onFailed.Invoke(EStatusCode.ServerException);
    //         }
    //         else
    //         {
    //             if (rv.StatusCode != EStatusCode.OK)
    //             {
    //                 onFailed.Invoke(rv.StatusCode);
    //             }
    //             else
    //             {
    //                 onSuccess.Invoke(rv.Data);
    //             }
    //         }
    //     });
    // }
    //GetUserAccountPassword
    public void GetUserAccountPassword(ReqDtoGetUserAccountPassword requestDto, Action<ResDtoGetUserAccountPassword> onSuccess = null, Action<EStatusCode> onFailed = null)
    {
        Managers.Web.SendGetRequest(WebRoute.GetUserAccountPassword(requestDto), (response) =>
        {
            CommonResult<ResDtoGetUserAccountPassword> rv = JsonConvert.DeserializeObject<CommonResult<ResDtoGetUserAccountPassword>>(response);

            if (rv == null)
            {
                onFailed?.Invoke(EStatusCode.ServerException);
            }
            else if (!rv.IsSuccess || rv.StatusCode != EStatusCode.OK)
            {
                // IsSuccess가 false이거나 StatusCode가 OK가 아닌 경우 서버에서 보낸 StatusCode를 그대로 사용
                onFailed?.Invoke(rv.StatusCode);
            }
            else
            {
                onSuccess?.Invoke(rv.Data);
            }
        });
    }
    // XXX
    public void GetOrAddUserAccount(ReqDtoGetOrAddUserAccount requestDto, Action<ResDtoGetOrAddUserAccount> onSuccess = null, Action<EStatusCode> onFailed = null)
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
                    Managers.Game.UserInfo.UserName = rv.Data.UserName;
                    Managers.Game.UserInfo.UserNickname = rv.Data.Nickname;
                    onSuccess.Invoke(rv.Data);
                }
            }
        });
    }
    public void GetUserAccountList(ReqDtoGetUserAccountList requestDto, Action<ResDtoGetUserAccountList> onSuccess = null, Action<EStatusCode> onFailed = null)
    {
        Managers.Web.SendGetRequest(WebRoute.GetUserAccountList(requestDto), (response) =>
        {
            CommonResult<ResDtoGetUserAccountList> rv = JsonConvert.DeserializeObject<CommonResult<ResDtoGetUserAccountList>>(response);
            
            if (rv == null)
            {
                onFailed?.Invoke(EStatusCode.ServerException);
            }
            else if (!rv.IsSuccess || rv.StatusCode != EStatusCode.OK)
            {
                // IsSuccess가 false이거나 StatusCode가 OK가 아닌 경우 서버에서 보낸 StatusCode를 그대로 사용
                onFailed?.Invoke(rv.StatusCode);
            }
            else
            {
                onSuccess?.Invoke(rv.Data);
            }
        });
    }
    public void InsertUserMission(ReqDtoInsertUserMissionList requestDto, Action<ResDtoInsertUserMissionList> onSuccess = null, Action<EStatusCode> onFailed = null)
    {
        string body = JsonConvert.SerializeObject(requestDto, Formatting.Indented);
        Managers.Web.SendPostRequest(WebRoute.InsertUserMissionList, body , (response) =>
        {
            CommonResult<ResDtoInsertUserMissionList> rv = JsonConvert.DeserializeObject<CommonResult<ResDtoInsertUserMissionList>>(response);

            if (rv == null)
            {
                onFailed?.Invoke(EStatusCode.ServerException);
            }
            else if (!rv.IsSuccess || rv.StatusCode != EStatusCode.OK)
            {
                // IsSuccess가 false이거나 StatusCode가 OK가 아닌 경우 서버에서 보낸 StatusCode를 그대로 사용
                onFailed?.Invoke(rv.StatusCode);
            }
            else
            {
                onSuccess?.Invoke(rv.Data);
            }
        });
    }
    public void CompleteUserMission(ReqDtoCompleteUserMission requestDto, Action<ResDtoCompleteUserMissionList> onSuccess = null, Action<EStatusCode> onFailed = null)
    {
        string body = JsonConvert.SerializeObject(requestDto, Formatting.Indented);
        Managers.Web.SendPostRequest(WebRoute.CompleteUserMission, body , (response) =>
        {
            CommonResult<ResDtoCompleteUserMissionList> rv = JsonConvert.DeserializeObject<CommonResult<ResDtoCompleteUserMissionList>>(response);

            if (rv == null)
            {
                onFailed?.Invoke(EStatusCode.ServerException);
            }
            else if (!rv.IsSuccess || rv.StatusCode != EStatusCode.OK)
            {
                // IsSuccess가 false이거나 StatusCode가 OK가 아닌 경우 서버에서 보낸 StatusCode를 그대로 사용
                onFailed?.Invoke(rv.StatusCode);
            }
            else
            {
                onSuccess?.Invoke(rv.Data);
            }
        });
    }
    public void UpdateUserMissionList(ReqDtoUpdateUserMissionList requestDto, Action<ResDtoUpdateUserMissionList> onSuccess = null, Action<EStatusCode> onFailed = null)
    {
        string body = JsonConvert.SerializeObject(requestDto, Formatting.Indented);
        Managers.Web.SendPostRequest(WebRoute.UpdateUserMissionList, body , (response) =>
        {
            CommonResult<ResDtoUpdateUserMissionList> rv = JsonConvert.DeserializeObject<CommonResult<ResDtoUpdateUserMissionList>>(response);

            if (rv == null)
            {
                onFailed?.Invoke(EStatusCode.ServerException);
            }
            else if (!rv.IsSuccess || rv.StatusCode != EStatusCode.OK)
            {
                // IsSuccess가 false이거나 StatusCode가 OK가 아닌 경우 서버에서 보낸 StatusCode를 그대로 사용
                onFailed?.Invoke(rv.StatusCode);
            }
            else
            {
                onSuccess?.Invoke(rv.Data);
            }
        });
    }
    public void GetUserMissionList(ReqDtoGetUserMissionList requestDto, Action<ResDtoGetUserMissionList> onSuccess = null, Action<EStatusCode> onFailed = null)
    {
        string body = JsonConvert.SerializeObject(requestDto, Formatting.Indented);
        Managers.Web.SendGetRequest(WebRoute.GetUserMissionList(requestDto), (response) =>
        {
            CommonResult<ResDtoGetUserMissionList> rv = JsonConvert.DeserializeObject<CommonResult<ResDtoGetUserMissionList>>(response);
            
            if (rv == null)
            {
                onFailed?.Invoke(EStatusCode.ServerException);
            }
            else 
            {
                // 상태 로그 추가
                Debug.Log($"[GetUserMissionList] IsSuccess: {rv.IsSuccess}, StatusCode: {rv.StatusCode}");
                
                if (!rv.IsSuccess || rv.StatusCode != EStatusCode.OK)
                {
                    onFailed?.Invoke(rv.StatusCode);
                }
                else
                {
                    onSuccess?.Invoke(rv.Data);
                }
            }
        });
    }
    public void UpdateUserStyle(ReqDtoUpdateUserStyle requestDto, Action<ResDtoUpdateUserStyle> onSuccess = null, Action<EStatusCode> onFailed = null)
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
                    UI_ErrorButtonPopup.ShowErrorButton(Managers.Error.GetError(Define.EErrorCode.ERR_NetworkSettlementErrorResend));
                }
                else
                {
                    onSuccess.Invoke(rv.Data);
                    Managers.Scene.LoadScene(EScene.SuberunkerSceneHomeScene);
                }
            }
        });
    }
    public void UpdateUserGold(ReqDtoUpdateUserGold requestDto, Action<ResDtoUpdateUserGold> onSuccess = null, Action<EStatusCode> onFailed = null)
    {
        string body = JsonConvert.SerializeObject(requestDto, Formatting.Indented);

        Managers.Web.SendPostRequest(WebRoute.UpdateUserGold, body , (response) =>
        {
            CommonResult<ResDtoUpdateUserGold> rv = JsonConvert.DeserializeObject<CommonResult<ResDtoUpdateUserGold>>(response);

            if (rv == null)
            {
                onFailed?.Invoke(EStatusCode.ServerException);
            }
            else if (!rv.IsSuccess || rv.StatusCode != EStatusCode.OK)
            {
                // IsSuccess가 false이거나 StatusCode가 OK가 아닌 경우 서버에서 보낸 StatusCode를 그대로 사용
                onFailed?.Invoke(rv.StatusCode);
            }
            else
            {
                onSuccess?.Invoke(rv.Data);
            }
        });
    }
    public void HeartBeat(ReqDtoHeartBeat requestDto, Action<ResDtoHeartBeat> onSuccess = null, Action<EStatusCode> onFailed = null)
    {
        string body = JsonConvert.SerializeObject(requestDto, Formatting.Indented);

        Managers.Web.SendPostRequest(WebRoute.HeartBeat, body , (response) =>
        {
            CommonResult<ResDtoHeartBeat> rv = JsonConvert.DeserializeObject<CommonResult<ResDtoHeartBeat>>(response);

            if (rv == null)
            {
                onFailed?.Invoke(EStatusCode.ServerException);
            }
            else if (!rv.IsSuccess || rv.StatusCode != EStatusCode.OK)
            {
                // IsSuccess가 false이거나 StatusCode가 OK가 아닌 경우 서버에서 보낸 StatusCode를 그대로 사용
                onFailed?.Invoke(rv.StatusCode);
            }
            else
            {
                onSuccess?.Invoke(rv.Data);
            }
        });
    }
    public void GameStart(ReqDtoGameStart requestDto, Action<ResDtoGameStart> onSuccess = null, Action<EStatusCode> onFailed = null)
    {
        string body = JsonConvert.SerializeObject(requestDto, Formatting.Indented);

        Managers.Web.SendPostRequest(WebRoute.GameStart, body , (response) =>
        {
            CommonResult<ResDtoGameStart> rv = JsonConvert.DeserializeObject<CommonResult<ResDtoGameStart>>(response);

            if (rv == null)
            {
                onFailed?.Invoke(EStatusCode.ServerException);
            }
            else if (!rv.IsSuccess || rv.StatusCode != EStatusCode.OK)
            {
                // IsSuccess가 false이거나 StatusCode가 OK가 아닌 경우 서버에서 보낸 StatusCode를 그대로 사용
                onFailed?.Invoke(rv.StatusCode);
            }
            else
            {
                onSuccess?.Invoke(rv.Data);
            }
        });
    }
    public void UpdateEnergy(ReqDtoUpdateEnergy requestDto, Action<ResDtoUpdateEnergy> onSuccess = null, Action<EStatusCode> onFailed = null)
    {
        string body = JsonConvert.SerializeObject(requestDto, Formatting.Indented);

        Managers.Web.SendPostRequest(WebRoute.UpdateEnergy, body , (response) =>
        {
            CommonResult<ResDtoUpdateEnergy> rv = JsonConvert.DeserializeObject<CommonResult<ResDtoUpdateEnergy>>(response);

            if (rv == null)
            {
                onFailed?.Invoke(EStatusCode.ServerException);
            }
            else if (!rv.IsSuccess || rv.StatusCode != EStatusCode.OK)
            {
                // IsSuccess가 false이거나 StatusCode가 OK가 아닌 경우 서버에서 보낸 StatusCode를 그대로 사용
                onFailed?.Invoke(rv.StatusCode);
            }
            else
            {
                onSuccess?.Invoke(rv.Data);
            }
        });
    }
    public void PurchaseCashProduct(ReqDtoPurchaseCashProduct requestDto, Action<ResDtoPurchaseCashProduct> onSuccess = null, Action<EStatusCode> onFailed = null)
    {
        string body = JsonConvert.SerializeObject(requestDto, Formatting.Indented);

        Managers.Web.SendPostRequest(WebRoute.PurchaseCashProduct, body, (response) =>
        {
            CommonResult<ResDtoPurchaseCashProduct> rv = JsonConvert.DeserializeObject<CommonResult<ResDtoPurchaseCashProduct>>(response);

            if (rv == null)
            {
                onFailed?.Invoke(EStatusCode.ServerException);
            }
            else if (!rv.IsSuccess || rv.StatusCode != EStatusCode.OK)
            {
                // IsSuccess가 false이거나 StatusCode가 OK가 아닌 경우 서버에서 보낸 StatusCode를 그대로 사용
                onFailed?.Invoke(rv.StatusCode);
            }
            else
            {
                onSuccess?.Invoke(rv.Data);
            }
        });
    }
    public void GetCashProductList(ReqDtoGetCashProductList requestDto, Action<ResDtoGetCashProductList> onSuccess = null, Action<EStatusCode> onFailed = null)
    {
        Managers.Web.SendGetRequest(WebRoute.GetCashProductList(requestDto), (response) =>
        {
            CommonResult<ResDtoGetCashProductList> rv = JsonConvert.DeserializeObject<CommonResult<ResDtoGetCashProductList>>(response);

            if (rv == null)
            {
                onFailed?.Invoke(EStatusCode.ServerException);
            }
            else if (!rv.IsSuccess || rv.StatusCode != EStatusCode.OK)
            {
                // IsSuccess가 false이거나 StatusCode가 OK가 아닌 경우 서버에서 보낸 StatusCode를 그대로 사용
                onFailed?.Invoke(rv.StatusCode);
            }
            else
            {
                onSuccess?.Invoke(rv.Data);
            }
        });
    }
    public void InsertEnergy(ReqDtoInsertEnergy requestDto, Action<ResDtoInsertEnergy> onSuccess = null, Action<EStatusCode> onFailed = null)
    {
        Managers.Web.SendGetRequest(WebRoute.InsertEnergy(requestDto), (response) =>
        {
            CommonResult<ResDtoInsertEnergy> rv = JsonConvert.DeserializeObject<CommonResult<ResDtoInsertEnergy>>(response);
            
            if (rv == null)
            {
                onFailed?.Invoke(EStatusCode.ServerException);
            }
            else if (!rv.IsSuccess || rv.StatusCode != EStatusCode.OK)
            {
                // IsSuccess가 false이거나 StatusCode가 OK가 아닌 경우 서버에서 보낸 StatusCode를 그대로 사용
                onFailed?.Invoke(rv.StatusCode);
            }
            else
            {
                onSuccess?.Invoke(rv.Data);
            }
        });
    }
    public void UpdateRewardClaim(ReqDtoUpdateRewardClaim requestDto, Action<ResDtoUpdateRewardClaim> onSuccess = null, Action<EStatusCode> onFailed = null)
    {
        string body = JsonConvert.SerializeObject(requestDto, Formatting.Indented);

        Managers.Web.SendPostRequest(WebRoute.UpdateRewardClaim, body, (response) =>
        {
            CommonResult<ResDtoUpdateRewardClaim> rv = JsonConvert.DeserializeObject<CommonResult<ResDtoUpdateRewardClaim>>(response);
            
            if (rv == null)
            {
                onFailed?.Invoke(EStatusCode.ServerException);
            }
            else if (!rv.IsSuccess || rv.StatusCode != EStatusCode.OK)
            {
                // IsSuccess가 false이거나 StatusCode가 OK가 아닌 경우 서버에서 보낸 StatusCode를 그대로 사용
                onFailed?.Invoke(rv.StatusCode);
            }
            else
            {
                onSuccess?.Invoke(rv.Data);
            }
        });
    }
    public void InsertGoogleAccount(ReqDtoInsertGoogleAccount requestDto, Action<ResDtoInsertGoogleAccount> onSuccess = null, Action<EStatusCode> onFailed = null)
    {
        string body = JsonConvert.SerializeObject(requestDto, Formatting.Indented);

        Managers.Web.SendPostRequest(WebRoute.InsertGoogleAccount, body, (response) =>
        {
            CommonResult<ResDtoInsertGoogleAccount> rv = JsonConvert.DeserializeObject<CommonResult<ResDtoInsertGoogleAccount>>(response);
            
            if (rv == null)
            {
                onFailed?.Invoke(EStatusCode.ServerException);
            }
            else if (!rv.IsSuccess || rv.StatusCode != EStatusCode.OK)
            {
                // IsSuccess가 false이거나 StatusCode가 OK가 아닌 경우 서버에서 보낸 StatusCode를 그대로 사용
                onFailed?.Invoke(rv.StatusCode);
            }
            else
            {
                onSuccess?.Invoke(rv.Data);
            }
        });
    }

    public void BindUserAccountToGoogle(ReqDtoBindUserAccountToGoogle requestDto, Action<ResDtoBindUserAccountToGoogle> onSuccess = null, Action<EStatusCode> onFailed = null)
    {
        string body = JsonConvert.SerializeObject(requestDto, Formatting.Indented);

        Managers.Web.SendPostRequest(WebRoute.BindUserAccountToGoogle, body, (response) =>
        {
            CommonResult<ResDtoBindUserAccountToGoogle> rv = JsonConvert.DeserializeObject<CommonResult<ResDtoBindUserAccountToGoogle>>(response);
            
            if (rv == null)
            {
                onFailed?.Invoke(EStatusCode.ServerException);
            }
            else if (!rv.IsSuccess || rv.StatusCode != EStatusCode.OK)
            {
                // IsSuccess가 false이거나 StatusCode가 OK가 아닌 경우 서버에서 보낸 StatusCode를 그대로 사용
                onFailed?.Invoke(rv.StatusCode);
            }
            else
            {
                onSuccess?.Invoke(rv.Data);
            }
        });
    }
}