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
    public override void SetPool(ObjectPool<PlatformController> pool)
    {
        base._pool = pool;
    }
}
