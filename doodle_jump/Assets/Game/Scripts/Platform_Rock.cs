using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform_Rock : PlatformController
{
    void Start()
    {
        this.GetComponent<BoxCollider2D>().isTrigger = true;
    }
}
