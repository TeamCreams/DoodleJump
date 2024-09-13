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

        OnTriggerEnter2D_Event -= OnTriggerEnterPlayer;
        OnTriggerEnter2D_Event += OnTriggerEnterPlayer;

        return true;
    }


    private void OnTriggerEnterPlayer(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerController>() == true)
        {
            Debug.Log("is Collision");
            var go = Managers.Resource.Instantiate("SkillItem", collision.gameObject.transform);
            go.GetOrAddComponent<SkillSpeed>().SetSpeedSkillEvent(2);
            //  이것도 pooling으로 만들어서 닿으면 수납
        }
    }
}
