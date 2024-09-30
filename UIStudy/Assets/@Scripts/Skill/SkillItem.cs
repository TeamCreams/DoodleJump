using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillItem : ObjectBase
{
    private StatModifier _statModifier;
    private float _skillSpeed = 1.6f;
    private float _skillLuck = 1.5f;
    private Skill _skillState = Skill.Speed;

    enum Skill
    {
        Speed,
        Luck,
        None
    }
    public override bool Init()
    {
        if (false == base.Init())
        {
            return false;
        }

        OnTriggerEnter_Event -= OnTriggerEnterPlayer;
        OnTriggerEnter_Event += OnTriggerEnterPlayer;

        return true;
    }

    public void SetSkillInfo(int id)
    {
        _skillState = (Skill)id;
    }

    private void OnTriggerEnterPlayer(Collider collision)
    {
        if (collision.gameObject.GetComponent<PlayerController>() != null)
        {
            switch(_skillState)
            {
                case Skill.Speed:
                    SkillSpeedItem(collision);
                    break;
                case Skill.Luck:
                    SkillLuckItem(collision);
                    break;
                case Skill.None:
                    break;

            }
            Managers.Pool.Push(this.gameObject);
        }
    }

    private void SkillSpeedItem(Collider collision)
    {
        SkillSpeed skillSpeedComponent = collision.gameObject.GetComponentInChildren<SkillSpeed>();

        if (skillSpeedComponent != null)
        {
            skillSpeedComponent.ResetSpeedSkillEvent(_skillSpeed);
        }
        else
        {
            var go = Managers.Resource.Instantiate("AddSkill", collision.gameObject.transform);
            go.GetOrAddComponent<SkillSpeed>().SetSpeedSkillEvent(_skillSpeed);
        }
        Managers.Pool.Push(this.gameObject);
    }

    private void SkillLuckItem(Collider collision)
    {
        SkillLuck skillLuckComponent = collision.gameObject.GetComponentInChildren<SkillLuck>();

        if (skillLuckComponent != null)
        {
            skillLuckComponent.ResetLuckSkillEvent(_skillLuck);
        }
        else
        {
            var go = Managers.Resource.Instantiate("AddSkill", collision.gameObject.transform);
            go.GetOrAddComponent<SkillLuck>().SetLuckSkillEvent(_skillLuck);
        }
        Managers.Pool.Push(this.gameObject);
    }
}
