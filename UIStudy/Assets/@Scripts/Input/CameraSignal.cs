using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSignal : ObjectBase
{

    Camera _camera;

    public override bool Init()
    {
        if (base.Init() == false)
        {
            return false;
        }
        _camera = this.GetComponent<Camera>();

        return true;
    }
    public void TestCameraShake()
    {
        StartCoroutine(ShakeCo(1.0f, 0.2f));
    }

    IEnumerator ShakeCo(float shakePower, float shakeDuration)
    {
        Vector3 cameraPos = _camera.transform.position;
        float timer = 0.0f;
        while (timer < shakeDuration)
        {
            float x = Random.Range(-1.0f, 1.0f);
            float y = Random.Range(-1.0f, 1.0f);

            x *= shakePower;
            y *= shakePower;

            Vector3 newCameraPos = cameraPos + new Vector3(x, y, 0);
            _camera.transform.position = newCameraPos;

            timer += Time.unscaledDeltaTime;
            yield return null;
        }
        yield return null;
    }
}
