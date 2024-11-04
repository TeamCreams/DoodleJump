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
		TinyFarmScene,
        SignUpScene,
        SignInScene,
        SuberunkerSceneHomeScene,
        SuberunkerTimelineScene,
        ChooseCharacterScene,
        InputNameScene,
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
        StoneShower,
        SetLanguage,
        SignIn,
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
        ERR_DuplicateNickname, // 중복된 닉네임입니다.
        ERR_ValidationNickname, // 닉네임 사용 Validaiotn 에러
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

	public class HardCoding
	{
		public static readonly Vector2 PlayerStartPos = new Vector2(0, -120);
        public static readonly Vector3 PlayerTeleportPos_Left = new Vector3(-70, -120, 0);
        public static readonly Vector3 PlayerTeleportPos_Right = new Vector3(70, -120, 0);
        public static readonly Vector3 ConfetiParticlePos = new Vector3(0, 0, -100);
    }

}
