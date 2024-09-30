using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class SkillSpeed : SkillBase
{
    private PlayerController _player = null;
    private float _changeValue = 0;
    private float _speed = 0;

    public override bool Init()
    {
        if (base.Init() == false)
        {
            return false;
        }
        _player = transform.parent.gameObject.GetComponent<PlayerController>();
        _speed = _player.Data.Speed;

        return true;
    }

    public void SetSpeedSkillEvent(float value)
    {
        _changeValue = value;
        StartCoroutine(ChangeSpeedValue());
    }

    public void ResetSpeedSkillEvent(float value)
    {
        _changeValue = value;
        StopCoroutine(ChangeSpeedValue());
        StartCoroutine(ChangeSpeedValue());
    }

    IEnumerator ChangeSpeedValue()
    {
        _player.SetSpeedSkill(_speed);

        float afterValue = _speed * _changeValue;
        _player.SetSpeedSkill(afterValue);
        Managers.Event.TriggerEvent(EEventType.SkillSpeed_Player, this);
        yield return new WaitForSeconds(3);
        _player.SetSpeedSkill(_speed);
        Managers.Event.TriggerEvent(EEventType.SkillSpeed_Player, this);
        Destroy(this.gameObject, 3);
    }


    // 스킬을 붙이면 스킬에 있는 값대로 본체의 값이 변경 그리고 시간이 지나면 다시 원복
    // 1. 아이템을 먹음
    // 2. 캐릭터에 해당 스킬 프리(컴포넌트가 붙어있)이 추가 됨 
    // 3. 스킬 프리펩이 할일을 함 
    // 4. 시간이 지나면 삭제됨 Destroy(this.gameObject) 

}
