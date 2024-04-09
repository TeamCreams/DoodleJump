using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    private PlatformSpawner _platformSpawner;
    // Start is called before the first frame update
    void Start()
    {
        _platformSpawner = GetComponent<PlatformSpawner>();
        for (int i = 0; i < 30; i++)
        {
            _platformSpawner.Pool.Get();
        }
    }

    // Update is called once per frame
    void Update()
    {
        PlatformSpawnInGame();
    }

    private void PlatformSpawnInGame()
    {
        if(_platformSpawner._SpawnCount < 20)
        {
            _platformSpawner.Pool.Get(); 
            // 30���� ó���� ����� �� �ڷ� 20�� ������ �θ� ������ ������ ��Ȱ���� �� ��.
            // �װ� �ƴϴ��� ����� �ݺ���.
        }
    }
}
