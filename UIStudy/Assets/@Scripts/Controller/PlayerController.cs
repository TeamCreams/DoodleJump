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

    public GameObject _eyesGameobject = null;
    private Sprite _emotion = null;
    private float _speed = 0;
    [SerializeField]
    EPlayerState _state = EPlayerState.Idle;
    private RaycastHit2D _hitStoneMonster;
    private Animator _animator;

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

    public override bool Init()
	{
		if(false == base.Init())
		{
            return false;
		}

        Managers.Event.AddEvent(EEventType.Attacked_Player, OnEvent_DamagedHp);

        this.OnChangedState -= SetState;
        this.OnChangedState += SetState;

        return true;
    }

	void Update()
    {
        Update_Default();
        CheckAttacked();
        _animator.SetInteger("State", (int)this.State);
        switch (_state)
		{
            case EPlayerState.Idle:
                Update_Idle();
                break;
            case EPlayerState.Walk:
                Update_Walk();
                break;
            case EPlayerState.Run:
                Update_Run();
                break;
        }
    }

	public override void SetInfo(int templateId)
	{
		base.SetInfo(templateId);
        _animator = GetComponentInChildren<Animator>();
        Data = Managers.Data.PlayerDic[templateId];

        _speed = Data.Speed;

        _emotion = _eyesGameobject.GetComponent<Sprite>();

    }

    void SetState(EPlayerState prevState, EPlayerState currentState)
	{

	}

    public void CheckAttacked()
    {
        _hitStoneMonster 
            = Physics2D.Raycast(transform.position, Vector2.up, 0.5f, LayerMask.GetMask("StoneMonster"));
        if(_hitStoneMonster.collider != null)
        {
            Managers.Pool.Push(_hitStoneMonster.collider.gameObject);
            Managers.Event.TriggerEvent(EEventType.Attacked_Player); // 새로 시작할 때 오류 뜸.
        }
    }

    private void Update_Default()
	{

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
        if(Managers.Game.JoystickState == EJoystickState.PointerDown)
		{
            this.State = EPlayerState.Walk;
        }
	}

    private void OnEvent_DamagedHp(Component sender, object param) // 굳이 이걸 event할 필요는 없다.
    {
        Data.Life -= 1;
        Managers.Game.Life = Data.Life;

        StartCoroutine(UpdateFace());
    }

    IEnumerator UpdateFace()
    {
        Debug.Log("UpdateFace"); //뜨는데
        _emotion = Managers.Resource.Load<Sprite>("Crying.sprite"); // 안 됨
        yield return new WaitForSeconds(0.5f);
        _emotion = Managers.Resource.Load<Sprite>("Sad.sprite");
    }
    private void Update_Walk()
    {
        if (Managers.Game.JoystickState == Define.EJoystickState.PointerUp)
        {
            this.State = EPlayerState.Idle;
        }
        
        this.transform.Translate(Managers.Game.JoystickAmount.x * _speed * Time.deltaTime, 0, 0);
        
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
        
        if (0.5f <= Managers.Game.JoystickAmount.sqrMagnitude)
        {
            this.State = EPlayerState.Run;
        }
    }

    private void Update_Run() 
    {
        if (Managers.Game.JoystickState == Define.EJoystickState.PointerUp)
        {
            this.State = EPlayerState.Idle;
        }

        this.transform.Translate(Managers.Game.JoystickAmount.x * _speed * Time.deltaTime, 0, 0);

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

        if (Managers.Game.JoystickAmount.sqrMagnitude < 0.5f)
        {
            this.State = EPlayerState.Walk;
        }
    }
}
