using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class SkillLuck : ItemBase
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
        Managers.Event.TriggerEvent(EEventType.TakeItem, this);
        _player.SetLuckSkill(_luck);

        float afterValue = _luck * _changeValue;
        _player.SetLuckSkill(afterValue);
        Managers.Event.TriggerEvent(EEventType.TakeItem, this);
        yield return new WaitForSeconds(3);
        _player.SetLuckSkill(_luck);
        //Managers.Event.TriggerEvent(EEventType.SkillLuck_Player, this);
        Destroy(this.gameObject, 3);
    }

    // 방법이 항상 1개만 있는건아니고
    // 여러가지 방법이있는데
    // 제일중요한건
    // 하나의 방법으로 통일 시키는게 중요.
}
