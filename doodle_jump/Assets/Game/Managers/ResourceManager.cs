using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager
    //���� ���� �� ���ҽ��� �� ���� �ҷ��ͼ� �����ϰ� ����
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
