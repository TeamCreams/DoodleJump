using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Platform_Wood : MonoBehaviour 
{
    private int _direction = 1;
    // Start is called before the first frame update
    void Start()
    {
        this.GetComponent<BoxCollider2D>().isTrigger = true;

        int _random = Random.Range(0, 2);
        switch(_random)
        {
            case 0:
                _direction = 1;
                break;
            case 1:
                _direction = -1;
                break;
        }
        StartCoroutine(PlatformMoving());

        /*
         if(UnityEngine.Random.Range(0, 5) == 0)
        {
            Debug.LogError("error!");
            Util.WhoAmI(gameObject);
        }
         */
    }

    IEnumerator PlatformMoving()
    {
        Vector3 _startPos = transform.position;
        Vector3 _endPos = _startPos + new Vector3(_direction, 0, 0);
        float _time = 0;

        while (_time < 0.5f)
        {
            _time += Time.deltaTime;
            float t = Mathf.Clamp01(_time / 0.8f); 
            transform.position = Vector3.Lerp(_startPos, _endPos, t);
            yield return null;
        }

        _direction *= -1; 

        yield return new WaitForSeconds(0.8f); 
        StartCoroutine(PlatformMoving());
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (this.transform.position.y < collision.transform.position.y)
            {
                this.GetComponent<BoxCollider2D>().isTrigger = false;
            }
        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.position.y < this.transform.position.y)
        {
            this.GetComponent<BoxCollider2D>().isTrigger = true;
        }
    }

}
