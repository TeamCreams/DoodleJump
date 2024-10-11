using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class ShakeSlave : MonoBehaviour
{
    Coroutine _shakeCo = null;

    private void OnDisable()
    {
        if (_shakeCo != null)
        {
            StopCoroutine(_shakeCo);
        }
    }

    public void Shake(float shakePower, float shakeDuration)
    {
        _shakeCo = StartCoroutine(ShakeCo(shakePower, shakeDuration));
    }

    IEnumerator ShakeCo(float shakePower, float shakeDuration)
    {
        Vector3 cameraPos = Camera.main.transform.position;
        float timer = 0.0f;
        while(timer < shakeDuration)
        {
            float x = UnityEngine.Random.Range(-1.0f, 1.0f);
            float y = UnityEngine.Random.Range(-1.0f, 1.0f);

            x *= shakePower;
            y *= shakePower;

            Vector3 newCameraPos = cameraPos + new Vector3(x, y, 0);
            Camera.main.transform.position = newCameraPos;

            timer += Time.unscaledDeltaTime;
            yield return null;
        }

        Managers.Resource.Destroy(this.gameObject);
        yield return null;
    }
}
