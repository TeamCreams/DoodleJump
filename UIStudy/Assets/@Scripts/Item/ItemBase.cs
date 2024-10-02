using Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class ItemBase : InitBase
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

    public override bool Init()
    {
        if (base.Init() == false)
        {
            return false;
        }
        return true;
    }

    public virtual void SetInfo(int templateId)
    {
        // 아이템받아오기
        Data = Managers.Data.SuberunkerItemDic[templateId];
        SetModifierList();

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

    private void OnTriggerEnterPlayer(Collider collision)
    {
        if (collision.gameObject.GetComponent<PlayerController>() != null)
        {
            Managers.Event.TriggerEvent(EEventType.TakeItem);
            /*
            switch (Data.Option1)
            {
                case EStat.MoveSpeed:
                    SkillSpeedItem(collision, Data.Option1Param);
                    break;
                case EStat.Luck:
                    SkillLuckItem(collision, Data.Option1Param);
                    break;
                case EStat.MaxHp: 
                    break;
                default:
                    break;
            }

            switch (Data.Option2)
            {
                case EStat.MoveSpeed:
                    SkillSpeedItem(collision, Data.Option2Param);
                    break;
                case EStat.Luck:
                    SkillLuckItem(collision, Data.Option2Param);
                    break;
                case EStat.MaxHp:
                    break;
                default:
                    break;
            }

            switch (Data.Option3)
            {
                case EStat.MoveSpeed:
                    SkillSpeedItem(collision, Data.Option3Param);
                    break;
                case EStat.Luck:
                    SkillLuckItem(collision, Data.Option3Param);
                    break;
                case EStat.MaxHp:
                    break;
                default:
                    break;
            }
            */
            Managers.Pool.Push(this.gameObject);
        }
    }

    private void SkillSpeedItem(Collider collision, float param)
    {
        SkillSpeed skillSpeedComponent = collision.gameObject.GetComponentInChildren<SkillSpeed>();

        if (skillSpeedComponent != null)
        {
            skillSpeedComponent.ResetSpeedSkillEvent(param);
        }
        else
        {
            var go = Managers.Resource.Instantiate("AddSkill", collision.gameObject.transform);
            go.GetOrAddComponent<SkillSpeed>().SetSpeedSkillEvent(param);
        }
    }

    private void SkillLuckItem(Collider collision, float param)
    {
        SkillLuck skillLuckComponent = collision.gameObject.GetComponentInChildren<SkillLuck>();

        if (skillLuckComponent != null)
        {
            skillLuckComponent.ResetLuckSkillEvent(param);
        }
        else
        {
            var go = Managers.Resource.Instantiate("AddSkill", collision.gameObject.transform);
            go.GetOrAddComponent<SkillLuck>().SetLuckSkillEvent(param);
        }
    }

    private void SkillHpItem(Collider collision, float param)
    {
        
    }
}
