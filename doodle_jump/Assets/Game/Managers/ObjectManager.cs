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
            Debug.LogError($"[{path}] 리소스가 없습니다");
            //Debug.Assert();
            return null;
       }

       var pool = origin.GetComponent<Poolable>();
       if (pool != null)
        {
            //var result = Managers.Instance.Pool.Pop(path); // object엔 transform 형식이 없다고 뜸
            GameObject result = (GameObject)Managers.Instance.Pool.Pop(path);
            result.transform.position = pos;
            result.transform.rotation = quaternion;
            _objects.Add(result);
            return result;
        }
        /* poolable은 이미 PoolManager에서 체크를 해서 생성을 하는데 아래의 코드와 Poolable이 필요한 이유를 모르겠습니다.
         else
        {
            var result = GameObject.Instantiate(origin, pos, quaternion);
            _objects.Add(result);
            return result;
        }
         */
    }

}