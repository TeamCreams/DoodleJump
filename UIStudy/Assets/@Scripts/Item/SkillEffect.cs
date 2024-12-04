using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class SkillEffect : InitBase
{
    private Animator _animator;
    public override bool Init()
        {
        if (base.Init() == false)
        {
            return false;
        }
        _animator = GetComponent<Animator>();
        Managers.Event.AddEvent(EEventType.LuckyTrigger_Player, OnEvent_LuckyTrigger);

        return true;
    }

    private void OnDestroy()
    {
        Managers.Event.RemoveEvent(EEventType.LuckyTrigger_Player, OnEvent_LuckyTrigger);
    }

    private void OnEvent_LuckyTrigger(Component sender, object param)
    {
        _animator.SetTrigger("isSkillEffect");

        //AudioClip luckSkillAudio = Managers.Resource.Load<AudioClip>("LuckSkillSound");
        Managers.Sound.Play(Define.ESound.Effect, "LuckSkillSound");
    }
}
