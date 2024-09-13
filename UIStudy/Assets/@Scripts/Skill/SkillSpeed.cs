using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillSpeed : SkillBase
{
    private PlayerController _player = null;

    public override bool Init()
    {
        if (base.Init() == false)
        {
            return false;
        }
        return true;
    }

    public void FindPlayer()
    {
        _player = transform.parent.gameObject.GetComponent<PlayerController>();        
    }

    void AdjustSpeed()
    {
        _player.Data.Speed *= 2;
        // 플레이어에 함수 제작.
        // 초를 넣어줌.
        // 몇초가 지나면 다시 플레이어 상태 원복.
    }

}
