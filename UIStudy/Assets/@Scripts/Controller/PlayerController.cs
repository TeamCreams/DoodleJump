using Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.XR;
using static Define;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

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

    public ItemBase Item { get; private set; }

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
    public HashSet<ItemBase> _haveItems = new HashSet<ItemBase>();

   
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

        _animator = GetComponentInChildren<Animator>();
        _characterController = GetComponent<CharacterController>();

        Managers.Event.AddEvent(EEventType.Attacked_Player, OnEvent_DamagedHp);
        Managers.Event.AddEvent(EEventType.TakeItem, OnEvent_TakeItem);
        Managers.Event.AddEvent(EEventType.OnPlayerRevive, OnEvent_Resurrect);

        return true;
    }

    private void OnDestroy()
    {
        Managers.Event.RemoveEvent(EEventType.Attacked_Player, OnEvent_DamagedHp);
        Managers.Event.RemoveEvent(EEventType.TakeItem, OnEvent_TakeItem);
        Managers.Event.RemoveEvent(EEventType.OnPlayerRevive, OnEvent_Resurrect);
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

        //1. 데이터 세팅 
        Data = Managers.Data.PlayerDic[templateId];
        this._stats = new Stats(Data); 

        //2. 캐릭터 외형 변경에 사용할 Renderer 불러오기
        EyeSpriteRenderer = Util.FindChild<SpriteRenderer>(go: _animator.gameObject, name: "Eyes", recursive: true);
        EyebrowsSpriteRenderer = Util.FindChild<SpriteRenderer>(go: _animator.gameObject, name: "Eyebrows", recursive: true);
        HairSpriteRenderer = Util.FindChild<SpriteRenderer>(go: _animator.gameObject, name: "Hair", recursive: true);
        ShoseLeftSpriteRenderer = Util.FindChild<SpriteRenderer>(go: _animator.gameObject, name: "Shin[Armor][L]", recursive: true);
        ShoseRightSpriteRenderer = Util.FindChild<SpriteRenderer>(go: _animator.gameObject, name: "Shin[Armor][R]", recursive: true);
        MaskSpriteRenderer = Util.FindChild<SpriteRenderer>(go: _animator.gameObject, name: "Mask", recursive: true);
       

        LoadPlayerCustomization();
        AddEvolutionStats();// 추가한 스탯만큼 변경
    }

    #region Update
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
            int doOrNot = UnityEngine.Random.Range(0, 10);
            if (doOrNot < 6)
            {
                Managers.Event.TriggerEvent(EEventType.ThoughtBubble, this, EBehavior.Boring);
            }
        }
    }
    
    private void Update_Move()
    {
        _animator.SetBool("Boring", false);

        if (Managers.Game.JoystickState == EJoystickState.PointerUp)
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

        if(this._characterController.transform.position.x <= HardCoding.PlayerTeleportPos_Left.x)
        {
            Teleport(HardCoding.PlayerTeleportPos_Left);
        }
        else if(HardCoding.PlayerTeleportPos_Right.x <= this._characterController.transform.position.x)
        {
            Teleport(HardCoding.PlayerTeleportPos_Right);
        }
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
            Managers.Event.TriggerEvent(EEventType.CancelThoughtBubble);
        }
        _waitTime = 0;
        _animator.SetBool("Boring", true);
    }
    #endregion

    #region OnEvent
    //데미지 처리 이벤트
    private void OnEvent_DamagedHp(Component sender, object param)
    {
        float damage = (float)param;

        // 2. 함수 호출 
        if(sender is StoneController)
        {
            // sender가 StoneController면 실행
            this.DamagedHp(damage);
        }
        else
        {
            this.DamagedHp(damage, true);
        }
    }

    //아이템 획득 이벤트
    public void OnEvent_TakeItem(Component sender, object param)
    {
        ItemBase data = sender as ItemBase;
        Debug.Assert(data != null, "is null");

        //1. 확률 계산후 말풍선 띄우기
        int doOrNot = UnityEngine.Random.Range(0, 10);
        if (doOrNot < 6)
        {
            Managers.Event.TriggerEvent(EEventType.ThoughtBubble, this, EBehavior.Item);
        }

        //2. 아이템 착용
        EquipItem(data);
    }
    #endregion

    #region Actor Interface
    public void OnEvent_Resurrect(Component sender, object param)
    {
        float maxHp = Managers.Object.Player.Stats.StatDic[EStat.MaxHp].Value;
        this._stats.Hp = maxHp;
        Debug.Log($"HP : {this._stats.Hp}");
    }

    public void DamagedHp(float damage, bool isStoneShard = false)
    {
        this._stats.Hp -= damage;
        Debug.Log($"HP : {this._stats.Hp}");
        Managers.Event.TriggerEvent(EEventType.ChangePlayerLife, this, this._stats.Hp);
        int doOrNot = UnityEngine.Random.Range(0, 10);
        if (doOrNot < 6)
        {
            Managers.Event.TriggerEvent(EEventType.ThoughtBubble, this, EBehavior.Attacked);
        }
        Managers.Sound.Play(ESound.Effect, "AttackedSound", 0.7f);

        if (isStoneShard == false)
        {
            Managers.Game.DifficultySettingsInfo.ChallengeScale = 0;
            Managers.Game.DifficultySettingsInfo.ChallengeScaleCount = Managers.Data.DifficultySettingsDic[Managers.Game.DifficultySettingsInfo.StageId].ChallengeScale;
            Managers.Event.TriggerEvent(EEventType.UIStoneCountRefresh);
        }
        StartCoroutine(Update_CryingFace());
    }

    // 캐릭터 커스텀마이징 정보 불러오기
    public void LoadPlayerCustomization()
    {
        // 1. 아이템 장착해제
        MakeNullSprite();

        // 2. 커스터마이징 정보로 외형 불러오기
        HairSpriteRenderer.sprite = Managers.Resource.Load<Sprite>($"{Managers.Game.ChracterStyleInfo.Hair}.sprite"); // GameManagers 정보로     
        EyebrowsSpriteRenderer.sprite = Managers.Resource.Load<Sprite>($"{Managers.Game.ChracterStyleInfo.Eyebrows}.sprite");
        EyeSpriteRenderer.sprite = Managers.Resource.Load<Sprite>($"{Managers.Game.ChracterStyleInfo.Eyes}.sprite");
    }

    //장비 장착 (EquipItem)
    public void EquipItem(ItemBase item)
    {
        //------------------------------------
        // 0. Validation Check
        //------------------------------------
        if(Item != null)
        {
            return;
        }

        //------------------------------------
        // 1. 변수 선처리
        //------------------------------------
        Item = item;
        _haveItems.Add(Item);

        //------------------------------------
        // 2. 이미지 적용
        //------------------------------------
        SuberunkerItemData itemData = Managers.Data.SuberunkerItemDic[item.Data.Id];
        List<EStat> options = new List<EStat>
        {
            itemData.Option1,       // MaxHp(per
            itemData.Option2,       // MaxHp (Fl
            itemData.Option3,       
            itemData.Option4        // 
        };

        // TblUser
        // 1, "백수영"
        // 2, "박서윤"

        // TblItem
        // 1, "검"
        // 2, "방패"

        // TblItemMapping
        // 1, 1
        // 2, 1
        // 1, 2

        // TblItemMapping 이테이블을 기준으로
        //   User를 모두 검색해라

        // "백수영"
        // "박서윤"



        int id = Managers.Game.UserInfo.EvolutionId;

        var query = Managers.Data.EvolutionDataDic
                        .Where(data => data.Key <= id)
                        .OrderByDescending(data => data.Key)
                        .Take(3)
                        .ToList();
        query = query.Where(item => options.Contains(item.Value.StatOption)).ToList();


        foreach (var group in query)
        {
            switch (group.Value.StatOption)
            {
                case EStat.MoveSpeed:
                    {
                        var sprite = Managers.Resource.Load<Sprite>($"{group.Value.ItemSprite}.sprite");
                        Debug.Log($"Equip : {sprite.name},{group.Value.ItemSprite}");
                        if (sprite != null)
                        {
                            ShoseLeftSpriteRenderer.sprite = sprite;
                            ShoseRightSpriteRenderer.sprite = sprite;
                        }
                    }
                    break;
                case EStat.Luck:
                    {
                        var sprite = Managers.Resource.Load<Sprite>($"{group.Value.ItemSprite}.sprite");
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

        //------------------------------------
        // 3. 스탯 변경
        //------------------------------------

        // 1) HP를 제외한 기본스탯 변경
        _statModifier = item.ModifierList;
        foreach (var (option, statModifier) in options.Zip(_statModifier, (optionIndex, StatModifierIndex) => (optionIndex, StatModifierIndex)))
        {
            if (option != 0)
            {
                this._stats.StatDic[option].AddStatModifier(statModifier);
            }
        }

        // 2) HP 변경
        if (0 < Item.Data.AddHp && this._stats.Hp <= Data.Hp)
        {
            this._stats.Hp += Item.Data.AddHp;
            Managers.Event.TriggerEvent(Define.EEventType.ChangePlayerLife, this, this._stats.Hp);
        }

        //------------------------------------
        // 4. 골드 변경
        //------------------------------------
        Managers.Game.Gold += Item.Data.Gold;
        Managers.Event.TriggerEvent(EEventType.GetGold);

        //------------------------------------
        // 5. UnEquip 예약
        //------------------------------------
        StartCoroutine(WaitUnEquipItemCo(Item.Data.Duration, Item));
    }

    //이미지 변경
    public void XXX_ChangeSprite(List<EStat> options)
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

    //아이템에 따른 스탯 변경
    IEnumerator XXX_ChangeStats(ItemBase data)
    {
        ItemData = data.Data;
        _statModifier = data.ModifierList;
        List<EStat> options = new List<EStat>
        {
            ItemData.Option1,
            ItemData.Option2,
            ItemData.Option3,
            ItemData.Option4
        };

        foreach (var (option, statModifier) in options.Zip(_statModifier, (optionIndex, StatModifierIndex) => (optionIndex, StatModifierIndex)))
        {
            if (option != 0)
            {
                this._stats.StatDic[option].AddStatModifier(statModifier);
            }
        }
        MakeNullSprite();
        XXX_ChangeSprite(options);

        if (0 < ItemData.AddHp && this._stats.Hp <= Data.Hp)
        {
            this._stats.Hp += ItemData.AddHp;
            Managers.Event.TriggerEvent(Define.EEventType.ChangePlayerLife, this, this._stats.Hp);
        }

        Managers.Game.Gold += ItemData.Gold;

        yield return new WaitForSeconds(ItemData.Duration);
        foreach (var (option, statModifier) in options.Zip(_statModifier, (optionIndex, StatModifierIndex) => (optionIndex, StatModifierIndex)))
        {
            if (option != 0)
            {
                this._stats.StatDic[option].RemoveStatModifier(statModifier.Id);
            }
        }
        MakeNullSprite();
        ItemData = null;
    }

    //장비 해제 (UnEquipItem)
    public void UnEquipItem(ItemBase item)
    {
        //------------------------------------
        // 0. Validation Check
        //------------------------------------
        if(false == _haveItems.Contains(item))
        {
            return;
        }
        if (Item != item)
        {
            return;
        }


        //------------------------------------
        // 1. 이미지 변경
        //------------------------------------
        MakeNullSprite();

        //------------------------------------
        // 2. 스탯 변경
        //------------------------------------
        List<EStat> options = new List<EStat>
        {
            item.Data.Option1,
            item.Data.Option2,
            item.Data.Option3,
            item.Data.Option4
        };

        foreach (var (option, statModifier) in options.Zip(_statModifier, (optionIndex, StatModifierIndex) => (optionIndex, StatModifierIndex)))
        {
            if (option != 0)
            {
                this._stats.StatDic[option].RemoveStatModifier(statModifier.Id);
            }
        }


        //------------------------------------
        // 3. 변수 후처리
        //------------------------------------
        _haveItems.Remove(Item);
        ItemData = null;
        Item = null;
    }
    public void AddEvolutionStats()
    {
        // 능력치 추가 구매를 통해 늘어난 것이므로 다시 remove를 할 필요가 없음
        Managers.Evolution.EvolutionDict();
        Managers.Evolution.SetModifierList();
        List<EStat> options = new List<EStat>
        {
            EStat.MoveSpeed,
            EStat.MaxHp,
            EStat.Luck
        };

        foreach (var (option, statModifier) in options.Zip(Managers.Evolution.ModifierList, (optionIndex, StatModifierIndex) => (optionIndex, StatModifierIndex)))
        {
            this._stats.StatDic[option].AddStatModifier(statModifier);
        }
    }

    //텔레포트
    public void Teleport(Vector3 pos)
    {
        _characterController.enabled = false;
        _characterController.transform.position = pos;
        _characterController.enabled = true;
    }
    #endregion

    #region Private Functions
    private void MakeNullSprite()
    {
        ShoseLeftSpriteRenderer.sprite = null;
        ShoseRightSpriteRenderer.sprite = null;
        MaskSpriteRenderer.sprite = null;
    }

    private IEnumerator WaitUnEquipItemCo(float duration, ItemBase item)
    {
        yield return new WaitForSeconds(duration);
        UnEquipItem(item);

        yield return null;
    }
    #endregion

    public override string ToString()
    {
        StringBuilder str = new StringBuilder();
        str.Append($"stat : {_stats.StatDic[EStat.MoveSpeed].Value}");
        str.Append($"stat : {_stats.StatDic[EStat.Luck].Value}");
        if (ItemData != null)
        {
            str.Append($"ItemData.Id : {ItemData.Id}");
        }
        return str.ToString();
    }
}
