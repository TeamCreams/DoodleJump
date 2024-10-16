using Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.XR;
using static Define;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;
using static UnityEditor.Experimental.GraphView.GraphView;
using static UnityEditor.PlayerSettings;
using static UnityEditor.Progress;

public class PlayerController : CreatureBase
{
    private PlayerData _data;
    public PlayerData Data
    {
        get => _data;
        private set
        {
            _data = value;
        }
    }

    private SuberunkerItemData _itemData;
    public SuberunkerItemData ItemData
    {
        get => _itemData;
        private set
        {
            _itemData = value;
        }
    }

    private float _waitTime = 0;
    [SerializeField]
    EPlayerState _state = EPlayerState.Idle;
    private Animator _animator;
    private CharacterController _characterController;


    private SpriteRenderer EyeSpriteRenderer;
    private SpriteRenderer EyebrowsSpriteRenderer;
    private SpriteRenderer HairSpriteRenderer;
    private SpriteRenderer ShoseLeftSpriteRenderer;
    private SpriteRenderer ShoseRightSpriteRenderer;
    private SpriteRenderer MaskSpriteRenderer;

    private List<StatModifier> _statModifier;

   
    public EPlayerState State
    {
        get => _state;
        set
        {
            if (_state != value)
            {
                OnChangedState?.Invoke(_state, value);
                _state = value;
            }
        }
    }
    public Action<EPlayerState, EPlayerState> OnChangedState;

    public override bool Init()
    {
        if (false == base.Init())
        {
            return false;
        }

        Managers.Event.AddEvent(EEventType.Attacked_Player, OnEvent_DamagedHp);
        Managers.Event.AddEvent(EEventType.TakeItem, OnEvent_TakeItem);

        return true;
    }

    private void OnDestroy()
    {
        Managers.Event.RemoveEvent(EEventType.Attacked_Player, OnEvent_DamagedHp);
    }

    void Update()
    {
        switch (_state)
        {
            case EPlayerState.Idle:
                Update_Idle();
                break;
            case EPlayerState.Move:
                Update_Move();
                break;
            case EPlayerState.Boring:
                Update_Boring();
                break;
        }

        //Debug.Log($"count  : {Managers.Game.DifficultySettingsInfo.ChallengeScale}");
    }

    public override void SetInfo(int templateId)
    {
        base.SetInfo(templateId);
        _animator = GetComponentInChildren<Animator>();

        Data = Managers.Data.PlayerDic[templateId];
        this._stats = new Stats(Data); 

        _characterController = GetComponent<CharacterController>();
      
        EyeSpriteRenderer = Util.FindChild<SpriteRenderer>(go: _animator.gameObject, name: "Eyes", recursive: true);
        EyebrowsSpriteRenderer = Util.FindChild<SpriteRenderer>(go: _animator.gameObject, name: "Eyebrows", recursive: true);
        HairSpriteRenderer = Util.FindChild<SpriteRenderer>(go: _animator.gameObject, name: "Hair", recursive: true);
        ShoseLeftSpriteRenderer = Util.FindChild<SpriteRenderer>(go: _animator.gameObject, name: "Shin[Armor][L]", recursive: true);
        ShoseRightSpriteRenderer = Util.FindChild<SpriteRenderer>(go: _animator.gameObject, name: "Shin[Armor][R]", recursive: true);
        MaskSpriteRenderer = Util.FindChild<SpriteRenderer>(go: _animator.gameObject, name: "Mask", recursive: true);
       
        ShoseLeftSpriteRenderer.sprite = null;
        ShoseRightSpriteRenderer.sprite = null;
        MaskSpriteRenderer.sprite = null;

        CommitPlayerCustomization();
    }

    public void CommitPlayerCustomization()
    {
        HairSpriteRenderer.sprite = Managers.Resource.Load<Sprite>($"{Managers.Game.ChracterStyleInfo.Hair}.sprite"); // GameManagers 정보로     
        EyebrowsSpriteRenderer.sprite = Managers.Resource.Load<Sprite>($"{Managers.Game.ChracterStyleInfo.Eyebrows}.sprite");
        EyeSpriteRenderer.sprite = Managers.Resource.Load<Sprite>($"{Managers.Game.ChracterStyleInfo.Eyes}.sprite");
    }


    private void Update_Idle()
    {
        if (Managers.Game.JoystickState == EJoystickState.PointerDown)
        {
            _waitTime = 0;
            this.State = EPlayerState.Move;
        }
        _waitTime += Time.deltaTime;
        if (4 <= _waitTime)
        {
            this.State = EPlayerState.Boring;
            Managers.Event.TriggerEvent(EEventType.ThoughtBubble, this, EBehavior.Boring);
        }
    }
    
    private void Update_Move()
    {
        _animator.SetBool("Boring", false);

        if (Managers.Game.JoystickState == Define.EJoystickState.PointerUp)
        {
            this.State = EPlayerState.Idle;
        }

        _animator.SetFloat("MoveSpeed", Mathf.Abs(Managers.Game.JoystickAmount.x));
        Vector2 motion = Vector2.right * (Managers.Game.JoystickAmount.x * this._stats.StatDic[EStat.MoveSpeed].Value * Time.deltaTime);
        _characterController.Move(motion);

        Transform animationTransform = _animator.gameObject.transform;

        if (Managers.Game.JoystickAmount.x < 0)
        {
            animationTransform.localScale
                 = new Vector3(-Mathf.Abs(animationTransform.localScale.x), animationTransform.localScale.y, animationTransform.localScale.z);
        }
        else if (0 < Managers.Game.JoystickAmount.x)
        {
            _animator.gameObject.transform.localScale
                    = new Vector3(Mathf.Abs(animationTransform.localScale.x), animationTransform.localScale.y, animationTransform.localScale.z);
        }
    }

    private void OnEvent_DamagedHp(Component sender, object param)
    {
        this._stats.Hp -= 1f;
        Managers.Event.TriggerEvent(EEventType.ChangePlayerLife, this, this._stats.Hp);
        Managers.Event.TriggerEvent(EEventType.ThoughtBubble, this, EBehavior.Attack);
        AudioClip attackedAudio = Managers.Resource.Load<AudioClip>("AttackedSound");
        Managers.Sound.Play(ESound.Effect, attackedAudio, 0.7f);

        Managers.Game.DifficultySettingsInfo.ChallengeScale = 0;

        StartCoroutine(Update_CryingFace());
    }

    IEnumerator Update_CryingFace()
    {
        EyeSpriteRenderer.sprite = Managers.Resource.Load<Sprite>("Crying.sprite");
        yield return new WaitForSeconds(0.8f);
        EyeSpriteRenderer.sprite = Managers.Resource.Load<Sprite>($"{Managers.Game.ChracterStyleInfo.Eyes}.sprite");
    }

    private void Update_Boring()
    {
        if (Managers.Game.JoystickState == EJoystickState.PointerDown)
        {
            this.State = EPlayerState.Move;
        }
        _waitTime = 0;
        _animator.SetBool("Boring", true);
    }

    public void OnEvent_TakeItem(Component sender, object param)
    {
        ItemBase data = sender as ItemBase;
        Debug.Assert(data != null, "is null");
        _statModifier = data.ModifierList;
        ItemData = data.Data;
        Managers.Event.TriggerEvent(EEventType.ThoughtBubble, this, EBehavior.Item);


        StopCoroutine(ChangeStats());
        StartCoroutine(ChangeStats());
        // 스탯의 이상한 스탯을 추가
        // 스탯중에서 IsFireMode 라는 스탯을 추가해서 
    }


    IEnumerator ChangeStats()
    {
        List<EStat> options = new List<EStat>
        {
            ItemData.Option1,
            ItemData.Option2,
            ItemData.Option3,
            ItemData.Option4
        };

        foreach(var (option, statModifier) in options.Zip(_statModifier, (optionIndex, StatModifierIndex) => (optionIndex, StatModifierIndex)))
        {
            if (option != 0)
            {
                this._stats.StatDic[option].AddStatModifier(statModifier);
            }
        }
        MakeNullSprite();
        ChangeSprite(options);

        if (0 < ItemData.AddLife && this._stats.Hp <= 8)
        {
            this._stats.Hp += ItemData.AddLife;
            Managers.Event.TriggerEvent(EEventType.GetLife);
        }

        yield return new WaitForSeconds(ItemData.Duration);
        foreach (var (option, statModifier) in options.Zip(_statModifier, (optionIndex, StatModifierIndex) => (optionIndex, StatModifierIndex)))
        {
            if (option != 0)
            {
                this._stats.StatDic[option].RemoveStatModifier(statModifier.Id);
            }
        }
        MakeNullSprite();
    }

    public void ChangeSprite(List<EStat> options)
    {
        var groupInfo = from option in options
                        join sprite in Managers.Data.SuberunkerItemSpriteDic
                        on option equals sprite.Value.StatOption
                        select new
                        {
                            SpriteName = sprite.Value.Name,
                            EStatOption = option
                        };

        foreach (var group in groupInfo)
        {
            switch (group.EStatOption)
            {
                case EStat.MoveSpeed:
                    {
                        var sprite = Managers.Resource.Load<Sprite>($"{group.SpriteName}.sprite");
                        if (sprite != null)
                        {
                            ShoseLeftSpriteRenderer.sprite = sprite;
                            ShoseRightSpriteRenderer.sprite = sprite;
                        }
                    }
                    break;
                case EStat.Luck:
                    {
                        var sprite = Managers.Resource.Load<Sprite>($"{group.SpriteName}.sprite");
                        if (sprite != null)
                        {
                            MaskSpriteRenderer.sprite = sprite;
                        }
                    }
                    break;
                default:
                    break;
            }
        }
    }


    public void MakeNullSprite()
    {
        ShoseLeftSpriteRenderer.sprite = null;
        ShoseRightSpriteRenderer.sprite = null;
        MaskSpriteRenderer.sprite = null;
    }

    public void Teleport(Vector3 pos)
    {
        _characterController.enabled = false;
        _characterController.transform.position = pos;
        _characterController.enabled = true;
    }
}
