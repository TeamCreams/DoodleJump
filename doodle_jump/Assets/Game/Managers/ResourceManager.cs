using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager
    //최초 실행 시 리소스를 한 번만 불러와서 저장하고 있음
{
    Dictionary<string, UnityEngine.Object> _resources = new Dictionary<string, UnityEngine.Object>();
    public UnityEngine.Object Load<T>(string path)
    {
        if (false == _resources.ContainsKey(path))
        {
            var currentResource = Resources.Load<T>(path);
            _resources.Add(path, currentResource);
        }
        return _resources[path];
    }
}
