using Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonsterController : MonoBehaviour
{
    
    private const int CORRECTION_VALUE = 10;
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
    private BoxCollider2D _collider;
    [SerializeField]
    private float _speed = 0;
    private SpriteRenderer _image;
    [SerializeField]
    private int _id = 0;
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
        _rigid = this.gameObject.GetOrAddComponent<Rigidbody2D>();
        _collider = this.gameObject.GetOrAddComponent<BoxCollider2D>();
        _collider.size = new Vector2(1.5f, 2f);
        _image = this.gameObject.GetOrAddComponent<SpriteRenderer>();
        _image.sprite = Managers.Resource.Load<Sprite>("Sprite_Icon_Weapon_Stone_02.sprite");
        _rigid.constraints = RigidbodyConstraints2D.FreezePositionX;
        _collider.isTrigger = true;
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
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.GetComponent<PlayerController>() == true)
        {
            collision.gameObject.GetComponent<PlayerController>().Data.Life -= 1;
            //Debug.Log(other.gameObject.GetOrAddComponent<PlayerController>().Data.Life);
            Managers.Game.Life = collision.gameObject.GetComponent<PlayerController>().Data.Life;
            Managers.Pool.Push(this.gameObject);
        }
    }
}