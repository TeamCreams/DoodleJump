using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;

public class Platform_Wood : PlatformController
{
    private int _direction = 1;
    private bool _isSecond = false;
    protected override void Start()
    {
        base.Start();
        StartCoroutine(WoodMovement());
    }
    private void OnDisable()
    {
        _isSecond = true;
    }
    protected override void OnEnable()
    {
        base.OnEnable();
        int _random = Random.Range(0, 2);
        switch (_random)
        {
            case 0:
                _direction = 1;
                break;
            case 1:
                _direction = -1;
                break;
        }
        if(_isSecond)
        {
            //Debug.Log("Second Time");
            StartCoroutine(WoodMovement());
        }
    }
    public override void SetPool(ObjectPool<PlatformController> pool)
    {
        base._pool = pool;
    }

    IEnumerator WoodMovement()
    {
        while (true)
        {
            Vector3 _startPos = transform.position;
            // CreatePlatform -> WoodMovement -> OnTakePlatformFromPool ->  Get
            // _startPos가 (0,0,0)으로 들어감. -> start에 넣는 걸로 어케저케 해결
            Vector3 _endPos = _startPos + new Vector3(_direction, 0, 0);
            float _time = 0;

            while (_time < 1f)
            {
                _time += Time.deltaTime;
                float t = Mathf.Clamp01(_time / 1f);
                transform.position = Vector3.Lerp(_startPos, _endPos, t);
                yield return null;
            }
            _direction *= -1;
            yield return null;
        }
    }
}
