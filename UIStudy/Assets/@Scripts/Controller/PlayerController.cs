using Data;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class PlayerController : ObjectBase
{
    private EntityData _data;
    public EntityData Data 
    {
        get => _data;
        private set
        {
            _data = value;
        }
    }

    private Rigidbody2D _rigid;
    private float _life = 0;
    private float _speed = 0;
    [SerializeField]
    EPlayerState _state = EPlayerState.Idle;
    public EPlayerState State
	{
        get => _state;
        set
		{
            if(_state != value)
            {
                OnChangedState?.Invoke(_state, value);
                _state = value;
            }
		}
	}
    public Action<EPlayerState, EPlayerState> OnChangedState;

    private SpriteRenderer _spriteRenderer = null;

    public override bool Init()
	{
		if(false == base.Init())
		{
            return false;
		}

        _rigid = this.gameObject.GetComponent<Rigidbody2D>();
        _rigid.constraints = RigidbodyConstraints2D.FreezePositionY;
        _rigid.freezeRotation = true;

        _spriteRenderer = this.GetComponent<SpriteRenderer>();

        Managers.Event.AddEvent(EEventType.Attacked_Player, DamagedHp);

        this.OnChangedState -= SetState;
        this.OnChangedState += SetState;

        return true;
    }

	void Update()
    {
        Update_Default();
        switch(_state)
		{
            case EPlayerState.Idle:
                Update_Idle();
                break;
            case EPlayerState.Move:
                Update_Move();
                break;
        }
    }

	public override void SetInfo(int templateId)
	{
		base.SetInfo(templateId);
        Data = Managers.Data.EntityDic[templateId];


        _life = Data.Life;
        _speed = Data.Speed;
    }

    void SetState(EPlayerState prevState, EPlayerState currentState)
	{

	}

    private void Update_Default()
	{

        if (this.transform.position.x < -310)
        {
            transform.position = new Vector2(305, this.transform.position.y);
        }
        if (310 < this.transform.position.x)
        {
            transform.position = new Vector2(-305, this.transform.position.y);
        }
    }

    private void Update_Idle()
	{
        if(Managers.Game.JoystickState == EJoystickState.PointerDown)
		{
            this.State = EPlayerState.Move;
		}
	}

    private void DamagedHp(Component sender, object param)
    {
        Data.Life -= 1;
        Managers.Game.Life = Data.Life;
    }

    private void Update_Move()
    {
        if(Managers.Game.JoystickState == Define.EJoystickState.PointerUp)
        {
            this.State = EPlayerState.Idle;
        }

        _rigid.AddForce(Managers.Game.JoystickAmount * _speed * Time.deltaTime, ForceMode2D.Impulse);
        if (Managers.Game.JoystickAmount.x < 0)
        {
            _spriteRenderer.flipX = true;
        }
        else if (0 < Managers.Game.JoystickAmount.x)
        {
            _spriteRenderer.flipX = false;
        }

    }
}
