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
    }

	public enum EUIEvent
	{
		Click,
		PointerDown,
		PointerUp,
		BeginDrag,
		Drag,
		EndDrag,
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
        ErrorPopup,
        ErrorButtonPopup,
        ToastPopupNotice,
        GetUserScoreList,
        GetMyScore,
        StartLoading,
        StopLoading,
        Mission,
        OnPlayerDead,
        OnSettlementComplete,
        OnFirstAccept,
        OnMissionComplete,
        OnUpdateMission,
        UIRefresh,
        Evolution,
        Purchase
    }

    // 서버와 값 공유중, 함부로 수정 금지.
    public enum EMissionStatus
    {
        None,
        Progress,
        Complete,
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
        ERR_Nothing
    }


    public enum EErrorCode2
    {
        ERR_OK,
        ERR_DuplicateNickname, // 중복된 닉네임입니다.
        ERR_ValidationNickname, // 닉네임 사용 Validaiotn 에러
        ERR_DuplicateId, // 중복된 Id입니다.
        ERR_ValidationId, // Id 사용 Validaiotn 에러
        ERR_ValidationPassword, // Password 사용 Validaiotn 에러
        ERR_ConfirmPassword,
        ERR_Nothing
    }


    public enum ELanguage
    {
        Kr,
        En
    }

    public class Constants
	{
		public const int PLAYER_ID = 20001;
	}

    public enum EScoreType
    {
        LatelyScore,
        RecordScore
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
        AchieveScoreInGame,
        Style,
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
    }

}
