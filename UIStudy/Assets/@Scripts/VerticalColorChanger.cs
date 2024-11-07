using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class VerticalColorChanger : MonoBehaviour
{
    private Image _image; 
    private float _duration = 1.5f; 

    private void Start()
    {
        _image = this.GetComponent<Image>();
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.A))
        {
            StartCoroutine(ChangeColor(Color.blue, Color.yellow, _duration));

        }
    }

   private IEnumerator ChangeColor(Color startColor, Color endColor, float _duration)
    {
        float elapsedTime = 0f;

        while (elapsedTime < _duration)
        {
            _image.color = Color.Lerp(startColor, endColor, elapsedTime / _duration);
            elapsedTime += Time.deltaTime; 
            yield return null; 
        }
        _image.color = endColor;
    }
}

