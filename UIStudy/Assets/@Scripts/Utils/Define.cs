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
}
