using Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Define;
public class StoneController : ObjectBase
{
    private const float CORRECTION_VALUE = 2.9f;

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

        _rockImage = this.gameObject.GetOrAddComponent<SpriteRenderer>();

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
        Vector3 movement = Vector2.down * Data.Speed * Time.fixedDeltaTime * CORRECTION_VALUE;
        transform.Translate(movement);
    }

    public void SetInfo(EnemyData data)
    {
        Data = data;

        _rockImage.sprite = Managers.Resource.Load<Sprite>($"{Managers.Data.EnemyDic[Data.Id].SpriteName}.sprite");
    }

    public void HitGround()
    {
        // 일정 높이가 되면 다시 Push
        Physics.Raycast(transform.position, Vector3.down, out _hitInfo, 3f, LayerMask.GetMask("Ground"));
        if (_hitInfo.collider != null)
        {
            
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