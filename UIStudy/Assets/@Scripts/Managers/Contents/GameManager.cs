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

    private ChattingInfo _chattingInfo;
    public ChattingInfo ChattingInfo
    {
        get { return _chattingInfo; }
        set
        {
            _chattingInfo = value;
        }
    }

    private int _remainingChange;
    public int GoldTochange
    {
        get { return _remainingChange; }
        set
        {
            _remainingChange = value;
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
        _chattingInfo = new ChattingInfo();
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

    public int TempCharacterId { get; set; } = 20001;
    public string TempEyes { get; set; } = "Dizzy";
    public string TempEyebrows { get; set; } = "DizzyEyebrows";
    public string TempHair { get; set; } = "ZombieShabby";

    public int IsChangedStyle {get; set;} = 0;

    public bool CheckAppearance()
    {
        if (CharacterId != TempCharacterId)
        {            
            return true;
        }

        if (Eyes != TempEyes)
        {
            return true;
        }

        if (Eyebrows != TempEyebrows)
        {
            return true;
        }

        if (Hair != TempHair)
        {            
            return true;
        }
        return false;
    }

    public void UpdateValuesFromTemp()
    {
        CharacterId = TempCharacterId;
        Eyes = TempEyes;
        Eyebrows = TempEyebrows;
        Hair = TempHair;
    }

    public void ResetValuesFromTemp()
    {
        TempCharacterId = CharacterId;
        TempEyes = Eyes;
        TempEyebrows = Eyebrows;
        TempHair = Hair;
    }

}
public class UserInfo // 서버로 전달될 데이터
{
    public int UserAccountId { get; set; } = 0;
    public string UserName { get; set; } = "";
    public string Password {get; set;} = "0000";
    public string UserNickname {get; set;} = "";
    public string GoogleAccount {get; set;} = "";
    public int RecordScore {get; set;} = 0;
    public int LatelyScore {get; set;} = 0;
    public int Gold {get; set;} = 0;
    public int Level {get; set;} = 1;
    public int PlayTime {get; set;} = 0;
    public int ScoreBoard {get; set;} = 0;
    public int AccumulatedStone {get; set;} = 0;
    public int StageLevel {get; set;} = 0;
    public int EvolutionId {get; set;} = 140003;
    public int EvolutionSetLevel {get; set;} = 0;
    public int Energy { get; set;} = 10;
    public int PurchaseEnergyCountToday { get; set; } = 0;
    public DateTime LastRewardClaimTime {get; set;}
    public DateTime LatelyEnergy {get; set;}
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
    public int LatelyPlayTime {get; set;} = 0;
}

public class NoticeInfo
{
    public enum EType
    {
        Unknown,
        Debug,
        Info,
        Warning,
        Error,
        Critical
    }

    public string Title;
    public string Notice;
    public EType Type;

    public NoticeInfo(string title, string notice, EType type = EType.Unknown)
    {
        this.Title = title;
        this.Notice = notice;
        this.Type = type;
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
public struct ChattingStruct
{
    public bool IsPrivateMessage;
    public string Message;
    
    public ChattingStruct(bool isPrivateMessage, string message)
    {
        this.IsPrivateMessage = isPrivateMessage;
        this.Message = message;
    }
}

public class ChattingInfo
{
    public string SenderNickname { get; set; } = "";
    public Transform Root { get; set; }
}

public struct PurchaseStruct
{
    public int Id;
    public Define.EProductType ProductType;
    public Action OnOkay;
    public Action OnClose;
    
    public PurchaseStruct(int id, Define.EProductType productType, Action onOkay, Action onClose)
    {
        this.Id = id;
        this.ProductType = productType;
        this.OnOkay = onOkay;
        this.OnClose = onClose;
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