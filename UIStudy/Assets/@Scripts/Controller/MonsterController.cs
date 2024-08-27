using Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterController : MonoBehaviour
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
    [SerializeField]
    private float _speed = 0;

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
        _rigid = this.gameObject.GetComponent<Rigidbody2D>();
        _rigid.constraints = RigidbodyConstraints2D.FreezePositionX;
        _rigid.freezeRotation = true;
        _speed = Data.Speed;
        _id = Data.Id;
    }

    public void Moving()
    {
        // 떨어지는 속도 좀 더 빠르게
        _rigid.AddForce(Vector2.down * _speed * Time.deltaTime, ForceMode2D.Impulse);
        // 일정 높이가 되면 다시 Push
        if (this.transform.position.y < -130)
        {
            Managers.Pool.Push(this.gameObject);
        }
    }
}