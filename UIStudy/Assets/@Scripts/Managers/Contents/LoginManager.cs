using System;
using GameApi.Dtos;
using UnityEngine;
using WebApi.Models.Dto;
using static Define;

public class LoginManager
{
    private LoginSlave _slave;
    private bool _isInitialized = false;

    public void Init()
    {
        if (_isInitialized)
            return;

        GameObject slave = new GameObject("LoginSlave");
        _slave = slave.GetOrAddComponent<LoginSlave>();
        GameObject.DontDestroyOnLoad(slave);

        _isInitialized = true;
    }

    public void ProcessGoogleAccount(string googleAccount, bool isAccountMerge = false)
    {
        Debug.Log($"ProcessGoogleAccount: googleAccount={googleAccount}, isAccountMerge={isAccountMerge}");
        
        // 구글 계정 존재 여부 확인
        Managers.WebContents.CheckGoogleAccountExists(new ReqDtoGoogleAccount()
        {
            GoogleAccount = googleAccount
        }, 
        // 성공 콜백: 구글 계정이 존재하지 않음
        (response) =>
        {
            if (isAccountMerge)
            {
                // 합병 처리 - 구글 계정이 신규이므로 합병 가능
                Debug.Log($"구글 계정 합병 가능: {googleAccount}");
                Managers.Game.UserInfo.GoogleAccount = googleAccount;
                
                // 확인 팝업 표시
                UI_ErrorButtonPopup.ShowErrorButton(Managers.Error.GetError(EErrorCode.ERR_AccountCreationCancellation), 
                    () => {
                        BindUserAccountToGoogle();
                    });
            }
            else
            {
                // 새 계정 생성 처리 - 구글 계정이 신규이므로 가입 가능
                Debug.Log($"새 구글 계정으로 가입 가능: {googleAccount}");
                Managers.Game.UserInfo.GoogleAccount = googleAccount;
                // UI_ToastPopup.ShowError(Managers.Error.GetError(EErrorCode.ERR_ValidationNickname));
                Managers.Scene.LoadScene(EScene.InputNicknameScene);
            }
        }, 
        // 실패 콜백
        (errorCode) =>
        {
            if (errorCode == EStatusCode.GoogleAccountAlreadyExists)
            {
                if (isAccountMerge)
                {
                    // 합병 처리 - 구글 계정이 이미 사용 중이므로 합병 불가
                    Debug.Log($"구글 계정 합병 불가능 (이미 사용 중): {googleAccount}");
                    UI_ToastPopup.ShowError(Managers.Error.GetError(EErrorCode.ERR_GoogleAccountAlreadyExists), 3,
                        ()=>
                        {
                            Managers.Game.UserInfo.GoogleAccount = googleAccount;
                            SignInWithGoogle();
                        });
                }
                else
                {
                    // 로그인 처리 - 이미 가입된 구글 계정이므로 바로 로그인
                    Debug.Log($"기존 구글 계정으로 자동 로그인: {googleAccount}");
                    Managers.Game.UserInfo.GoogleAccount = googleAccount;
                    SignInWithGoogle();
                }
            }
            else
            {
                // 기타 오류 처리
                Debug.LogError($"구글 계정 처리 중 오류 발생: {errorCode}");
                UI_ToastPopup.ShowError(Managers.Error.GetError(EErrorCode.ERR_NetworkSaveError));
            }
        });
    }

    // 구글 계정으로 로그인
    public void SignInWithGoogle()
    {
        Debug.Log($"구글 계정으로 로그인: {Managers.Game.UserInfo.GoogleAccount}");
        
        var loadingComplete = UI_LoadingPopup.Show();
        
        Managers.WebContents.GetUserAccountByGoogle(new ReqDtoGetUserAccountByGoogle()
        {
            GoogleAccount = Managers.Game.UserInfo.GoogleAccount
        },
        // 성공 콜백
        (response) =>
        {
            // 사용자 정보 설정
            Managers.Game.UserInfo.UserName = response.UserName;
            Managers.Game.UserInfo.UserNickname = response.Nickname;
            Managers.Game.UserInfo.UserAccountId = response.UserAccountId;
            Managers.Game.UserInfo.GoogleAccount = response.GoogleAccount;
            Managers.SignalR.LoginUser(Managers.Game.UserInfo.UserAccountId);

            //캐릭터 스타일 저장
            Managers.Game.ChracterStyleInfo.CharacterId = response.CharacterId;
            Managers.Game.ChracterStyleInfo.Hair = response.HairStyle;
            Managers.Game.ChracterStyleInfo.Eyebrows = response.EyebrowStyle;
            Managers.Game.ChracterStyleInfo.Eyes = response.EyesStyle;
            Managers.Game.UserInfo.EvolutionId = response.Evolution;
            Managers.Game.UserInfo.EvolutionSetLevel = response.EvolutionSetLevel;

            // Energy
            Managers.Game.UserInfo.Energy = response.Energy;
            Managers.Game.UserInfo.LatelyEnergy = response.LatelyEnergy;
            Managers.Game.UserInfo.PurchaseEnergyCountToday = response.PurchaseEnergyCountToday;

            // 일일 보상
            Managers.Game.UserInfo.LastRewardClaimTime = response.LastRewardClaimTime;

            //게임 진행 정보
            Managers.Game.UserInfo.RecordScore = response.HighScore;
            Managers.Game.UserInfo.LatelyScore = response.LatelyScore;
            Managers.Game.UserInfo.Gold = response.Gold;
            Managers.Game.UserInfo.PlayTime = response.PlayTime;
            Managers.Game.UserInfo.AccumulatedStone = response.AccumulatedStone;
            Managers.Game.UserInfo.StageLevel = response.StageLevel;
            
            // 계정 정보 저장
            SaveUserAccountInfo();
            
            Managers.Event.TriggerEvent(EEventType.OnSettlementComplete);
            Managers.Event.TriggerEvent(EEventType.OnFirstAccept);
            
            loadingComplete.Value = true;

            // 에너지 업데이트 요청 실행
            _slave.StartUpdateEnergy();
            
        },
        // 실패 콜백
        (errorCode) =>
        {
            loadingComplete.Value = true;
            Debug.Log($"구글 계정 로그인 실패: {errorCode}");
            //Managers.Scene.LoadScene(EScene.SignInScene);
            UI_ToastPopup.ShowError(Managers.Error.GetError(EErrorCode.ERR_NetworkSaveError), 2f, () => Managers.Scene.LoadScene(EScene.SignInScene));
        });
    }

    // 일반 계정으로 로그인
    public void SignIn(string username, string password)
    {
        Debug.Log($"일반 계정으로 로그인: {username}");
        
        var loadingComplete = UI_LoadingPopup.Show();
        
        Managers.Game.UserInfo.UserName = username;
        Managers.Game.UserInfo.Password = password;
        
        Managers.WebContents.GetUserAccount(new ReqDtoGetUserAccount()
        {
            UserName = username,
            Password = password
        },
        // 성공 콜백
        (response) =>
        {
            // 사용자 정보 설정
            Managers.Game.UserInfo.UserName = response.UserName;
            Managers.Game.UserInfo.UserNickname = response.Nickname;
            Managers.Game.UserInfo.UserAccountId = response.UserAccountId;
            Managers.Game.UserInfo.GoogleAccount = response.GoogleAccount;
            Managers.SignalR.LoginUser(Managers.Game.UserInfo.UserAccountId);

            //캐릭터 스타일 저장
            Managers.Game.ChracterStyleInfo.CharacterId = response.CharacterId;
            Managers.Game.ChracterStyleInfo.Hair = response.HairStyle;
            Managers.Game.ChracterStyleInfo.Eyebrows = response.EyebrowStyle;
            Managers.Game.ChracterStyleInfo.Eyes = response.EyesStyle;
            Managers.Game.UserInfo.EvolutionId = response.Evolution;
            Managers.Game.UserInfo.EvolutionSetLevel = response.EvolutionSetLevel;

            // Energy
            Managers.Game.UserInfo.Energy = response.Energy;
            Managers.Game.UserInfo.LatelyEnergy = response.LatelyEnergy;
            Managers.Game.UserInfo.PurchaseEnergyCountToday = response.PurchaseEnergyCountToday;

            // 일일 보상
            Managers.Game.UserInfo.LastRewardClaimTime = response.LastRewardClaimTime;

            //게임 진행 정보
            Managers.Game.UserInfo.RecordScore = response.HighScore;
            Managers.Game.UserInfo.LatelyScore = response.LatelyScore;
            Managers.Game.UserInfo.Gold = response.Gold;
            Managers.Game.UserInfo.PlayTime = response.PlayTime;
            Managers.Game.UserInfo.AccumulatedStone = response.AccumulatedStone;
            Managers.Game.UserInfo.StageLevel = response.StageLevel;
            
            // 계정 정보 저장
            SaveUserAccountInfo();
            
            Managers.Event.TriggerEvent(EEventType.OnSettlementComplete);
            Managers.Event.TriggerEvent(EEventType.OnFirstAccept);

            loadingComplete.Value = true;

            // 에너지 업데이트 요청 실행
            _slave.StartUpdateEnergy();
        },
        // 실패 콜백
        (errorCode) =>
        {
            loadingComplete.Value = true;
            Debug.Log($"일반 계정 로그인 실패: {errorCode}");
            //Managers.Scene.LoadScene(EScene.SignInScene);
            UI_ToastPopup.ShowError(Managers.Error.GetError(EErrorCode.ERR_InvalidCredentials), 2f, () => Managers.Scene.LoadScene(EScene.SignInScene));
        });
    }

    // 기존 계정에 구글 계정 연결
    public void BindUserAccountToGoogle()
    {
        Debug.Log($"기존 계정에 구글 계정 연결: UserAccountId={Managers.Game.UserInfo.UserAccountId}, GoogleAccount={Managers.Game.UserInfo.GoogleAccount}");
        
        var loadingComplete = UI_LoadingPopup.Show();
        
        Managers.WebContents.BindUserAccountToGoogle(new ReqDtoBindUserAccountToGoogle()
        {
            UserAccountId = Managers.Game.UserInfo.UserAccountId,
            GoogleAccount = Managers.Game.UserInfo.GoogleAccount
        },
        // 성공 콜백
        (response) =>
        {
            loadingComplete.Value = true;
            Debug.Log("계정 합병 성공");
            
            // 계정 정보 저장
            SaveUserAccountInfo();
            UI_ToastPopup.Show("구글 계정이 성공적으로 연결되었습니다.", UI_ToastPopup.Type.Info);
        },
        // 실패 콜백
        (errorCode) =>
        {
            loadingComplete.Value = true;
            Debug.LogError($"계정 합병 실패: {errorCode}");
            UI_ToastPopup.ShowError(Managers.Error.GetError(EErrorCode.ERR_NetworkSaveError));
        });
    }

    // 자동 로그인 시도
    public void TryAutoLogin()
    {
        string usernameData = SecurePlayerPrefs.GetString(HardCoding.UserName, "");
        string passwordData = SecurePlayerPrefs.GetString(HardCoding.Password, ""); 
        string googleAccountData = SecurePlayerPrefs.GetString(HardCoding.GoogleAccount, ""); 
        
        Managers.Game.UserInfo.UserName = usernameData;
        Managers.Game.UserInfo.Password = passwordData;
        Managers.Game.UserInfo.GoogleAccount = googleAccountData;

        Debug.Log("1--1 usernameData : " + usernameData);
        Debug.Log("1--1 passwordData : " + passwordData);
        Debug.Log("1--1 googleAccountData : " + googleAccountData);
        
        // 로그인 정보가 전혀 없는 경우
        if (string.IsNullOrEmpty(usernameData) && string.IsNullOrEmpty(passwordData) && string.IsNullOrEmpty(googleAccountData)
        || (usernameData == HardCoding.UserName && passwordData == HardCoding.Password) && (googleAccountData == "0" || googleAccountData == HardCoding.GoogleAccount)) 
        {
            Managers.Scene.LoadScene(EScene.SignInScene);
            return;
        }

        // 로그인 방식 결정 - 조건문 수정
        if (!string.IsNullOrEmpty(googleAccountData) && googleAccountData != "0" && googleAccountData != HardCoding.GoogleAccount)
        {
            // 구글 계정 정보가 있으면 구글 계정으로 로그인
            SignInWithGoogle();
        }
        else if (!string.IsNullOrEmpty(usernameData) && !string.IsNullOrEmpty(passwordData))
        {
            // 일반 계정 정보가 있으면 일반 로그인
            SignIn(usernameData, passwordData);
        }
        else
        {
            // 정보가 불완전한 경우
            Managers.Scene.LoadScene(EScene.SignInScene);
        }
    }

    // 사용자 정보 저장
    public void SaveUserAccountInfo()
    {
        Debug.Log("1--1 usernameData : " + Managers.Game.UserInfo.UserName);

        Debug.Log("1--1 passwordData : " + Managers.Game.UserInfo.Password);

        Debug.Log("1--1 googleAccountData : " + Managers.Game.UserInfo.GoogleAccount);

        if (!string.IsNullOrEmpty(Managers.Game.UserInfo.UserName))
        {
            SecurePlayerPrefs.SetString(HardCoding.UserName, Managers.Game.UserInfo.UserName);
        }
        
        if (!string.IsNullOrEmpty(Managers.Game.UserInfo.Password))
        {
            SecurePlayerPrefs.SetString(HardCoding.Password, Managers.Game.UserInfo.Password);
        }
        
        if (!string.IsNullOrEmpty(Managers.Game.UserInfo.GoogleAccount))
        {
            SecurePlayerPrefs.SetString(HardCoding.GoogleAccount, Managers.Game.UserInfo.GoogleAccount);
        }
        
        SecurePlayerPrefs.Save();
    }
    public void LogoutAndClearMission()
    {
        SecurePlayerPrefs.SetString(HardCoding.UserName, HardCoding.UserName);
        SecurePlayerPrefs.SetString(HardCoding.Password, HardCoding.Password);
        SecurePlayerPrefs.SetString(HardCoding.GoogleAccount, HardCoding.GoogleAccount);
        SecurePlayerPrefs.Save();
        Managers.Event.TriggerEvent(EEventType.OnLogout);
        Managers.Scene.LoadScene(EScene.SignInScene);
    }
}
