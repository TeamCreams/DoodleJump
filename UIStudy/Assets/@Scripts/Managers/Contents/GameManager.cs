using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager
{
	#region Hero
	//moveDir, amount
	private (Vector2, float) _moveDir;
	public (Vector2, float) MoveDir
	{
		get { return _moveDir; }
		set
		{
			_moveDir = value;
            OnMoveDirChanged?.Invoke(value);
		}
	}

	private Define.EJoystickState _joystickState;
	public Define.EJoystickState JoystickState
	{
		get { return _joystickState; }
		set
		{
			_joystickState = value;
			OnJoystickStateChanged?.Invoke(_joystickState);
		}
	}
	#endregion

	#region Action
	public event Action<(Vector2, float)> OnMoveDirChanged;
	public event Action<Define.EJoystickState> OnJoystickStateChanged;
    #endregion

    private List<ItemData> items = new List<ItemData>()
        {
            new ItemData() { Icon = "Shoese1", Rare = 1, Parts = 1, Level = 1 },
            new ItemData() { Icon = "Shoese2", Rare = 2, Parts = 2, Level = 2 },
            new ItemData() { Icon = "Gun1", Rare = 3, Parts = 3, Level = 3 },
            new ItemData() { Icon = "Gun2", Rare = 4, Parts = 4, Level = 4 },
            new ItemData() { Icon = "Armor1", Rare = 5, Parts = 5, Level = 5 },
            new ItemData() { Icon = "Armor2", Rare = 1, Parts = 1, Level = 1 },
            new ItemData() { Icon = "Helmet1", Rare = 2, Parts = 2, Level = 2 },
            new ItemData() { Icon = "Helmet2", Rare = 3, Parts = 3, Level = 3 },
            new ItemData() { Icon = "Shield1", Rare = 4, Parts = 4, Level = 4 },
            new ItemData() { Icon = "Shield2", Rare = 5, Parts = 5, Level = 5 },
            new ItemData() { Icon = "Shoese1", Rare = 1, Parts = 1, Level = 1 },
            new ItemData() { Icon = "Shoese2", Rare = 2, Parts = 2, Level = 2 },
            new ItemData() { Icon = "Gun1", Rare = 3, Parts = 3, Level = 3 },
            new ItemData() { Icon = "Gun2", Rare = 4, Parts = 4, Level = 4 },
            new ItemData() { Icon = "Armor1", Rare = 5, Parts = 5, Level = 5 },
            new ItemData() { Icon = "Armor2", Rare = 1, Parts = 1, Level = 1 },
            new ItemData() { Icon = "Helmet1", Rare = 2, Parts = 2, Level = 2 },
            new ItemData() { Icon = "Helmet2", Rare = 3, Parts = 3, Level = 3 },
            new ItemData() { Icon = "Shield1", Rare = 4, Parts = 4, Level = 4 },
            new ItemData() { Icon = "Shield2", Rare = 5, Parts = 5, Level = 5 },
            new ItemData() { Icon = "Shoese1", Rare = 1, Parts = 1, Level = 1 },
            new ItemData() { Icon = "Shoese2", Rare = 2, Parts = 2, Level = 2 },
            new ItemData() { Icon = "Gun1", Rare = 3, Parts = 3, Level = 3 },
            new ItemData() { Icon = "Gun2", Rare = 4, Parts = 4, Level = 4 },
            new ItemData() { Icon = "Armor1", Rare = 5, Parts = 5, Level = 5 },
            new ItemData() { Icon = "Armor2", Rare = 1, Parts = 1, Level = 1 },
            new ItemData() { Icon = "Helmet1", Rare = 2, Parts = 2, Level = 2 },
            new ItemData() { Icon = "Helmet2", Rare = 3, Parts = 3, Level = 3 },
            new ItemData() { Icon = "Shield1", Rare = 4, Parts = 4, Level = 4 },
            new ItemData() { Icon = "Shield2", Rare = 5, Parts = 5, Level = 5 }
        };
    public List<ItemData> Items
    {
        get { return items; }
        set {  items = value; }
    }
}

public class ItemData
{
	public string Icon { get; set; }
	public int Rare { get; set; }
	public int Parts { get; set; }
	public int Level { get; set; }
}
