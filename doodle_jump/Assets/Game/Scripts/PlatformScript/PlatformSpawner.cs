using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Pool;

public class PlatformSpawner : MonoBehaviour
{
    public ObjectPool<PlatformController> _pool;
    private PlatformController _platformRock;
    private PlatformController _platformWood;
    private PlatformController _platformBush;

    private int _maxPoolSize = 50;

    private float _platformSpawnPosY = 0;

    private int _spawnCount = 0;
    public int _SpawnCount { get { return _spawnCount; } }

    public ObjectPool<PlatformController> Pool
    {
        get
        {
            if (_pool == null)
            {
                _pool = new ObjectPool<PlatformController>(CreatePlatform, OnTakePlatformFromPool, 
                    OnReturnedPlatformToPool, OnDestroyPlatform, true, 50, _maxPoolSize);
            }
            return _pool;
        }
    }

    private PlatformController CreatePlatform()
    {
        PlatformController _createPlatform = RandomPlatform();
        _createPlatform.SetPool(_pool);
        return _createPlatform;
    }

    private void OnTakePlatformFromPool(PlatformController platform)
    {
        _platformSpawnPosY += 0.3f * Random.Range(1f, 2f);
        float _platformSpawnPosX = Random.Range(-1.5f, 1.5f);
        platform.transform.position = new Vector2(_platformSpawnPosX, _platformSpawnPosY);
        platform.gameObject.SetActive(true);
        _spawnCount++;
    }

    private void OnReturnedPlatformToPool(PlatformController platform)
    {
        platform.gameObject.SetActive(false);
        _spawnCount--;
    }

    private void OnDestroyPlatform(PlatformController platform)
    {
        Destroy(platform.gameObject);
    }

    private void Awake()
    {
        _platformRock = Resources.Load<PlatformController>("Prefabs/platform_rock");
        _platformWood = Resources.Load<PlatformController>("Prefabs/platform_wood");
        _platformBush = Resources.Load<PlatformController>("Prefabs/platform_bush");
        //_pool = new ObjectPool<PlatformController>(CreatePlatform, OnTakePlatformFromPool, OnReturnedToPool, OnDestroyPlatform, true, 20, _maxPoolSize);
    }

    private PlatformController RandomPlatform()
    {
        float _random = Random.Range(0, 1.0f);

        PlatformController _template = null;

        if (_random <= 0.25f)
        {
            _template = Instantiate(_platformWood);
            // 시작 좌표 설정이 필요..
        }
        else if (_random <= 0.55f)
        {
            _template = Instantiate(_platformBush);
        }
        else if (_random <= 1f)
        {
            _template = Instantiate(_platformRock);
        }
        return _template;
    }
}
