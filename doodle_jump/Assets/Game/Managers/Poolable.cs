using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poolable : MonoBehaviour
{
    [SerializeField]
    private int _poolCount = 10;

    [SerializeField]
    private int _growCount = 10;

    public int PoolCount => _poolCount;//Get¿ë
    public int GrowCount => _growCount;
}
