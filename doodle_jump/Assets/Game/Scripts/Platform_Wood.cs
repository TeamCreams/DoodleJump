using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Platform_Wood : PlatformController
{
    private int _direction = 1;
    // Start is called before the first frame update

    protected override void Start()
    {
        base.Start();
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
        StartCoroutine(BushMovement());
    }

    IEnumerator BushMovement()
    {
        while (true)
        {
            Vector3 _startPos = transform.position;
            Vector3 _endPos = _startPos + new Vector3(_direction, 0, 0);
            float _time = 0;

            while (_time < 0.8f)
            {
                _time += Time.deltaTime;
                float t = Mathf.Clamp01(_time / 0.8f);
                transform.position = Vector3.Lerp(_startPos, _endPos, t);
                yield return null;
            }
            _direction *= -1;
            yield return new WaitForSeconds(0.5f);
        }
    }
}
