using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public static class Define
{
	public enum EScene
	{
		Unknown,
		DevLoadingScene,
        StartLoadingScene,
		TinyFarmScene,
        SignUpScene,
        SignInScene,
        SuberunkerSceneHomeScene,
        SuberunkerTimelineScene,
        ChooseCharacterScene,
        InputNicknameScene,
        SuberunkerScene,
        SignalRTestScene, // test
        LoadingPageTimelineScene
    }

	public enum EUIEvent
	{
		Click,
		PointerDown,
		PointerUp,
		BeginDrag,
		Drag,
		EndDrag,
		PointerEnter,
		PointerExit,
    }

	public enum ESound
	{
		Bgm,
		Effect,
		Max,
	}

	public enum EJoystickState
    {
        PointerDown,
        PointerUp,
        Drag
    }

	public enum EMouseEvent
	{
		Press,
		Click
	}

	public enum EPlayerState
	{
        Ready = -2,
        Relax = -1,
        Idle = 0,
        Move = 1,
        Run = 2,
        Jump = 3,
		Boring = 4,
    }

	public enum EEventType
	{
		Attacked_Player,
		SetStyle_Player,
        LuckyTrigger_Player,
        TakeItem,
        ChangePlayerLife,
        GetLife,
        GetGold,
        LevelStageUp,
        ThoughtBubble,
        CancelThoughtBubble,
        IsStoneShower,
        StopStoneShower,
        SetLanguage,
        SignIn,
        //ErrorPopup,
        //ErrorButtonPopup,
        //ToastPopupNotice,
        GetUserScoreList,
        GetMyScore,
        StartLoading,
        StopLoading,
        Mission,
        OnPlayerDead,
        OnPlayerRevive,
        OnSettlementComplete,   // 플레이 끝나고 정산 (플레이타임 등)
        OnFirstAccept,          // 로그인할떄 미션 리스트 체크
        OnMissionComplete,      // 미션 완료 체크
        OnUpdateMission,        // 진행중 업데이트
        OnLogout,         // 로그아웃 시  
        UIRefresh,
        Evolution,
        Purchase,
        UIStoneCountRefresh,
        UpdateEnergy,
        ReceiveMessage,
        LoadScene,
        EnterShop,
        PayGold,
        AddGold
    }

    // 서버와 값 공유중, 함부로 수정 금지.
    public enum EMissionStatus
    {
        None,
        Progress,
        Complete,
        Rewarded
    }

    public enum EEquipType
    {
		None = 0,
		Hair,
		Eyebrows,
		Eyes
	}

    public enum EStat
    {
        MaxHp,
        MoveSpeed,
        Luck,
        MaxCount,
    }

    public enum EStatModifierKind
    {
        Item,
        Pet,
        Summon,
        Passive,
        Buff,
        Relic,
    }

    public enum EStatModifierType
    {
        Flat,
        Percentage,
    }

    public enum EColorMode
    {
        Rgb,
        Hsv
    }

    public enum EBehavior
    {
        Attacked,
        Boring,
        Item,
        Lucky
    }

    public enum EErrorCode
    {
        ERR_OK,
        ERR_NetworkSettlementErrorResend,
        ERR_NetworkSettlementError, 
        ERR_NetworkIDError, 
        ERR_NetworkLoginSuccess, 
        ERR_NetworkIDNotFound, 
        ERR_NetworkPasswordMismatch, 
        ERR_NetworkSaveError, 
        ERR_AccountCreationFailed, 
        ERR_ValidationId, 
        ERR_AccountPasswordRequirement, 
        ERR_AccountCreationSuccess, 
        ERR_AccountCreationCancellation, 
        ERR_ValidationNickname, 
        ERR_GoldInsufficient, 
        ERR_ValidationPassword, 
        ERR_ConfirmPassword, 
        ERR_Nothing,
        ERR_Logout,
        ERR_EnergyInsufficient,
        ERR_InvalidCredentials,
        ERR_GoogleAccountAlreadyExists,
        ERR_GoogleAccountMergeConfirm,
    }

    public enum EProductType
    {
        Evolution,
        Custom,
        None,
    }

    public enum ELanguage
    {
        Kr,
        En
    }

    public enum EMission
    {
        Level,
        Shop,
    }

    public enum EMissionType
    {
        Time,
        SurviveToLevel,
        AvoidRocksCount,
        // XXX : 더이상 사용하지 않는 TYPE
        // TODO : 삭제필요.
        AchieveScoreInGame, 
        Style,
        RecordScore,
        Evolution,
    }

    public enum EItemType
    {
        Boots,
        Armor,
        Mask
    }

    public class HardCoding
    {
        public static readonly Vector2 PlayerStartPos = new Vector2(0, -120);
        public static readonly Vector3 PlayerTeleportPos_Left = new Vector3(-70, -120, 0);
        public static readonly Vector3 PlayerTeleportPos_Right = new Vector3(70, -120, 0);
        public static readonly Vector3 ConfetiParticlePos = new Vector3(0, 0, -100);

        public static readonly int MAX_FAIL_COUNT = 1;

        public static readonly string PersonlSetting = "personalSetting";
        public static readonly string UserName = "UserName";
        public static readonly string Password = "Password";
        public static readonly string GoogleAccount = "GoogleAccount";

        public static readonly int ChangeStyleGold = 500;
        public static readonly int ContinueGameGold = 500;
    }

}
