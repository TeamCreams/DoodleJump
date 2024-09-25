using Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Define;
public class MonsterController : ObjectBase
{
    private const int CORRECTION_VALUE = 5;

    private EnemyData _data;
    public EnemyData Data
    {
        get => _data;
        private set
        {
            _data = value;
        }
    }

    private BoxCollider _collider;
    [SerializeField]
    private float _speed = 0;
    private SpriteRenderer _image;
    [SerializeField]
    private int _id = 0;
    private RaycastHit _hitInfo;

    public override bool Init()
	{
		if (false == base.Init())
		{
            return false;
		}

        _image = this.gameObject.GetOrAddComponent<SpriteRenderer>();

       OnTriggerEnter_Event -= Attack;
       OnTriggerEnter_Event += Attack;

        return true;
	}

	void Update()
    {
        HitGround();
    }
    private void FixedUpdate()
    {
        Vector3 movement = Vector2.down * _speed * Time.fixedDeltaTime;
        transform.Translate(movement);
    }

    public void SetInfo(EnemyData data)
    {
        Data = data;

        _image.sprite = Managers.Resource.Load<Sprite>("Sprite_Icon_Weapon_Stone_02.sprite");
        _speed = Data.Speed;
        _id = Data.Id;
    }

    public void HitGround()
    {
        // 일정 높이가 되면 다시 Push
        Physics.Raycast(transform.position, Vector3.down, out _hitInfo, 3f, LayerMask.GetMask("Ground"));
        if (_hitInfo.collider != null)
        {
            Debug.Log("is collider");
            Managers.Pool.Push(this.gameObject);
        }
    }


    private void Attack(Collider collision)
    {
        if (collision.gameObject.GetComponent<PlayerController>() != null)
        {
            //  플레이어가 가진 행운에 따라 무시.
            float playerLuck = collision.gameObject.GetComponent<PlayerController>().Data.Luck;
            float rand = Random.Range(0, 1.0f);
            if(rand <= playerLuck)
            {
                Managers.Event.TriggerEvent(EEventType.Attacked_Player, this);
            }
            Managers.Pool.Push(this.gameObject);
        }
    }
}