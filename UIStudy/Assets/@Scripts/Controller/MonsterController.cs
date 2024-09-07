using Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Define;
public class MonsterController : ObjectBase
{
    private const int CORRECTION_VALUE = 5;

    private EntityData _data;
    public EntityData Data
    {
        get => _data;
        private set
        {
            _data = value;
        }
    }
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

        _collider = this.gameObject.GetOrAddComponent<BoxCollider2D>();
        _collider.size = new Vector2(2f, 1.45f);
        _image = this.gameObject.GetOrAddComponent<SpriteRenderer>();
        _collider.isTrigger = true;

       // OnTriggerEnter2D_Event -= Attack;
        //OnTriggerEnter2D_Event += Attack;

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
        transform.Translate(Vector2.down * _speed * CORRECTION_VALUE * Time.deltaTime);
        // 일정 높이가 되면 다시 Push
        if (this.transform.position.y < -130)
        {
            Managers.Pool.Push(this.gameObject);
        }
    }
    /*
    private void Attack(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerController>() == true)
        {
            Debug.Log("is Collision");
            Managers.Event.TriggerEvent(EEventType.Attacked_Player, this);
            Managers.Pool.Push(this.gameObject);
        }
    }
    */
}