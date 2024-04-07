using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.Pool;

public class CreateRemovePlatform : MonoBehaviour
{
    private InGameScene _gameScene;
    private GameObject _platformRock;
    private GameObject _platformWood;
    private GameObject _platformBush;

    private int _poolSize = 100;
    private List<GameObject> _pool = new List<GameObject>();

    private float _platformSpawnPosY = 0;

    private float _activeTime = 1.5f;
    private float _time = 0;

    // Start is called before the first frame update
    void Start()
    {
        _gameScene = Util.FindChildWithPath<InGameScene>("@InGameScene");
    }
    void Awake()
    {
        _platformRock = Resources.Load<GameObject>("Prefabs/platform_rock");
        _platformWood = Resources.Load<GameObject>("Prefabs/platform_wood");
        _platformBush = Resources.Load<GameObject>("Prefabs/platform_bush");

        for (int i = 0; i < _poolSize; i++) 
        {
            float _random = Random.Range(0, 1.0f);
            GameObject _template = null;
            if(_random <= 0.3f)
            {
                _template = Instantiate(_platformBush);
            }
            else if(_random <= 0.6f)
            {
                _template = Instantiate(_platformWood);
            }
            else if(_random <= 1f)
            {
                _template = Instantiate(_platformRock);
            }
            _template.SetActive(false);
            _pool.Add(_template);
        }

        for(int i = 0; i < 20; i++)
        {
            SpawnPlatforms();
        }
    }

    // Update is called once per frame
    void Update()
    { 
        _time += Time.deltaTime;
        if(_time > _activeTime)
        {
            SpawnPlatforms();
            _time = 0;
        }
        ReturnObjectToPool();
    }

    private GameObject GetObjectFromPool()
    {
        foreach (GameObject _platformObject in _pool)
        {
            if (!_platformObject.activeSelf)
            {
                _platformObject.SetActive(true);
                return _platformObject;
            }
        }
        return null;
    }

    private void ReturnObjectToPool()
    {
        foreach (GameObject _platformObject in _pool)
        {
            if (_platformObject.activeSelf)
            {
                if(_platformObject.transform.position.y + 3 < _gameScene.PlayerController.transform.position.y)
                {
                    _platformObject.SetActive(false);
                }
            }
        }
    }

    private void SpawnPlatforms()
    {
        GameObject _platform = GetObjectFromPool();
        if(_platform != null)
        {
            _platform.transform.position = SpawnPosition();
        }
    }

    private UnityEngine.Vector2 SpawnPosition()
    {
        _platformSpawnPosY += 0.3f * Random.Range(1f, 1.6f);
        float _platformSpawnPosX = Random.Range(-1.5f, 1.5f);
        return new UnityEngine.Vector2(_platformSpawnPosX, _platformSpawnPosY);
    }
}
