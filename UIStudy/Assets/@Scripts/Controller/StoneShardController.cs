using System.Collections;
using Data;
using UnityEngine;
using static Define;

public class StoneShardController : ObjectBase
{
    private float _lifeTime = 2f;
    private EnemyData _data;
    private Rigidbody _rigidbody;

    public override bool Init()
    {
        if (base.Init() == false)
        {
            return false;
        }
        
        _rigidbody = this.gameObject.GetOrAddComponent<Rigidbody>();

        _rigidbody.useGravity = false;
        _rigidbody.isKinematic = false;
        OnTriggerEnter_Event -= Attack;
        OnTriggerEnter_Event += Attack;

        return true;
    }

    public void SetInfo(EnemyData data, Vector3 direction)
    {
        _data = data;

        // rigidbody 수정
        _rigidbody.linearVelocity = direction; // 한 방향으로 날아가기만 하면 돼서 fixed Update에 쓸 필요 없음
        _rigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        _rigidbody.interpolation = RigidbodyInterpolation.Interpolate;

        // 사이즈 조절
        transform.localScale = Vector3.one * 4;

        StartCoroutine(CallingPool());
    }

    private IEnumerator CallingPool()
    {
        yield return new WaitForSeconds(_lifeTime);
        Managers.Pool.Push(this.gameObject);
    }

    private void Attack(Collider collision)
    {
        // 플레이어에게 맞았을 때만 처리
        if (collision.gameObject.GetComponent<PlayerController>() != null)
        {
            //Managers.Game.GetScore.Total = ScorePenalty(Managers.Game.GetScore.Total);
            float playerLuck = collision.gameObject.GetComponent<PlayerController>().Data.Luck;
            float rand = Random.Range(0, 1.0f);
            _rigidbody.linearVelocity = Vector3.zero;// 이동 멈춤
            //  플레이어가 가진 행운에 따라 무시
            if (rand <= playerLuck)
            {
                Managers.Event.TriggerEvent(EEventType.LuckyTrigger_Player, this);
            }
            // 아닐 경우 데미지 감소
            else
            {
                Managers.Event.TriggerEvent(EEventType.Attacked_Player, this, _data.Damage);
            }
            Managers.Pool.Push(this.gameObject);
        }
    }
    public void Teleport(Vector3 pos)
    {
        this.gameObject.SetActive(false);
        this.transform.position = pos;
        this.gameObject.SetActive(true);
    }
}
