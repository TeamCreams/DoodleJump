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
		Move,
		Idle,
	}

	public enum EEventType
	{
		Attacked_Player,
	}

	public class Constants
	{
		public const int PLAYER_ID = 1;
	}

	public class HardCoding
	{
		public static readonly Vector2 PlayerStartPos = new Vector2(0, -120);
	}

}
