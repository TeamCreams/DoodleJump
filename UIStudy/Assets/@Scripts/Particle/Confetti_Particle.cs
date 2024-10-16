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

        StartCoroutine(ConfettiParticle());
        return true;
    }

    IEnumerator ConfettiParticle()
    {
        this.GetComponent<ParticleSystem>().Play();
        yield return new WaitForSeconds(this.GetComponent<ParticleSystem>().main.duration);
        Time.timeScale = 0;
        Managers.UI.ShowPopupUI<UI_LevelUpPopup>();
        Managers.Resource.Destroy(this.gameObject);
    }
}
