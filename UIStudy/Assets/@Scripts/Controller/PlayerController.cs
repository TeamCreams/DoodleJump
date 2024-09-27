using Data;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class PlayerController : ObjectBase
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

    private Transform EyeTransform;
    private Transform EyebrowsTransform;
    private Transform HairTransform;

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

        this.OnChangedState -= SetState;
        this.OnChangedState += SetState;

        return true;
    }

    private void OnDestroy()
    {
        Managers.Event.RemoveEvent(EEventType.Attacked_Player, OnEvent_DamagedHp);
    }

    void Update()
    {
        //Update_PositionX();
        //CheckAttacked();
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

        PlayerData originalData = Managers.Data.PlayerDic[templateId];
        Data = new PlayerData(originalData);

        _characterController = GetComponent<CharacterController>();

        EyeTransform = Util.FindChild(go: _animator.gameObject, name: "Eyes", recursive: true).transform;
        EyebrowsTransform = Util.FindChild(go: _animator.gameObject, name: "Eyebrows", recursive: true).transform;
        HairTransform = Util.FindChild(go: _animator.gameObject, name: "Hair", recursive: true).transform;

        _playerSettingData = LoadPlayerSettingData();
        CommitPlayerCustomization();
    }

    public void CommitPlayerCustomization()
    {
        HairTransform.GetComponent<SpriteRenderer>().sprite = Managers.Resource.Load<Sprite>($"{_playerSettingData.Hair}.sprite");
        EyebrowsTransform.GetComponent<SpriteRenderer>().sprite = Managers.Resource.Load<Sprite>($"{_playerSettingData.Eyebrows}.sprite");
        EyeTransform.GetComponent<SpriteRenderer>().sprite = Managers.Resource.Load<Sprite>($"{_playerSettingData.Eyes}.sprite");
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

    void SetState(EPlayerState prevState, EPlayerState currentState)
	{

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
        Vector2 motion = Vector2.right * (Managers.Game.JoystickAmount.x * Data.Speed * Time.deltaTime);
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
        Data.Life -= 1;
        Managers.Game.Life = Data.Life;

        StartCoroutine(Update_CryingFace());
    }

    IEnumerator Update_CryingFace()
    {
        EyeTransform.GetComponent<SpriteRenderer>().sprite = Managers.Resource.Load<Sprite>("Crying.sprite");
        yield return new WaitForSeconds(0.8f);
        EyeTransform.GetComponent<SpriteRenderer>().sprite = Managers.Resource.Load<Sprite>($"{_playerSettingData.Eyes}.sprite");
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

    public void SetSpeedSkill(float speed)//OnEvent로 하고 싶은데 parameter값 ??
    {
        Data.Speed *= speed;
    }
    public void SetLife(int life = 1)
    {
        this.Data.Life += life;
    }
}
