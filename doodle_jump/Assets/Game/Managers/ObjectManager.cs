using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    private List<UnityEngine.Object> _objects = new List<UnityEngine.Object>();
    public UnityEngine.Object Instantiate<T>(string path, Vector3 pos, Quaternion quaternion)
        where T : UnityEngine.Object
    {
       var origin = Managers.Instance.Resource.Load<T>(path);
       if (origin == null )
       {
            Debug.LogError($"[{path}] ���ҽ��� �����ϴ�");
            //Debug.Assert();
            return null;
       }

       var pool = origin.GetComponent<Poolable>();
       if (pool != null)
        {
            //var result = Managers.Instance.Pool.Pop(path); // object�� transform ������ ���ٰ� ��
            GameObject result = (GameObject)Managers.Instance.Pool.Pop(path);
            result.transform.position = pos;
            result.transform.rotation = quaternion;
            _objects.Add(result);
            return result;
        }
        /* poolable�� �̹� PoolManager���� üũ�� �ؼ� ������ �ϴµ� �Ʒ��� �ڵ�� Poolable�� �ʿ��� ������ �𸣰ڽ��ϴ�.
         else
        {
            var result = GameObject.Instantiate(origin, pos, quaternion);
            _objects.Add(result);
            return result;
        }
         */
    }

}