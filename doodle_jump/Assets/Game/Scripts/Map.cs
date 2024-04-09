using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    private PlatformSpawner _platformSpawner;
    private InGameScene _gameScene;

    // Start is called before the first frame update
    void Start()
    {
        _platformSpawner = GetComponent<PlatformSpawner>();
        _gameScene = Util.FindChildWithPath<InGameScene>("@InGameScene");
        
          for (int i = 0; i < 20; i++)
        {
            _platformSpawner._pool.Get();
        }
    }

    // Update is called once per frame
    void Update()
    {
        PlatformSpawnInGame();
    }

    private void PlatformSpawnInGame()
    {
        if(_platformSpawner._SpawnCount < 15) // Input.GetKeyDown(KeyCode.P)
        {
            _platformSpawner._pool.Get();
            //��� ������� ����. ����� �ݺ���.->Release �ڷ�ƾ�� ù start �ÿ��� ȣ���. ����
        }
    }
}
