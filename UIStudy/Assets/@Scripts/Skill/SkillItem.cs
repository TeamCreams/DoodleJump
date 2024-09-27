using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillItem : ObjectBase
{
    private StatModifier _statModifier;
    private float _skillSpeed = 2.0f;
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


    private void OnTriggerEnterPlayer(Collider collision)
    {
        if (collision.gameObject.GetComponent<PlayerController>() != null)
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
    }
}
