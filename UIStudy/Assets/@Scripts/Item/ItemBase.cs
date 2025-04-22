using Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class ItemBase : ObjectBase
{
    private SuberunkerItemData _data;
    public SuberunkerItemData Data
    {
        get => _data;
        private set
        {
            _data = value;
        }
    }

    private List<StatModifier> _modifierList = new List<StatModifier>();
    public List<StatModifier> ModifierList
    {
        get => _modifierList;
        private set
        {
            _modifierList = value;
        }

    }

    private Animator _animator;


    public override bool Init()
    {
        if (base.Init() == false)
        {
            return false;
        }
        OnTriggerEnter_Event -= OnTriggerEnterPlayer;
        OnTriggerEnter_Event += OnTriggerEnterPlayer;
        _animator = GetComponent<Animator>();
        return true;
    }

    public override void SetInfo(int templateId)
    {
        // 아이템받아오기
        Data = Managers.Data.SuberunkerItemDic[templateId];
        SetModifierList();
        StartCoroutine(AutoPooling());
        // 1. modifierList 세팅하기
        // _modifierList
    }

    private void SetModifierList()
    {
        List<(EStat, EStatModifierType, float)> options = new List<(EStat, EStatModifierType, float)>
        {
            (Data.Option1, Data.Option1ModifierType, Data.Option1Param),
            (Data.Option2, Data.Option2ModifierType, Data.Option2Param),
            (Data.Option3, Data.Option3ModifierType, Data.Option3Param),
            (Data.Option4, Data.Option4ModifierType, Data.Option4Param)
        };

        foreach (var option in options)
        {
            if (option.Item1 != 0)
            {
                StatModifier temp = new StatModifier(EStatModifierKind.Buff, option.Item2, option.Item3);
                _modifierList.Add(temp);
            }
        }
    }

    private IEnumerator AutoPooling()
    {
        yield return new WaitForSeconds(8);
        _animator.SetTrigger("isDisappear");
        float duration = _animator.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(duration);
        Managers.Resource.Destroy(this.gameObject);// Managers.Pool.Push(this.gameObject);
    }

    private void OnTriggerEnterPlayer(Collider collision)
    {
        if (collision.gameObject.GetComponent<PlayerController>() != null)
        {
            Managers.Sound.Play(Define.ESound.Effect, "GetItemSound");

            Managers.Event.TriggerEvent(EEventType.TakeItem, this);
        
            Managers.Resource.Destroy(this.gameObject);//Managers.Pool.Push(this.gameObject);
        }
    }
}
