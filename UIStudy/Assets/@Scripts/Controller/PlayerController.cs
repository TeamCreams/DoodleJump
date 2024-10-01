using Data;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using static Define;
using static UnityEditor.Experimental.GraphView.GraphView;

public class PlayerController : CreatureBase
{
    private PlayerData _data;
    public PlayerData Data
    {
        get => _data;
        private set
        {
            _data = value;
        }
    }

    private float _waitTime = 0;
    [SerializeField]
    EPlayerState _state = EPlayerState.Idle;
    private Animator _animator;
    private CharacterController _characterController;

    private SpriteRenderer EyeSpriteRenderer;
    private SpriteRenderer EyebrowsSpriteRenderer;
    private SpriteRenderer HairSpriteRenderer;
    private SpriteRenderer ShoseLeftSpriteRenderer;
    private SpriteRenderer ShoseRightSpriteRenderer;
    private SpriteRenderer MaskSpriteRenderer;

    private List<StatModifier> _statModifier;

    public PlayerSettingData PlayerSettingData
    {
        get => _playerSettingData;
        set
        {
            _playerSettingData = value;
            CommitPlayerCustomization();
        }
    }
    private PlayerSettingData _playerSettingData;

    public EPlayerState State
    {
        get => _state;
        set
        {
            if (_state != value)
            {
                OnChangedState?.Invoke(_state, value);
                _state = value;
            }
        }
    }
    public Action<EPlayerState, EPlayerState> OnChangedState;

    public override bool Init()
    {
        if (false == base.Init())
        {
            return false;
        }

        Managers.Event.AddEvent(EEventType.Attacked_Player, OnEvent_DamagedHp);
        Managers.Event.AddEvent(EEventType.SkillSpeed_Player, OnEvent_SkillSpeed);
        Managers.Event.AddEvent(EEventType.TakeItem, OnEvent_TakeItem);

        return true;
    }

    private void OnDestroy()
    {
        Managers.Event.RemoveEvent(EEventType.Attacked_Player, OnEvent_DamagedHp);
    }

    void Update()
    {
        switch (_state)
        {
            case EPlayerState.Idle:
                Update_Idle();
                break;
            case EPlayerState.Move:
                Update_Move();
                break;
            case EPlayerState.Boring:
                Update_Boring();
                break;
        }
    }

    public override void SetInfo(int templateId)
    {
        base.SetInfo(templateId);
        _animator = GetComponentInChildren<Animator>();

        Data = Managers.Data.PlayerDic[templateId];
        this._stats = new Stats(Data);

        _characterController = GetComponent<CharacterController>();

        EyeSpriteRenderer = Util.FindChild<SpriteRenderer>(go: _animator.gameObject, name: "Eyes", recursive: true);
        EyebrowsSpriteRenderer = Util.FindChild<SpriteRenderer>(go: _animator.gameObject, name: "Eyebrows", recursive: true);
        HairSpriteRenderer = Util.FindChild<SpriteRenderer>(go: _animator.gameObject, name: "Hair", recursive: true);
        ShoseLeftSpriteRenderer = Util.FindChild<SpriteRenderer>(go: _animator.gameObject, name: "Shin[Armor][L]", recursive: true);
        ShoseRightSpriteRenderer = Util.FindChild<SpriteRenderer>(go: _animator.gameObject, name: "Shin[Armor][R]", recursive: true);
        MaskSpriteRenderer = Util.FindChild<SpriteRenderer>(go: _animator.gameObject, name: "Mask", recursive: true);

        ShoseLeftSpriteRenderer.sprite = null;
        ShoseRightSpriteRenderer.sprite = null;
        MaskSpriteRenderer.sprite = null;



        PlayerSettingData = LoadPlayerSettingData();
    }

    public void CommitPlayerCustomization()
    {
        HairSpriteRenderer.GetComponent<SpriteRenderer>().sprite = Managers.Resource.Load<Sprite>($"{_playerSettingData.Hair}.sprite"); // GameManagers 정보로     
        EyebrowsSpriteRenderer.GetComponent<SpriteRenderer>().sprite = Managers.Resource.Load<Sprite>($"{_playerSettingData.Eyebrows}.sprite");
        EyeSpriteRenderer.GetComponent<SpriteRenderer>().sprite = Managers.Resource.Load<Sprite>($"{_playerSettingData.Eyes}.sprite");
    }

    private PlayerSettingData LoadPlayerSettingData()
    {
        string json = PlayerPrefs.GetString("PlayerSettingData", null);

        if (!string.IsNullOrEmpty(json))
        {
            return JsonUtility.FromJson<PlayerSettingData>(json);
        }
        else
        {
            return null;
        }
    }

    private void Update_PositionX()
	{
        // 여기를 바꾸자
        if (this.transform.position.x < -310)
        {
            transform.position = Define.HardCoding.PlayerTeleportPos_Left;
        }
        if (310 < this.transform.position.x)
        {
            transform.position = Define.HardCoding.PlayerTeleportPos_Right;
        }
    }

    private void Update_Idle()
    {
        if (Managers.Game.JoystickState == EJoystickState.PointerDown)
        {
            _waitTime = 0;
            this.State = EPlayerState.Move;
        }
        _waitTime += Time.deltaTime;
        if (4 <= _waitTime)
        {
            this.State = EPlayerState.Boring;
        }
    }
    
    private void Update_Move()
    {
        _animator.SetBool("Boring", false);

        if (Managers.Game.JoystickState == Define.EJoystickState.PointerUp)
        {
            this.State = EPlayerState.Idle;
        }

        _animator.SetFloat("MoveSpeed", Mathf.Abs(Managers.Game.JoystickAmount.x));
        Vector2 motion = Vector2.right * (Managers.Game.JoystickAmount.x * this._stats.StatDic[EStat.MoveSpeed].Value * Time.deltaTime);
        _characterController.Move(motion);

        if (Managers.Game.JoystickAmount.x < 0)
        {
            this.transform.localScale
                 = new Vector3(-Mathf.Abs(this.transform.localScale.x), this.transform.localScale.y, this.transform.localScale.z);
        }
        else if (0 < Managers.Game.JoystickAmount.x)
        {
            this.transform.localScale
                    = new Vector3(Mathf.Abs(this.transform.localScale.x), this.transform.localScale.y, this.transform.localScale.z);
        }
    }

    private void OnEvent_DamagedHp(Component sender, object param)
    {
        this._stats.Hp -= 1f;
        Managers.Event.TriggerEvent(EEventType.ChangePlayerLife, this, this._stats.Hp);

        StartCoroutine(Update_CryingFace());
    }

    IEnumerator Update_CryingFace()
    {
        EyeSpriteRenderer.GetComponent<SpriteRenderer>().sprite = Managers.Resource.Load<Sprite>("Crying.sprite");
        yield return new WaitForSeconds(0.8f);
        EyeSpriteRenderer.GetComponent<SpriteRenderer>().sprite = Managers.Resource.Load<Sprite>($"{_playerSettingData.Eyes}.sprite");
    }

    private void Update_Boring()
    {
        if (Managers.Game.JoystickState == EJoystickState.PointerDown)
        {
            this.State = EPlayerState.Move;
        }
        _waitTime = 0;
        _animator.SetBool("Boring", true);
    }

    public void SetSpeedSkill(float speed)
    {
        Data.Speed = speed;
    }
    public void SetLuckSkill(float luck)
    {
        Data.Luck = luck;
    }

    public void OnEvent_SkillSpeed(Component sender, object param)
    {
        //ShoseLeftTransform.GetComponent<SpriteRenderer>().sprite = Managers.Resource.Load<Sprite>($"Shin.sprite");
        //ShoseRightTransform.GetComponent<SpriteRenderer>().sprite = Managers.Resource.Load<Sprite>($"Shin.sprite");
    }

    public void OnEvent_TakeItem(Component sender, object param)
    {
        ItemBase data = sender as ItemBase;
        _statModifier = data.ModifierList;
        // 아이템
        // 옵션을 어떻게 만들것이냐

        StartCoroutine(ChangeStat());
        // 스탯의 이상한 스탯을 추가
        // 스탯중에서 IsFireMode 라는 스탯을 추가해서 
    }


    IEnumerator ChangeStat()
    {
        Stat stat = new Stat(this._stats.Hp);
        stat.AddStatModifier(_statModifier[0]);
        yield return new WaitForSeconds(3);
        stat.RemoveStatModifier(_statModifier[0].Id);
        // 스피드면 신발, 가면 등등 cvs파일 만들고 join이런거 쓰는 것도 ㄱㅊ을 
    }
}
