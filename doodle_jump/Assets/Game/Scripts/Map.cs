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
            // 30개를 처음에 만들고 그 뒤로 20개 제한을 두면 꺼지고난 다음엔 재활용이 안 됨.
            // 그게 아니더라도 몇번만 반복됨.
        }
    }
}
