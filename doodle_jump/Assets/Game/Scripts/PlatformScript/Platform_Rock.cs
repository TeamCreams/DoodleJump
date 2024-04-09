using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Platform_Rock : PlatformController
{
    protected override void Start()
    {
        base.Start();
    }
    public override void SetPool(ObjectPool<PlatformController> pool)//Platform_Rock가 아니어도 되나?
    {
        base._pool = pool;
    }

    protected override void OnEnable()
    {
        base.OnEnable();
    }
}
