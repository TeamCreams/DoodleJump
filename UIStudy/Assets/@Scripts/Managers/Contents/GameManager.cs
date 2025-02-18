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
    private float _life;
    public float Life
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
    public Action<float> OnChangedLife;

    private int _gold = 0;
    public int Gold
    {
        get { return _gold; }
        set
        {
            if (_gold != value)
            {
                _gold = value;
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
    private UserInfo _userInfo;
    public UserInfo UserInfo
    {
        get { return _userInfo; }
        set
        {
            _userInfo = value;
        }
    }
    private SettingInfo _settingInfo;
    public SettingInfo SettingInfo
    {
        get { return _settingInfo; }
        set
        {
            _settingInfo = value;
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
    private GetScore _getScore;
    public GetScore GetScore
    {
        get { return _getScore; }
        set
        {
            _getScore = value;
        }
    }

    private int _energy;
    public int Energy
    {
        get { return _energy; }
        set
        {
            _energy = value;
        }
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
        _userInfo = new UserInfo();
        _settingInfo = new SettingInfo();
        _getScore = new GetScore();
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
    public string Eyes { get; set; } = "Dizzy";
    public string Eyebrows { get; set; } = "DizzyEyebrows";
    public string Hair { get; set; } = "ZombieShabby";
}
public class UserInfo // 서버로 전달될 데이터
{
    public int UserAccountId { get; set; } = 0;
    public string UserName { get; set; } = "";
    public string Password {get; set;} = "0000";
    public string UserNickname {get; set;}
    public int RecordScore {get; set;} = 0;
    public int LatelyScore {get; set;} = 0;
    public int Gold {get; set;} = 0;
    public int Level {get; set;} = 1;
    public int PlayTime {get; set;} = 0;
    public int ScoreBoard {get; set;} = 0;
    public int AccumulatedStone {get; set;} = 0;
    public int EvolutionId {get; set;} = 140003;
}
public class SettingInfo
{
    public bool VibrationIsOn {get; set;} = true;
}
public class DifficultySettingsInfo // 다시시작할 때마다 초기화 필요 
{
    public int StageId { get; set; } = 70001;
    public int ChallengeScale { get; set; } = 0;
    public int ChallengeScaleCount { get; set; } = 0;
    public int StageLevel { get; set; } = 1;
    public float AddSpeed { get; set; } = 0;
    public int StoneCount {get; set;} = 0;
}
public class GetScore
{
    public int Total { get; set; } = 0;
    public int TimeScore { get; set; } = 0;
    public int StoneScore { get; set; } = 0;
}
public struct ErrorStruct
{
    public string Title;
    public string Notice;
    public ErrorStruct(string title, string notice)
    {
        this.Title = title;
        this.Notice = notice;
    }
}

public struct PersonalSetting
{
    public float MusicVolume;
    public float SoundFxVolume;
    public bool IsOnVibration;
    public bool IsOnKr;
    
    public PersonalSetting(float musicVolume, float soundFxVolume, bool isOnVibration, bool isOnKr)
    {
        this.MusicVolume = musicVolume;
        this.SoundFxVolume = soundFxVolume;
        this.IsOnVibration = isOnVibration;
        this.IsOnKr = isOnKr;
    }
    public string Serialize()
    {
        return MusicVolume + "," + SoundFxVolume + "," + IsOnVibration + "," + IsOnKr.ToString();
    }
    public static PersonalSetting Deserialize(string data)
    {
        string[] parts = data.Split(',');
        return new PersonalSetting(float.Parse(parts[0]), float.Parse(parts[1]), bool.Parse(parts[2]), bool.Parse(parts[3]));
    }
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