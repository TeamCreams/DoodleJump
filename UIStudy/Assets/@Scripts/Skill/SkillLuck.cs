using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class SkillLuck : SkillBase
{
    private PlayerController _player = null;
    private float _changeValue = 0;
    private float _luck = 0;

    public override bool Init()
    {
        if (base.Init() == false)
        {
            return false;
        }
        _player = transform.parent.gameObject.GetComponent<PlayerController>();
        _luck = _player.Data.Luck;

        return true;
    }

    public void SetLuckSkillEvent(float value)
    {
        _changeValue = value;
        StartCoroutine(ChangeLuckValue());
    }

    public void ResetLuckSkillEvent(float value)
    {
        _changeValue = value;
        StopCoroutine(ChangeLuckValue());
        StartCoroutine(ChangeLuckValue());
    }

    IEnumerator ChangeLuckValue()
    {
        _player.SetLuckSkill(_luck);

        float afterValue = _luck * _changeValue;
        _player.SetLuckSkill(afterValue);
        Managers.Event.TriggerEvent(EEventType.SkillLuck_Player, this);
        yield return new WaitForSeconds(3);
        _player.SetLuckSkill(_luck);
        Managers.Event.TriggerEvent(EEventType.SkillLuck_Player, this);
        Destroy(this.gameObject, 3);
    }

}
