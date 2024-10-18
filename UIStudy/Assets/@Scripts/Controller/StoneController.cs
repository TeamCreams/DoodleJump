using Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Define;
public class StoneController : ObjectBase
{
    private const float CORRECTION_VALUE = 2.9f;

    private bool _isCount = true;
    public bool IsCount
    {
        get => _isCount;
        set
        {
            _isCount = value;
        }
    }
    Rigidbody _rigidbody;

    private float _editSpeed = 0;
    private EnemyData _data;
    public EnemyData Data
    {
        get => _data;
        private set
        {
            _data = value;
        }
    }

    private SpriteRenderer _rockImage;
    private RaycastHit _hitInfo;

    public override bool Init()
	{
		if (false == base.Init())
		{
            return false;
		}
        _rigidbody = this.GetComponent<Rigidbody>();
        _rockImage = this.gameObject.GetOrAddComponent<SpriteRenderer>();

        OnTriggerEnter_Event -= Attack;
        OnTriggerEnter_Event += Attack;

        return true;
	}

    private void FixedUpdate()
    {
        _editSpeed = Data.Speed + Managers.Game.DifficultySettingsInfo.AddSpeed;
        Vector3 movement = Vector2.down * _editSpeed * Time.fixedDeltaTime * CORRECTION_VALUE;

        _rigidbody.MovePosition(_rigidbody.position + movement);

        PushStone();
    }

    public void SetInfo(EnemyData data)
    {
        Data = data;

        _rockImage.sprite = Managers.Resource.Load<Sprite>($"{Managers.Data.EnemyDic[Data.Id].SpriteName}.sprite");
    }

    public void Teleport(Vector3 pos)
    {
        this.gameObject.SetActive(false);
        this.transform.position = pos;
        this.gameObject.SetActive(true);
    }

    public void PushStone()
    {
        if(Physics.Raycast(_rigidbody.position, Vector3.down, out _hitInfo, 25f, LayerMask.GetMask("Ground")))
        {
            if (_isCount == true)
            {
                Managers.Game.DifficultySettingsInfo.ChallengeScale++;
            }
            Managers.Pool.Push(this.gameObject);
        }
       
    }

    private void Attack(Collider collision)
    {
        if (collision.gameObject.GetComponent<PlayerController>() != null)
        {
            //  플레이어가 가진 행운에 따라 무시
            float playerLuck = collision.gameObject.GetComponent<PlayerController>().Data.Luck;
            float rand = Random.Range(0, 1.0f);
            if (rand <= playerLuck)
            {
                Managers.Event.TriggerEvent(EEventType.LuckyTrigger_Player, this);
            }
            else
            {
                Managers.Event.TriggerEvent(EEventType.Attacked_Player, this);
            }
        

            Managers.Pool.Push(this.gameObject);
        }
    }
}