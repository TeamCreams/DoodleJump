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
        Vector3 movement = Vector2.down * _editSpeed * Time.fixedDeltaTime;

        _rigidbody.MovePosition(_rigidbody.position + movement);

        PushStone();
    }

    public void SetInfo(EnemyData data)
    {
        Data = data;
        _rockImage.sprite = Managers.Resource.Load<Sprite>($"{Managers.Data.EnemyDic[Data.Id].SpriteName}.sprite");
    }

    #region Actor Interface
    // 돌 오브젝트풀링 
    public void PushStone()
    {
        if (Physics.Raycast(_rigidbody.position, Vector3.down, out _hitInfo, 25f, LayerMask.GetMask("Ground")))
        {
            // 스톤샤워가 아닐 경우에만 돌 개수 체크
            if (_isNotStoneShower == true)
            {
                Managers.Game.DifficultySettingsInfo.StoneCount++;
                Managers.Game.DifficultySettingsInfo.ChallengeScale++;
                Managers.Game.GetScore.Total += 5;
                Managers.Game.DifficultySettingsInfo.ChallengeScaleCount--;
                Managers.Event.TriggerEvent(EEventType.UIStoneCountRefresh);
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

    public int ScorePenalty(int score)
    {
        float tempScore = score;
        tempScore -= 0.2f * score;
        return (int)tempScore;
    }
    #endregion
}