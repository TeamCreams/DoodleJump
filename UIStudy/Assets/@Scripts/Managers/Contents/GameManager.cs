using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security;
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

    // SuberunkerScene Player Life
    private int _life;
    public int Life
    {
        get { return _life; }
        set
        {
            Debug.Log($"Life = {value}");
            if (_life != value)
            {
                _life = value;
                OnChangedLife?.Invoke(value);
            }
        }
    }
    public Action<int> OnChangedLife;

    private int _playTimeRecord = 0;
    public int PlayTimeRecord
    {
        get { return _playTimeRecord; }
        set
        {
            if (_playTimeRecord != value)
            {
                _playTimeRecord = value;
            }
        }
    }
    private int _playTime = 0;
    public int PlayTime
    {
        get { return _playTime; }
        set
        {
            if (_playTime != value)
            {
                _playTime = value;
            }
        }
    }
    private ChracterStyleInfo _chracterStyleInfo;
    public ChracterStyleInfo ChracterStyleInfo
    {
        get { return _chracterStyleInfo; }
        set
        {
            _chracterStyleInfo = value;
        }

    }

    private DifficultySettingsInfo _difficultySettingsInfo;
    public DifficultySettingsInfo DifficultySettingsInfo
    {
        get { return _difficultySettingsInfo; }
        set
        {
            _difficultySettingsInfo = value;
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

    private Vector2 _joystickAmount;
    public Vector2 JoystickAmount 
    {
        get { return _joystickAmount; }
        set
        {
            _joystickAmount = value;
            Joystickstate?.Invoke(value);
        }
    }

    private Define.ELanguage _eLanguageInfo = Define.ELanguage.Kr;
    public Define.ELanguage ELanguageInfo
    {
        get { return _eLanguageInfo; }
        set { _eLanguageInfo = value; }
    }

    #endregion

    #region Action
    public event Action<(Vector2, float)> OnMoveDirChanged;
	public event Action<Define.EJoystickState> OnJoystickStateChanged;
    public event Action<Vector2> Joystickstate;
    #endregion


    public void Init()
    {
        _chracterStyleInfo = new ChracterStyleInfo();
        _difficultySettingsInfo = new DifficultySettingsInfo();
    }
   

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

    //public OptionType Option1 { get; set; }
    //public float Option1Paramter1 { get; set; }
    //public OptionType Option2 { get; set; }
    //public float Option2Paramter1 { get; set; }

    //public ClothType ClothType { get; set; }
    //public string ClothPrefab { get; set; }
}

public class ChracterStyleInfo
{
    public int CharacterId { get; set; } = 20001;
    public string PlayerName { get; set; } = "player";
    public string Eyes { get; set; } = "Dizzy";
    public string Eyebrows { get; set; } = "DizzyEyebrows";
    public string Hair { get; set; } = "ZombieShabby";
}

public class DifficultySettingsInfo // 다시시작할 때마다 초기화 필요 
{
    public int StageId { get; set; } = 70001;
    public int ChallengeScale { get; set; } = 0;
    public int StoneShower { get; set; } = 0;
    public int StageLevel { get; set; } = 1;
    public float AddSpeed { get; set; } = 0;
}


[System.Serializable] //얘가 있어야 직렬화 가능
public class MessageData
{
    public string id; // get, set도 있으면 안 됨
    public string name;
    public string time; // 아직 안 쓸 듯.
    public string message;
}

[System.Serializable]
public class Messages
{
    public List<MessageData> Chatting;
}