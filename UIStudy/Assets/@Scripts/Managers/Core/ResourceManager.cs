using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Object = UnityEngine.Object;

public class ResourceManager
{
	private Dictionary<string, UnityEngine.Object> _resources = new Dictionary<string, UnityEngine.Object>();
	private Dictionary<string, AsyncOperationHandle> _handles = new Dictionary<string, AsyncOperationHandle>();

	private HashSet<string> _loadKeys = new HashSet<string>();

	#region Load Resource
	public T Load<T>(string key) where T : Object
	{
		if (_resources.TryGetValue(key, out Object resource))
			return resource as T;

		return null;
	}

	public GameObject Instantiate(string key, Transform parent = null, bool pooling = false)
	{
		GameObject prefab = Load<GameObject>(key);
		if (prefab == null)
		{
			Debug.LogError($"Failed to load prefab : {key}");
			return null;
		}

		GameObject go = null;
		if (pooling)
		{
			go = Managers.Pool.Pop(prefab);
			if(parent != null)
            {
				go.transform.SetParent(parent, false);
			}
		}
		else
		{
			go = Object.Instantiate(prefab, parent);
		}

		go.name = prefab.name;

		return go;
	}

	public void Destroy(GameObject go)
	{
		if (go == null)
			return;

		if (Managers.Pool.Push(go))
			return;

		Object.Destroy(go);
	}
    #endregion

    #region Addressable
    private void LoadAsync<T>(string key, Action<T> callback = null) where T : UnityEngine.Object
    {
        // Cache
        if (_resources.TryGetValue(key, out Object resource))
        {
            callback?.Invoke(resource as T);
            return;
        }


        string loadKey = key;
        if (key.Contains(".sprite"))
            loadKey = $"{key}[{key.Replace(".sprite", "")}]";


        // Cache
        if (_resources.TryGetValue(loadKey, out resource))
        {
            callback?.Invoke(resource as T);
            return;
        }


        if (_loadKeys.Contains(loadKey))
        {
            Debug.Log($"warning Load : {loadKey}");
            callback?.Invoke(null);
            return;
        }

        _loadKeys.Add(loadKey);

        var asyncOperation = Addressables.LoadAssetAsync<T>(loadKey);
        asyncOperation.Completed += (op) =>
        {
            _resources.Add(key, op.Result);
            _handles.Add(key, asyncOperation);
            callback?.Invoke(op.Result);
        };
    }

    public void LoadAllAsync<T>(string label, Action<string, int, int> callback) where T : UnityEngine.Object
    {
        var opHandle = Addressables.LoadResourceLocationsAsync(label, typeof(T));
        opHandle.Completed += (op) =>
        {
            if (op.Status == AsyncOperationStatus.Succeeded)
            {
                int loadCount = 0;
                int totalCount = op.Result.Count;

                foreach (var result in op.Result)
                {
                    Debug.Log($"loadstart {result.PrimaryKey} {loadCount}/{totalCount}");
                    if (result.PrimaryKey.EndsWith(".sprite"))
                    {
                        LoadAsync<Sprite>(result.PrimaryKey, (obj) =>
                        {
                            loadCount++;
                            callback?.Invoke(result.PrimaryKey, loadCount, totalCount);
                        });
                    }
                    else
                    {
                        LoadAsync<T>(result.PrimaryKey, (obj) =>
                        {
                            loadCount++;
                            callback?.Invoke(result.PrimaryKey, loadCount, totalCount);
                        });
                    }
                }
            }
            else
            {
                Debug.LogWarning($"Failed to load resource locations for label: {label}");
            }
        };
    }


    public void Clear()
	{
		_resources.Clear();

		foreach (var handle in _handles)
			Addressables.Release(handle);

		_handles.Clear();
	}
	#endregion
}
