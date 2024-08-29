using Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Define;
public class MonsterController : ObjectBase
{
    private const int CORRECTION_VALUE = 10;

    public delegate void CustomAction(Component sender = null, object param = null);


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
    private BoxCollider2D _collider;
    [SerializeField]
    private float _speed = 0;
    private SpriteRenderer _image;
    [SerializeField]
    private int _id = 0;

	public override bool Init()
	{
		if (false == base.Init())
		{
            return false;
		}

        _rigid = this.gameObject.GetOrAddComponent<Rigidbody2D>();
        _collider = this.gameObject.GetOrAddComponent<BoxCollider2D>();
        _collider.size = new Vector2(1.5f, 2f);
        _image = this.gameObject.GetOrAddComponent<SpriteRenderer>();
        _rigid.constraints = RigidbodyConstraints2D.FreezePositionX;
        _collider.isTrigger = true;

        OnTriggerEnter2D_Event -= Attack;
        OnTriggerEnter2D_Event += Attack;

        return true;
	}

	void Update()
    {
        Moving();
    }

    public void SetInfo(EntityData data)
    {
        Data = data;

        _image.sprite = Managers.Resource.Load<Sprite>("Sprite_Icon_Weapon_Stone_02.sprite");
        _speed = Data.Speed;
        _id = Data.Id;
    }


    public void Moving()
    {
        //Vector2 move = Vector2.down * _speed * CORRECTION_VALUE * Time.deltaTime;
        //_rigid.MovePosition(_rigid.position + move);
        _rigid.AddForce(Vector2.down * _speed * Time.deltaTime, ForceMode2D.Impulse);
        // 일정 높이가 되면 다시 Push
        if (this.transform.position.y < -130)
        {
            Managers.Pool.Push(this.gameObject);
        }
    }

    private void Attack(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerController>() == true)
        {
            Managers.Event.TriggerEvent(EEventType.Attacked_Player);
            Managers.Pool.Push(this.gameObject);
        }
    }
}