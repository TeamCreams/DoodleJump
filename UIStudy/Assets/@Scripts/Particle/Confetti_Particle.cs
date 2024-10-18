using System.Collections;
using System.Collections.Generic;
using Data;
using UnityEngine;

public class Confetti_Particle : ObjectBase
{
    public override bool Init()
    {
        if (base.Init() == false)
        {
            return false;
        }

        return true;
    }
    public void StartParticle(Vector3 pos)
    {
        this.transform.position = pos;
        StartCoroutine(ConfettiParticle());
    }
    IEnumerator ConfettiParticle()
    {
        ParticleSystem particle = this.GetComponent<ParticleSystem>();
        if (particle == null)
        {
            yield break;
        }


        Managers.UI.ShowPopupUI<UI_LevelUpPopup>();


        // 파티클이 재생되는 동안 기다림
        while (particle.IsAlive())
        {
            particle.Simulate(Time.unscaledDeltaTime, true, false);
            yield return null; // 다음 프레임까지 대기
        }


        Managers.Resource.Destroy(this.gameObject);
    }

}
