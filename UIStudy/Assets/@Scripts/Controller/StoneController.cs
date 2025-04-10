using Data;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static Define;

public class StoneController : ObjectBase
{
    private bool _isNotStoneShower = true;
    public bool IsNotStoneShower
    {
        get => _isNotStoneShower;
        set
        {
            _isNotStoneShower = value;
        }
    }
    protected Rigidbody _rigidbody; // protected로 변경
    protected SpriteRenderer _rockImage; // protected로 변경
    protected RaycastHit _hitInfo; // protected로 변경

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

    private float shardSpeed = 5f;

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
        Vector3 movement = Vector2.down * _editSpeed * Time.fixedDeltaTime;

        _rigidbody.MovePosition(_rigidbody.position + movement);

        PushStone();
    }

    public void SetInfo(EnemyData data)
    {
        Data = data;
        _rockImage.sprite = Managers.Resource.Load<Sprite>($"{Managers.Data.EnemyDic[Data.Id].SpriteName}.sprite");
        SetLocalScale();
    }

    private void SetLocalScale()
    {
        transform.localScale = Vector3.one * 5;
    }

    #region Actor Interface
    // 돌 오브젝트풀링 
    private void PushStone()
    {
        if (Physics.Raycast(_rigidbody.position, Vector3.down, out _hitInfo, 25f, LayerMask.GetMask("Ground")))
        {
            // 스톤샤워가 아닐 경우에만 돌 개수 체크
            if (_isNotStoneShower == true)
            {
                // Managers.Game.DifficultySettingsInfo.StoneCount++;
                // Managers.Game.DifficultySettingsInfo.ChallengeScale++;
                // Managers.Game.GetScore.Total += 5;
                // Managers.Game.DifficultySettingsInfo.ChallengeScaleCount--;
                // Managers.Event.TriggerEvent(EEventType.UIStoneCountRefresh);

                // 랜덤한 확률로 실행
                SpawnStoneShards(); // 이게 실행되는 동안 돌이 계속 땅과 닿아있어서 push가 안되고 점수가 계속 올라감
            }
            Managers.Pool.Push(this.gameObject);
        }
    }

    // 플레이어와 충돌했을 때
    private void Attack(Collider collision)
    {
        if (collision.gameObject.GetComponent<PlayerController>() != null)
        {
            Managers.Game.GetScore.Total = ScorePenalty(Managers.Game.GetScore.Total);
            float playerLuck = collision.gameObject.GetComponent<PlayerController>().Data.Luck;
            float rand = Random.Range(0, 1.0f);
            //  플레이어가 가진 행운에 따라 무시
            if (rand <= playerLuck)
            {
                Managers.Event.TriggerEvent(EEventType.LuckyTrigger_Player, this);
            }
            // 아닐 경우 데미지 감소
            else
            {
                Managers.Event.TriggerEvent(EEventType.Attacked_Player, this, Data.Damage);
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

    private int ScorePenalty(int score)
    {
        float tempScore = score;
        tempScore -= 0.2f * score;
        return (int)tempScore;
    }
    private void SpawnStoneShards()
    {
        float angle = Random.Range(9f, 80f);
        for (int i = 0; i < 2; i++)
        {
            float radian = angle * Mathf.Deg2Rad;

            // X축 방향: 왼쪽 / 오른쪽
            float xDir = (i == 0) ? 1 : -1;
            Vector3 direction = new Vector3(xDir * Mathf.Cos(radian), Mathf.Sin(radian), 0f).normalized;

            GameObject shard = Managers.Object.Spawn<StoneShardController>(transform.position, true);

            float speed = 30f;
            Vector3 velocity = direction * speed;

            StoneShardController shardScript = shard.GetComponent<StoneShardController>();
            shardScript.SetInfo(Data, velocity);
        }
    }

    #endregion
}