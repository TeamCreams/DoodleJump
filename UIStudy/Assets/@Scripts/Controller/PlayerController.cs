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
    private SpriteRenderer _emotion = null;
    private float _speed = 0;
    private float _waitTime = 0;
    [SerializeField]
    EPlayerState _state = EPlayerState.Idle;
    private RaycastHit2D _hitStoneMonster;
    private Animator _animator;
    private CharacterController _characterController;

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

	private void OnDestroy()
    {
        Managers.Event.RemoveEvent(EEventType.Attacked_Player, OnEvent_DamagedHp);
    }

	void Update()
    {
        //Update_PositionX();
        CheckAttacked();
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

        _speed = Data.Speed;

        _emotion = _eyesGameobject.GetComponent<SpriteRenderer>();
        _characterController = GetComponent<CharacterController>();
        _characterController.isTrigger = true;
    }

    void SetState(EPlayerState prevState, EPlayerState currentState)
	{

	}

    public void CheckAttacked()
    {
        _hitStoneMonster
            = Physics2D.BoxCast(transform.position, new Vector2(1f, 1f), 0f, Vector2.up, 1f, LayerMask.GetMask("StoneMonster"));

/*
        _hitStoneMonster
            = Physics2D.Raycast(transform.position, Vector2.up, 0.5f, LayerMask.GetMask("StoneMonster"));
*/      
        if(_hitStoneMonster.collider != null)
        {
            Managers.Pool.Push(_hitStoneMonster.collider.gameObject);
            Managers.Event.TriggerEvent(EEventType.Attacked_Player); // 새로 시작할 때 오류 뜸.
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

    private void OnEvent_DamagedHp(Component sender, object param) // 굳이 이걸 event할 필요는 없다.
    {
        Data.Life -= 1;
        Managers.Game.Life = Data.Life;

        StartCoroutine(Update_CryingFace());
    }
     
    IEnumerator Update_CryingFace()
    {
        _emotion.sprite = Managers.Resource.Load<Sprite>("Crying.sprite");
        yield return new WaitForSeconds(0.8f);
        _emotion.sprite = Managers.Resource.Load<Sprite>("Sad.sprite");
    }
    
    private void Update_Move()
    {
        _animator.SetBool("Boring", false);

        if (Managers.Game.JoystickState == Define.EJoystickState.PointerUp)
        {
            this.State = EPlayerState.Idle;
            
        }
        //this.transform.Translate(Managers.Game.JoystickAmount.x * Data.Speed * Time.deltaTime, 0, 0);

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

    private void Update_Boring()
    {
        _waitTime = 0;

        if (Managers.Game.JoystickState == EJoystickState.PointerDown)
        {
            this.State = EPlayerState.Move;
        }
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

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if(hit.collider.gameObject.GetComponent<SkillItem>())
        {
            Debug.Log("-------------------------------------");
        }

    }
}
