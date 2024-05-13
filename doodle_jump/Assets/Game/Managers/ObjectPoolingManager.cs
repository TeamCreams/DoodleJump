using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ObjectPoolingManager : MonoBehaviour
{

    public void OnGetFromPool(GameObject poolingObject)
    {
        poolingObject.gameObject.SetActive(true);
    }

    public void OnReleaseToPool(GameObject poolingObject)
    {
        poolingObject.gameObject.SetActive(false);
    }

    public void OnDestroyPooledObject(GameObject poolingObject)
    {
        Destroy(poolingObject.gameObject);
    }

}
