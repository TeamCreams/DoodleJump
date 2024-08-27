using Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private EntityData _data;
    public EntityData Data 
    {
        get => _data;
        private set
        {
            _data = value;
            MyAwake();
        }
    }

    private Rigidbody2D _rigid;
    private float _hp = 0;
    private float _speed = 0;

    void Update()
    {
        Moving();
    }

    public void SetInfo(EntityData data) 
    { 
        Data = data;
    }

    public void MyAwake()
    {
        _rigid = this.gameObject.GetComponent<Rigidbody2D>();
        _rigid.constraints = RigidbodyConstraints2D.FreezePositionY;
        _rigid.freezeRotation = true;

        _hp = Data.Hp;
        _speed = Data.Speed;
    }

    public void Moving()
    {
        _rigid.AddForce(Managers.Game.Amount * _speed * Time.deltaTime, ForceMode2D.Impulse);
        if (Managers.Game.Amount.x < 0)
        {
            GetComponent<SpriteRenderer>().flipX = true;
        }
        else if (0 < Managers.Game.Amount.x)
        {
            GetComponent<SpriteRenderer>().flipX = false;
        }

        if (this.transform.position.x < -310)
        {
            transform.position = new Vector2(305, this.transform.position.y);
        }
        if (310 < this.transform.position.x)
        {
            transform.position = new Vector2(-305, this.transform.position.y);
        }
    }
}
