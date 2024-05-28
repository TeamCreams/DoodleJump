using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PoolManager
{
    Dictionary<string, Queue<UnityEngine.Object>> _pools = new Dictionary<string, Queue<Object>>();

    public UnityEngine.Object Pop(string path)
    {
        if (false == _pools.ContainsKey(path))
        {
            Queue<UnityEngine.Object> objects = new Queue<UnityEngine.Object>();
            
            var resource = Managers.Instance.Resource.Load<UnityEngine.Object>(path);
            var poolable = resource.GetComponent<Poolable>();

            if (poolable != null) // �ش� ����� ť�� ���ٸ�
            {
                for(int i = 0;  i < poolable.PoolCount; i++)
                {
                    var instance = UnityEngine.Object.Instantiate(poolable);
                    instance.gameObject.SetActive(false); //instance.GameObject.SetActive(false);
                    objects.Enqueue(instance);
                }
            }
            _pools.Add(path, objects);
        }

        if (_pools[path].Count == 0) // �ش� ����� ť�� �ִٸ�
        {
            Queue<UnityEngine.Object> objects = _pools[path]; // �ش� ����� ť�� ������

            var resource = Managers.Instance.Resource.Load<UnityEngine.Object>(path);
            var poolable = resource.GetComponent<Poolable>();

            for (int i = 0; i < poolable.GrowCount; i++)
            {
                var instance = UnityEngine.Object.Instantiate(poolable);
                instance.gameObject.SetActive(false);
                objects.Enqueue(instance); // ť�� �߰�
            }
        }
        var result = _pools[path].Dequeue();
        result.GameObject().SetActive(true);//instance.GameObject.SetActive(false);
        return result;
    }

    public void Push(string path, UnityEngine.Object Object)
    {
        Object.GameObject().SetActive(false);
        _pools[path].Enqueue(Object);
    }
}