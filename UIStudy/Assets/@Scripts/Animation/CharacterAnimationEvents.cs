using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimationEvents : InitBase
{
    public override bool Init()
    {
        if (base.Init() == false)
        {
            return false;
        }
        return true;
    }


    public void SetSoundAudio(string soundAudio)
    {
        AudioClip audioClip = Managers.Resource.Load<AudioClip>($"{soundAudio}");
        Managers.Sound.Play(Define.ESound.Effect, audioClip);
    }
}
