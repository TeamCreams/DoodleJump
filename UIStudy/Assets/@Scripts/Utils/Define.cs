using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Define
{
	public enum EScene
	{
		Unknown,
		DevLoadingScene,
		TinyFarmScene,
        SuberunkerScene,
        ChooseCharacterScene,
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
        SkillSpeed_Player,
        SkillLuck_Player,
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
        Atk,
        Def,
        MaxHp,
        Recovery,
        CritRate,
        AttackRange,
        AttackDelay,
        AttackDelayReduceRate,
        DodgeRate,
        SkillCooldownReduceRate,
        MoveSpeed,
        ElementAdvantageRate,
        GoldAmountAdvantageRate,
        ExpAmountAdvantageRate,
        BossAtkAdvantageRate,
        ActiveSKillAdvantageRate,
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


    public class Constants
	{
		public const int PLAYER_ID = 20001;
	}

	public class HardCoding
	{
		public static readonly Vector2 PlayerStartPos = new Vector2(0, -120);
        public static readonly Vector2 PlayerTeleportPos_Left = new Vector2(305, -120);
        public static readonly Vector2 PlayerTeleportPos_Right = new Vector2(-305, -120);
    }

}
