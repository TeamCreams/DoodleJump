using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillItem : ObjectBase
{
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
            Debug.Log("is Collision");
            var go = Managers.Resource.Instantiate("AddSkill", collision.gameObject.transform);
            //  이것도 pooling으로 만들어서 닿으면 수납
            Managers.Pool.Push(this.gameObject);
        }
    }
}
