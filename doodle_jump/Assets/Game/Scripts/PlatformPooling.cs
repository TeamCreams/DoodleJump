using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class PlatformPooling : MonoBehaviour
{

    private InGameScene _gameScene;
    private GameObject _platformRock;
    private GameObject _platformWood;
    private GameObject _platformBush;

    private int _maxPoolSize = 100;

    private float _platformSpawnPosY = 0;

    private float _activeTime = 1.5f;
    private float _time = 0;

    private List<GameObject> activeObjects = new List<GameObject>();
    IObjectPool<GameObject> m_Pool;

    public IObjectPool<GameObject> Pool
    {
        get
        {
            if (m_Pool == null)
            {
                m_Pool = new ObjectPool<GameObject>(CreatePooledItem, OnTakeFromPool, OnReturnedToPool, OnDestroyPoolObject, true, 10, _maxPoolSize);
            }
            return m_Pool;
        }
    }

    GameObject CreatePooledItem()
    {
        GameObject _template = null;

        float _random = Random.Range(0, 1.0f);

        if (_random <= 0.3f)
        {
            _template = Instantiate(_platformBush);
        }
        else if (_random <= 0.6f)
        {
            _template = Instantiate(_platformWood);
        }
        else if (_random <= 1f)
        {
            _template = Instantiate(_platformRock);
        }
        var returnToPool = _template.AddComponent<ReturnToPool>();
        returnToPool.pool = Pool;
        return _template;
    }

    void OnReturnedToPool(GameObject system)
    {
        if (activeObjects.Contains(system))
        {
            activeObjects.Remove(system);
        }
        system.gameObject.SetActive(false);
    }

    void OnTakeFromPool(GameObject system)
    {
        if (!activeObjects.Contains(system))
        {
            activeObjects.Add(system);
        }
        system.gameObject.SetActive(true);
    }

    void OnDestroyPoolObject(GameObject system)
    {
        Destroy(system.gameObject);
    }

    private void SpawnPlatforms()
    {
        GameObject _platform = Pool.Get();
        if (_platform != null)
        {
            _platform.transform.position = SpawnPosition();
        }
    }

    private Vector2 SpawnPosition()
    {
        _platformSpawnPosY += 0.3f * Random.Range(1f, 2f);
        float _platformSpawnPosX = Random.Range(-1.5f, 1.5f);
        return new Vector2(_platformSpawnPosX, _platformSpawnPosY);
    }

    void Start()
    {
        _gameScene = Util.FindChildWithPath<InGameScene>("@InGameScene");
    }
    private void Awake()
    {
        _platformRock = Resources.Load<GameObject>("Prefabs/platform_rock");
        _platformWood = Resources.Load<GameObject>("Prefabs/platform_wood");
        _platformBush = Resources.Load<GameObject>("Prefabs/platform_bush");
    }
    void Update()
    {
        if (activeObjects.Count < 30)
        {
            SpawnPlatforms();
        }
    }
}
