using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

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
            _platformSpawner.GetPool();
        }
    }

    // Update is called once per frame
    void Update()
    {
        PlatformSpawnInGame();
    }

    private void PlatformSpawnInGame()
    {
        if(_platformSpawner._SpawnCount < 18) // Input.GetKeyDown(KeyCode.P)
        {
            _platformSpawner.GetPool();
            //��� ������� ����. ����� �ݺ���.->Release �ڷ�ƾ�� ù start �ÿ��� ȣ���. ����
        }
    }
}
