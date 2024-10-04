using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundAnimation : InitBase
{
    private Animator _animator;


    public override bool Init()
    {
        if (base.Init() == false)
        {
            return false;
        }

        _animator = GetComponentInChildren<Animator>();


        return true;
    }


    public void Update()
    {
        _animator.SetFloat("MoveDirection", Managers.Game.JoystickAmount.x);
    }

}
